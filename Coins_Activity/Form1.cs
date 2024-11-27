using AForge.Imaging.Filters;
using ImageProcess2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Coins_Activity
{
    public partial class Form1 : Form
    {
        Bitmap img, processed;
        Label fivecentavo, tencentavo, twentyfivecentavo, onepeso, fivepeso, resultLabel;

        public Form1()
        {
            InitializeComponent();
            fivecentavo = label1;
            tencentavo = label2;
            twentyfivecentavo = label3;
            onepeso = label4;
            fivepeso = label5;
            resultLabel = label6;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            img = new Bitmap(openFileDialog1.FileName);
            processed = new Bitmap(img);
            pictureBox1.Image = img;
        }

        private void CalculateBtn(object sender, EventArgs e)
        {
            if (img == null) return;

            Library library = new Library();
            processed = library.Helper(img);
            var coins = library.CoinCounter(processed);

            fivecentavo.Text = $"{coins["5 Centavo"]}";
            tencentavo.Text = $"{coins["10 Centavo"]}";
            twentyfivecentavo.Text = $"{coins["25 Centavo"]}";
            onepeso.Text = $"{coins["1 Peso"]}";
            fivepeso.Text = $"{coins["5 Peso"]}";
            resultLabel.Text = $"{coins["Value"]:F2} PHP";

            pictureBox2.Image = processed; // Display processed image
        }



        private void Load_ImgBtn(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
