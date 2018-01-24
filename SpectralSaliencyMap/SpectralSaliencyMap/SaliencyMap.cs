using System;
using System.Drawing;
using System.Windows.Forms;

using AForge.Imaging;
using AForge.Imaging.Filters;

using Accord.Imaging;
using AForge.Math;


namespace SpectralSaliencyMap
{
    public class SaliencyMap
    {
        private static int IMAGE_SIZE_X = 512;
        private static int IMAGE_SIZE_Y = 512;


        public SaliencyMap()
        {

        }

        public static Bitmap GetTransferImage(Bitmap srcImage, int imageSize)
        {
            if ((imageSize & (imageSize - 1)) != 0)
                throw new ArgumentException("이미지 사이즈는 2의 n승이여야 합니다.");
            else
            {
                IMAGE_SIZE_X = imageSize;
                IMAGE_SIZE_Y = imageSize;
            }

            AForge.Imaging.ComplexImage fft2Image = GetFFT2(srcImage);
            double[,] logAmp = GetLAmp(fft2Image);
            double[,] phase = GetPhase(fft2Image);
            Bitmap sr = GetSR(logAmp);
            Bitmap salMap = GetSalMap(sr, phase);

            var filt2 = new Accord.Imaging.Filters.GaussianBlur(2.5, 9);
            filt2.ApplyInPlace(salMap);

            var filtNorm = new Accord.Imaging.Filters.ContrastStretch();
            filtNorm.ApplyInPlace(salMap);

            return salMap;
        }

        public static AForge.Imaging.ComplexImage GetFFT2(Bitmap srcImage)
        {
            var complexImage = AForge.Imaging.ComplexImage.FromBitmap(ResizeImage(ToGrayScale(srcImage)));
            AForge.Math.FourierTransform.FFT2(complexImage.Data, AForge.Math.FourierTransform.Direction.Forward);

            return complexImage;
        }

        private static double[,] GetLAmp(AForge.Imaging.ComplexImage fft2Image)
        {
            AForge.Imaging.ComplexImage complexImage = fft2Image;

            double[,] logAmpl = new double[IMAGE_SIZE_X, IMAGE_SIZE_Y];
            for (int x = 0; x < IMAGE_SIZE_X; x++)
            {
                for (int y = 0; y < IMAGE_SIZE_Y; y++)
                {
                    // abs (z) = sqrt (x^2 + y^2)
                    // 따라서, 우리는 Complex.Magnitude로 구할 수 있다.
                    logAmpl[x, y] = Math.Log(complexImage.Data[x, y].Magnitude);
                }
            }

            return logAmpl;
        }

        private static double[,] GetPhase(AForge.Imaging.ComplexImage fft2Image)
        {
            AForge.Imaging.ComplexImage complexImage = fft2Image;

            double[,] phase = new double[IMAGE_SIZE_X, IMAGE_SIZE_Y];
            for (int x = 0; x < IMAGE_SIZE_X; x++)
            {
                for (int y = 0; y < IMAGE_SIZE_Y; y++)
                {
                    phase[x, y] = complexImage.Data[x, y].Phase;
                }
            }

            return phase;
        }

        private static Bitmap GetSR(double[,] logAmp)
        {
            Bitmap amp = logAmp.ToBitmap();

            Accord.Imaging.Filters.FastBoxBlur imfilter = new Accord.Imaging.Filters.FastBoxBlur(3, 3);
            Bitmap smoothAmp = imfilter.Apply(amp);

            AForge.Imaging.Filters.Subtract filter = new AForge.Imaging.Filters.Subtract(smoothAmp);
            Bitmap specResi = filter.Apply(amp);
            
            return specResi;
        }

        private static Bitmap GetSalMap(Bitmap sr, double[,] phase)
        {
            AForge.Imaging.ComplexImage totalSpec = AForge.Imaging.ComplexImage.FromBitmap(sr);
            
            // exp(SR + 1i * Phase)
            for (int x = 0; x < IMAGE_SIZE_X; x++)
            {
                for (int y = 0; y < IMAGE_SIZE_Y; y++)
                {
                    totalSpec.Data[x, y].Im = phase[x, y];
                    totalSpec.Data[x, y] = Complex.Exp(totalSpec.Data[x, y]);
                }
            }

            totalSpec.ForwardFourierTransform();

            // .^2
            for (int x = 0; x < IMAGE_SIZE_X; x++)
            {
                for (int y = 0; y < IMAGE_SIZE_Y; y++)
                {
                    totalSpec.Data[x, y] =
                        Complex.Multiply(totalSpec.Data[x, y], totalSpec.Data[x, y]);
                }
            }

            // shift
            Complex[,] shifted = Shift(totalSpec.Data, IMAGE_SIZE_X, IMAGE_SIZE_Y);
            for (int x = 0; x < IMAGE_SIZE_X; x++)
                for (int y = 0; y < IMAGE_SIZE_Y; y++)
                    totalSpec.Data[x, y] = shifted[x, y];

            return totalSpec.ToBitmap();
        }

        public static Complex[,] Shift(Complex[,] data, int width, int height)
        {
            var shifted = new Complex[width, height];
            for (int x = 0; x <= width / 2 - 1; x++)
            { 
                for (int y = 0; y <= height / 2 - 1; y++)
                {
                    shifted[x, y] = data[width / 2 - x, height / 2 - y];
                    shifted[x + width / 2, y] = data[width - 1 - x, height / 2 - y];
                    shifted[x, y + height / 2] = data[width / 2 - x, height -1 - y];
                    shifted[x + height / 2, y + height / 2] = data[width -1 - x, height - 1 - y];
                }
            }

            return shifted;
        }

        public static Bitmap ToGrayScale(Bitmap srcImage)
        {
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            return filter.Apply(srcImage);
        }

        public static Bitmap ResizeImage(Bitmap srcImage)
        {
            ResizeBilinear filter = new ResizeBilinear(IMAGE_SIZE_X, IMAGE_SIZE_Y);
            return filter.Apply(srcImage);
        }
    }
}
