# computer-vision

## OpenCV

### Sample test

Test Environment
- OpenCV version: 4.8.0
- OS: Windows 11(22H2)
- IDE: Visual Studio 2022

Install OpenCV Libraries(Easy way to learn)
- Download OpenCV(Pre-built Libraries): https://opencv.org/releases/
- Unpack downloaded file
- Add it to the systems path on Terminal(run as  Administrator)
```
# Add OpenCV Path
setx OpenCV_DIR [Installed path, ex: "C:\OpenCV\build\x86\vc16"]
```

Sample Project on VS2022
- Create a new project
- Add the path to OpenCV include
```
"Additional Include Directories" [ex: "$(OPENCV_DIR)\..\..\include"]
```
- Add the libs directory
```
"Additional Library Directories" [ex: "$(OPENCV_DIR)\lib"]
```
- Add the name of all modules which you want to use
```
"Additional Dependencies" [ex: "opencv_world480d.lib"]
```