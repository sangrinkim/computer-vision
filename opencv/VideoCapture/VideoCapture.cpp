// VideoCapture.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <opencv2/core.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>
#include <stdio.h>

int main()
{
    cv::Mat frame;
    cv::VideoCapture cap;
    cv::String filename; // Windows: "C:\\Temp\\abc.mp4" | requirement : opencv_videoio_ffmpeg480_64.dll
    
    int deviceID = 0;
    int apiID = cv::CAP_ANY;

    if (filename.empty()) {
        cap.open(deviceID, apiID);
    }
    else {
        cap.open(filename, apiID);
    }

    if (!cap.isOpened()) {
        std::cerr << "ERROR! Unable to open camera\n";
        return -1;
    }

    std::cout << "Start grabbing" << std::endl;
    while (true) {
        cap.read(frame);
        if (frame.empty()) {
            std::cerr << "ERROR! blank frame grabbed\n";
            break;
        }

        cv::imshow("Live", frame);
        if (cv::waitKey(5) >= 0) {
            break;
        }
    }
    return 0;
}