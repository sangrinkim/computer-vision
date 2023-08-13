// SampleTest1.cpp : This file contains the 'main' function. Program execution begins and ends there.
//


#include <opencv2/core.hpp>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/highgui.hpp>
#include <iostream>


int main(int argc, char** argv)
{
    if (argc != 2) {
        std::cout << " Usage: " << argv[0] << " ImageToLoadAndDisplay" << std::endl;
        return -1;
    }

    cv::Mat image;
    image = cv::imread(argv[1], cv::IMREAD_COLOR);  // Read the file

    // Check for invalid input
    if (image.empty()) {
        std::cout << "Could not open or find the image" << std::endl;
        return -1;
    }

    cv::namedWindow("Display window", cv::WINDOW_AUTOSIZE); // Create a window for display.
    cv::imshow("Display window", image);    // Show our image inside it.

    cv::waitKey(0); // Wait for a keystroke in the window
    return 0;
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
