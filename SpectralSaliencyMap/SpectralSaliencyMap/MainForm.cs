using Accord.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpectralSaliencyMap
{
    public partial class MainForm : Form
    {
        private Bitmap loadedImage;

        public MainForm()
        {
            InitializeComponent();
            SetOpenFileDialog();
        }

        private void SetOpenFileDialog()
        {
            openFileDialog1.Title = "Select Image";
            openFileDialog1.Filter = "Image Files(*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    loadedImage = AForge.Imaging.Image.FromFile(openFileDialog1.FileName);
                    txtFilePath.Text = openFileDialog1.FileName;
                    pictureBox1.Image = loadedImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Can't read file from disk.\n" + ex.Message);
                }
            }
        }

        private void btnSilencyMap_Click(object sender, EventArgs e)
        {
            if (loadedImage == null)
            {
                MessageBox.Show("Please, Select Image..");
                return;
            }

            pictureBox2.Image = SaliencyMap.GetTransferImage(loadedImage, 512);
        }
    }
}
