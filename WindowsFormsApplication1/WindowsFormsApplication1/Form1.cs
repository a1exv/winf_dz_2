using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        StreamReader sr;
        string text;
        public Form1()
        {
            
            InitializeComponent();
            sr = new StreamReader("E:/Programms/step/c3/logger.txt");
            text = sr.ReadToEnd();
            progressBar1.Maximum = text.Length;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < text.Length; i++)
            {
                textBox1.Text += text[i].ToString();
                
                progressBar1.PerformStep();
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
