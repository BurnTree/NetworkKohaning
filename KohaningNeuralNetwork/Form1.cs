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

using KohaningNeuralNetwork.component;

namespace KohaningNeuralNetwork
{
    public partial class Form1 : Form
    {
        private Network _network;
        private Network _exNetwork;
        Table table;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var testBmp = new Bitmap(@"..\bmp\1\1.bmp");
            _network = new Network(testBmp.Width * testBmp.Height, 5);
            Random rand = new Random();
            int time = Convert.ToInt32(textBox1.Text);
            string str;
            int len;
            int answer;
            _network.Create();
            for (int i = 0; i < time; i++)
            {
                var kl = rand.Next(0, 3) + 1;
                Text = new DirectoryInfo(@"..\bmp\" + kl).GetFiles().Length.ToString();
                len = Convert.ToInt32(Text);
                var image = new Bitmap(@"..\bmp\" + kl + @"\" + (rand.Next(0, len) + 1) + ".bmp");
                var bmp = new Bitmap(image);
                var mass = ImgToMat(bmp);
                answer = _network.Handle(mass);
                _network.Study(mass, answer);

            }
        }

        public void writeLabel()
        {
            label2.Text = "";
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < _network._neurons.Length; j++)
                    label2.Text += Math.Round(_network._inputs[i].OutgoingLinks[j].Weight, 2) + " ";
                label2.Text += "\n";
            }
            label3.Text = Math.Round(_network._inputs[1].OutgoingLinks[2].Weight, 2) + " ";
        }

        public double[] ImgToMat(Bitmap img)
        {
            double[] mat = new double[img.Width * img.Height];
            for (int ik = 0; ik < img.Height; ik++)
                for (int jk = 0; jk < img.Width; jk++)
                {
                    Color c1 = img.GetPixel(jk, ik);
                    if (c1.R > 0)
                        mat[ik * 10 + jk] = 0;
                    else
                        mat[ik * img.Width + jk] = 1;
                }
            return mat;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ("Image Files(*.BMP)|*.BMP");
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var img = new Bitmap(ofd.FileName);
                    var mass = ImgToMat(img);
                    pictureBox1.Image = img;
                    var mat = ImgToMat(img);
                    var answer = _network.Handle(mat);
                    label1.Text = "Класс: " + (answer + 1);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        static public void openFile()
        {
            Excel excel = new Excel(@"test.xlsx", 1);
            Console.WriteLine(excel.read(1, 1));
            Console.ReadKey();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ("Excel Files(*.xlsx)|*.xlsx");
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    label3.Text = "";
                    for (int i = 0; i < table.height; i++)
                    {
                        for (int j = 0; j < table.width; j++)
                            label3.Text += Math.Round(table.norm[i, j], 2) + "   ";
                        label3.Text += "\n";
                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            readExToMAt(ofd.FileName);
            table.normalization();
            studyForText();
            //readExToMAt(@"tes");

        }

        public void studyForText()
        {
            int answer = 0;
            _network = new Network(table.width, Convert.ToInt32(textBox2.Text));
            _network.Create();
            int time = Convert.ToInt32(textBox3.Text);
            for (int k = 0; k < time; k++)
            {
                for (int i = 0; i < table.height; i++)
                {
                    var mass = new double[table.width];
                    for (int j = 0; j < mass.Length; j++)
                        mass[j] = table.norm[i, j];
                    answer = _network.Handle(mass);
                    _network.Study(mass, answer);
                }
            }
            label3.Text = "";
            for (int i = 0; i < table.height; i++)
            {
                var mass = new double[table.width];
                for (int j = 0; j < mass.Length; j++)
                    mass[j] = table.norm[i, j];
                answer = _network.Handle(mass);
                label3.Text += (answer + 1) + " \n";
            }
        }

        public void readExToMAt(string p)
        {
            Excel studyEx = new Excel(p, 1);
            table = new component.Table();
            int iterI = 2, iterJ = 2;
            while (studyEx.read(iterI, iterJ) != "")
            {
                table.width++;
                iterJ++;
            }
            iterJ = 2;
            while (studyEx.read(iterI, iterJ) != "")
            {
                table.height++;
                iterI++;
            }
            table.mat = new double[table.height, table.width];
            for (int i = 0; i < table.height; i++)
            {
                for (int j = 0; j < table.width; j++)
                {
                    table.mat[i, j] = Convert.ToDouble(studyEx.read(i + 2, j + 2));
                }
            }
            studyEx.close();
            label2.Text = "";
            for (int i = 0; i < table.height; i++)
            {
                for (int j = 0; j < table.width; j++)
                    label2.Text += table.mat[i, j] + " ";
                label2.Text += "\n";
            }

        }

        public void writeToEx(string name)
        {
            Excel ex = new Excel();
            ex.createNewFile();
            ex.createNewSheet();
            for (int i = 0; i < table.height; i++)
                for (int j = 0; j < table.width; j++)
                    ex.write(i, j, table.norm[i, j].ToString());
            ex.saveAs(@"..\" + name + ".xlsx");
            ex.close();
            label3.Text = "";
            for (int i = 0; i < table.width; i++)
            {
                for (int j = 0; j < table.height; j++)
                    label3.Text += table.norm[i, j] + " ";
                label3.Text += "\n";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            double[] mas = { };

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ("Excel Files(*.xlsx)|*.xlsx");
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    test(ofd.FileName);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            mas = readExAndNorm(ofd.FileName);
            var answer = _network.Handle(mas);
            label1.Text = "Класс: " + (answer + 1);
        }

        private void test(string p)
        {
            label3.Text = "";
            Excel studyEx = new Excel(p, 1);
            double[] m = new double[table.width];
            for (int i = 0; i < m.Length; i++)
            {
                label3.Text += studyEx.read(2, i + 1) + " ";
            }
        }

        private double[] readExAndNorm(string p)
        {
            Excel studyEx = new Excel(p, 1);
            double[] m = new double[table.width];
            for (int i = 0; i < m.Length; i++)
            {
                m[i] = Convert.ToDouble(studyEx.read(2, i + 1));
            }

            double min = 1000;
            double max = -1000;
            for (int j = 0; j < table.width - 1; j++)
            {
                for (int i = 0; i < table.height; i++)
                {
                    if (table.mat[i, j] < min) min = table.mat[i, j];
                    if (table.mat[i, j] > max) max = table.mat[i, j];
                }
                if (max != min)
                    m[j] = (m[j] - min) / (max - min);
                else
                    m[j] = 1;
                min = 1000;
                max = -1000;
            }

            return m;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
