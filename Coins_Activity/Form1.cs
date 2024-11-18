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

        private void Edging(object sender, EventArgs e)
        {
            
        }

        private void CalculateBtn(object sender, EventArgs e)
        {
            int totalCount = 0;
            int peso5Count = 0, peso1Count = 0, cent25Count = 0, cent10Count = 0, cent5Count = 0;
            float totalValue = 0;

            Library.GetCoinPixels(
                processed,
                ref totalCount,
                ref totalValue,
                ref peso5Count,
                ref peso1Count,
                ref cent25Count,
                ref cent10Count,
                ref cent5Count
            );

            fivecentavo.Text = $"5 Centavo: {cent5Count}";
            tencentavo.Text = $"10 Centavo: {cent10Count}";
            twentyfivecentavo.Text = $"25 Centavo: {cent25Count}";
            onepeso.Text = $"1 Peso: {peso1Count}";
            fivepeso.Text = $"5 Peso: {peso5Count}";
            resultLabel.Text = $"{totalValue:F2} PHP";
        }


        private void Load_ImgBtn(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
