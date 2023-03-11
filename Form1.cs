using Backprop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BackPropagation
{
    public partial class Form1 : Form
    {
        Bitmap loaded;
        Backprop.NeuralNet neuralNet;
        public Form1()
        {
            InitializeComponent();
            neuralNet = new NeuralNet(256, 400, 4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            neuralNet = new Backprop.NeuralNet();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }
        private void LearnHelper(string directory, int[] desiredOutputs)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string currentDirectory = Directory.GetParent(workingDirectory).Parent.FullName + "\\Shapes" + directory;
            var files = new DirectoryInfo(currentDirectory).GetFiles();
            int count = 0;
            foreach (var file in files)
            {
                using (var image = Image.FromFile(file.FullName))
                {
                    var histogram = ImageProcessing.ImageProcesses.Histogram((Bitmap)image);

                    for (int i = 0; i < histogram.Length; i++)
                    {
                        neuralNet.setInputs(i, histogram[i]);
                    }
                    neuralNet.setDesiredOutput(0, desiredOutputs[0]);
                    neuralNet.setDesiredOutput(1, desiredOutputs[1]);
                    neuralNet.setDesiredOutput(2, desiredOutputs[2]);
                    neuralNet.setDesiredOutput(3, desiredOutputs[3]);
                    Console.WriteLine($"Image {++count}");
                }
                neuralNet.learn();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string circleFiles = "\\circle";
            string squareFiles = "\\square";
            string starFiles = "\\star";
            string triangleFiles = "\\triangle";

            for (int i = 0; i < 5; i++)
            {
                LearnHelper(circleFiles, new int[] { 1, 0, 0, 0 });
                LearnHelper(squareFiles, new int[] { 0, 1, 0, 0 });
                LearnHelper(starFiles, new int[] { 0, 0, 1, 0 });
                LearnHelper(triangleFiles, new int[] { 0, 0, 0, 1 });
            }

            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var histogram = ImageProcessing.ImageProcesses.Histogram(loaded);

            for (int i = 0; i < histogram.Length; i++)
            {
                neuralNet.setInputs(i, histogram[i]);
            }
            neuralNet.run();
            double[] output = new double[] {
                neuralNet.getOuputData(0), 
                neuralNet.getOuputData(1), 
                neuralNet.getOuputData(2), 
                neuralNet.getOuputData(3)
            };
            double maxValue = output.Max();
            int maxIndex = output.ToList().IndexOf(maxValue);

            string outputText = "";

            switch (maxIndex)
            {
                case 0:
                    outputText = "Circle";
                    break;
                case 1:
                    outputText = "Square";
                    break;
                case 2:
                    outputText = "Star";
                    break;
                case 3:
                    outputText = "Triangle";
                    break;
                default:
                    break;
            }

            textBox1.Text = outputText;
        }
    }
}
