# ObjectTracingInVideoImage
.NET project for masters thesis: "Object tracing and trajectory analysis in a video image"
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
