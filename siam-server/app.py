from flask import Flask, request, jsonify, Response
import json
import numpy as np
import cv2
import torch
from os.path import isfile
from tools.test import load_config, siamese_init, siamese_track, load_pretrain
from custom import Custom
import time

MODEL_PATH = '../experiments/siammask_sharp/SiamMask_DAVIS.pth'
CONFIG_PATH = '../experiments/siammask_sharp/config_davis.json'
USE_CPU = False

app = Flask(__name__)

tracker_state = None
siammask = None
cfg = None
device = None

def init_model():
    global siammask, cfg, device
    device = torch.device('cpu' if USE_CPU else ('cuda' if torch.cuda.is_available() else 'cpu'))
    torch.backends.cudnn.benchmark = True
    args = type('Args', (), {
        'config': CONFIG_PATH,
        'resume': MODEL_PATH,
        'cpu': USE_CPU
    })()
    cfg = load_config(args)
    siammask_net = Custom(anchors=cfg['anchors'])
    assert isfile(MODEL_PATH), f'Please download {MODEL_PATH} first.'
    siammask_net = load_pretrain(siammask_net, MODEL_PATH)
    siammask_net.eval().to(device)
    siammask = siammask_net

@app.route('/init', methods=['POST'])
def api_init():
    global tracker_state, siammask, cfg, device
    file = request.files['image']
    bbox = request.form['bbox']
    x, y, w, h = map(float, bbox.split(','))

    npimg = np.frombuffer(file.read(), np.uint8)
    im = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    target_pos = np.array([x + w/2, y + h/2])
    target_sz = np.array([w, h])
    tracker_state = siamese_init(im, target_pos, target_sz, siammask, cfg['hp'], device=device)
    return jsonify({'result': 'ok'})

@app.route('/track', methods=['POST'])
def api_track():
    start = time.perf_counter()
    global tracker_state, siammask, cfg, device
    if tracker_state is None:
        return jsonify({'error': 'Tracker not initialized'}), 400
    file = request.files['image']
    npimg = np.frombuffer(file.read(), np.uint8)
    im = cv2.imdecode(npimg, cv2.IMREAD_COLOR)

    tracker_state = siamese_track(tracker_state, im, mask_enable=True, refine_enable=True, device=device)
    polygon = tracker_state['ploygon'].flatten().tolist()

    xs = polygon[::2]
    ys = polygon[1::2]
    min_x = min(xs)
    min_y = min(ys)
    max_x = max(xs)
    max_y = max(ys)
    rect_bbox = [float(min_x), float(min_y), float(max_x - min_x), float(max_y - min_y)]
    response = Response(json.dumps(rect_bbox), mimetype="application/json")
    elapsed = time.perf_counter() - start

    print(f"SiamMask track: {elapsed:.3f} s")
    return response

if __name__ == '__main__':
    init_model()
    app.run(host='0.0.0.0', port=5000)
