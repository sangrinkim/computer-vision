using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectralSaliencyMap
{
    class Common
    {
        /// <summary>
        /// Resize Bitmap Image
        /// </summary>
        /// <param name="image">target image</param>
        /// <param name="width">target image width</param>
        /// <param name="height">target image height</param>
        /// <returns>Resized image</returns>
        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(image, 0, 0, width, height);
            }

            return result;
        }
    }
}
