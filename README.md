# ObjectTracingInVideoImage
.NET project for masters thesis: "Object tracing and trajectory analysis in a video image"

## Description
The application allows the user to select any object in a video frame and then track it using a chosen algorithm. In addition to the methods described in the literature, such as KCF, CSRT, MIL, and SiamMask, a custom Hybrid Algorithm is also available, combining the previously mentioned methods and further enhancing tracking with a Kalman filter. The tracking process is performed in real time and does not require GPU computations, thanks to the use of lightweight methods.

The detected object is marked with a red rectangle, while its ground-truth position is shown in green. To evaluate the method using IoU, it is necessary to use videos from the [LaSOT dataset](https://arxiv.org/abs/2009.03465), which is available for free. The blue and red lines visualize the ground-truth position of the object's center and the Kalman filter’s predicted position, respectively.

## Application demo
[Example of tracking](https://github.com/user-attachments/assets/086340dc-f343-4250-aaa4-cd82ba5c748b)


## SiamMask server
This project uses the [SiamMask](https://github.com/foolwood/SiamMask) tracker.  
SiamMask is released under the MIT License (see the original [license file](https://github.com/foolwood/SiamMask/blob/master/LICENSE)).

In order to run the SiamMask-based server:

1. Clone the original SiamMask repository:
`git clone https://github.com/foolwood/SiamMask`
2. Place the `app.py` file from this repository into the `tools` directory of SiamMask (as a replacement for or alongside the original `demo.py`).
3. Run the server with:
`python app.py`
Make sure all other dependencies and model files are set up according to the original [SiamMask instructions](https://github.com/foolwood/SiamMask).

> **Disclaimer:**  
> This project provides only a wrapper/server for SiamMask, which remains the intellectual property of its original authors.  
> Please refer to the [original repository](https://github.com/foolwood/SiamMask) for licensing and usage terms.  
> This code is intended for research and educational purposes only.
