using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FClus
{
    public partial class Form1 : Form
    {
        private static Color[] COLORS = new Color[] { Color.RosyBrown, Color.SandyBrown, Color.LawnGreen, Color.BlueViolet, Color.Goldenrod, Color.Cyan, Color.DarkKhaki, Color.Crimson, Color.Maroon, Color.Teal, Color.IndianRed, Color.Violet, Color.OrangeRed, Color.Tan, Color.Sienna, Color.Olive, Color.LightGreen, Color.LightYellow };
        private Graphics gr;
        private List<Point> po = new List<Point>();
        private int algoType = 0;
        private int clustersNumber = 3;
        private double exp = 2;
        private double eps = 0.001;
        private double[] P;
        private bool vis = true;
        private int ii = 0;

        public Form1()
        {
            InitializeComponent();
            myInit();
        }

        private void myInit()
        {
            label5.Text = getValForLabel(trackBar1.Value);
            comboBox1.SelectedIndex = algoType;
            textBox1.Text = clustersNumber.ToString();
            textBox2.Text = exp.ToString();
            pictureBox1.Size = new Size(527, 448);
            pictureBox1.Location = new Point(194, 75);
            pictureBox1.Visible = true;
        }

        private string getValForLabel(int val)
        {
            string temp = "0.1";
            int t;
            if (val < 100 && val >= 75)
            {
                t = 100 - val;
                t = (int)(t / 2.5d);
                if (t == 0)
                    t = 1;
                temp = (0.1d / t).ToString();
            }
            else if (val < 75 && val >= 50)
            {
                t = 75 - val;
                t = (int)(t / 2.5d);
                if (t == 0)
                    t = 1;
                temp = (0.01d / t).ToString();
            }
            else if (val < 50 && val >= 25)
            {
                t = 50 - val;
                t = (int)(t / 2.5d);
                if (t == 0)
                    t = 1;
                temp = (0.001d / t).ToString();
            }
            else if(val < 25 && val >= 0)
            {
                t = 25 - val;
                t = (int)(t / 2.5d);
                if (t == 0)
                    t = 1;
                temp = (0.0001d / t).ToString();
            }
            return temp;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ii++;
            gr.FillEllipse(Brushes.DarkBlue, e.X, e.Y, 4.0f, 4.0f);
            po.Add(new Point(e.X, e.Y));
            if (ii >= clustersNumber)
            {
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Every group of points which belongs to the same CLUSTER will be in the same color", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            double[][] data = new double[po.Count][];
            double[][] res; 
            for (int f = 0; f < data.Length; f++)
            {
                data[f] = new double[2];
                data[f][0] = po[f].X;
                data[f][1] = po[f].Y;
            }
            CLUSTER c = new CLUSTER(data, clustersNumber, exp, eps);
            if(algoType==0)
                res = c.ClusterMyDataByC_Mean();
            else
                res = c.clusterByGK(P);
            gr.Clear(Color.WhiteSmoke);
            bool[][] cl=c.cluster_BY_data();
            for (int p = 0; p < data.Length; p++)
            {
                for (int q = 0; q < cl.Length; q++)
                {
                    
                    if (cl[q][p])
                    {
                        gr.FillEllipse(new SolidBrush(COLORS[q]), (float)data[p][0],(float) data[p][1], 4.0f, 4.0f);
                        break;
                    }
                }
            }
            button1.Enabled = false;
            po.Clear();
            ii = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadImage r = new ReadImage(@"G:\HandWriting\logo_4.bmp");
            double[][] data = r.getData;
            double[][] res;
            bool[][] mim;
            CLUSTER c = new CLUSTER(data, 6, 2, 0.0001);
            res = c.ClusterMyDataByC_Mean();
            mim = c.cluster_BY_data();
//            pictureBox1.BackgroundImage = r.IMAGE.Bitmap;
            UnsafeBitmap temp = new UnsafeBitmap(r.IMAGE.Width, r.IMAGE.Height);
            PixelData[] colors=new PixelData[6];
            colors[0] = new PixelData(Color.Coral.R, Color.Coral.G, Color.Coral.B);
            colors[1] = new PixelData(Color.Peru.R, Color.Peru.G, Color.Peru.B);
            colors[2] = new PixelData(Color.Goldenrod.R, Color.Goldenrod.G, Color.Goldenrod.B);
            colors[3] = new PixelData(Color.DarkKhaki.R, Color.DarkKhaki.G, Color.DarkKhaki.B);
            colors[4] = new PixelData(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B);
            colors[5] = new PixelData(Color.WhiteSmoke.R, Color.WhiteSmoke.G, Color.WhiteSmoke.B);
            temp.LockBitmap();
            for (int x = 0; x < mim[0].Length; x++)
            {
                for (int y = 0; y < mim.Length; y++)
                {
                    if (mim[y][x])
                    {
                        temp.SetPixel(x % temp.Width, x / temp.Width, colors[y]);
                        break;
                    }
                }
            }
  //          pictureBox2.BackgroundImage = temp.Bitmap;
            temp.UnlockBitmap();
            r.IMAGE.UnlockBitmap();
        }

        private void hideALL()
        {
            panel2.Visible = false;
            panel1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            label7.Visible = false;
            button3.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            vis = false;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name == "Node4")
            {
                hideALL();
                panel1.Location = new Point(200, 75);
                panel1.Size = new Size(607, 486);
                gr = panel1.CreateGraphics();
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                panel1.Visible = true;
                button1.Visible = true;
                string ClAlog;
                if (algoType == 0)
                    ClAlog = "C-MEAN";
                else ClAlog = "Gustafson-Kessel";
                label7.Text = "Clustering Algorithm: " + ClAlog + "\n";
                label7.Text += "Number of Clusters: " + clustersNumber + "\n";
                label7.Text += "Weighting Exponent: " + exp + "\n";
                label7.Text += "Termination Tolerance: " + eps + "\n";
                label7.Visible = true;
            }
            else if (e.Node.Name == "Node5" || e.Node.Name == "Node7")
            {
                hideALL();
                panel2.Location = new Point(233, 75);
                panel2.Size = new Size(336, 415);
                panel2.Visible = true;
            }
            else if (e.Node.Name == "Node6")
            {
                hideALL();
                pictureBox2.Location = new Point(200, 75);
                pictureBox2.Size = new Size(300, 300);
                pictureBox3.Location = new Point(510, 75);
                pictureBox3.Size = new Size(300, 300);
                pictureBox2.Visible = pictureBox3.Visible = true;
                string ClAlog;
                if (algoType == 0)
                    ClAlog = "C-MEAN";
                else ClAlog = "Gustafson-Kessel";
                label7.Text = "Clustering Algorithm: " + ClAlog + "\n";
                label7.Text += "Number of Clusters: " + clustersNumber + "\n";
                label7.Text += "Weighting Exponent: " + exp + "\n";
                label7.Text += "Termination Tolerance: " + eps + "\n";
                label7.Visible = true;
                button3.Visible = true;
            }
            else if (!vis)
            {
                hideALL();
                pictureBox1.Size = new Size(527, 448);
                pictureBox1.Location = new Point(194, 75);
                pictureBox1.Visible = true;
                vis = true;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label5.Text = getValForLabel(trackBar1.Value);
            eps = double.Parse(label5.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                comboBox2.Visible = true;
                textBox3.Visible = true;
                label6.Visible = true;
                P = new double[clustersNumber];
                comboBox2.Items.Clear();
                for (int i = 1; i <= clustersNumber; i++)
                {
                    comboBox2.Items.Add("Cluster " + i + " Volume");
                    P[i-1] = 1.0d;
                }
                algoType = 1;
            }
            else
            {
                comboBox2.Visible = false;
                textBox3.Visible = false;
                label6.Visible = false;
                algoType = 0;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = P[comboBox2.SelectedIndex].ToString();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            try
            {
                P[comboBox2.SelectedIndex] = double.Parse(textBox3.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "NOT ACCEPTED VALUE OF VOLUME", "VALUE ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clustersNumber = int.Parse(textBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "NOT ACCEPTED VALUE OF VOLUME", "VALUE ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                exp = double.Parse(textBox2.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(this, "NOT ACCEPTED VALUE OF VOLUME", "VALUE ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(this, "Every CLUSTER will be in a specific color", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                label8.Visible = true;
                //System.Threading.Thread.Sleep(20);
                this.Update();
                ReadImage r = new ReadImage(@openFileDialog1.FileName);
                double[][] data = r.getData;
                double[][] res;
                bool[][] mim;
                CLUSTER c = new CLUSTER(data, clustersNumber, exp, eps);
                if(algoType==0)
                    res = c.ClusterMyDataByC_Mean();
                else
                    res = c.clusterByGK(P);
                mim = c.cluster_BY_data();
                //System.
                pictureBox2.BackgroundImage = r.IMAGE.Bitmap;
                UnsafeBitmap temp = new UnsafeBitmap(r.IMAGE.Width, r.IMAGE.Height);
                temp.LockBitmap();
                for (int x = 0; x < mim[0].Length; x++)
                {
                    for (int y = 0; y < mim.Length; y++)
                    {
                        if (mim[y][x])
                        {
                            temp.SetPixel(x % temp.Width, x / temp.Width, new PixelData(COLORS[y].R, COLORS[y].G, COLORS[y].B));
                            break;
                        }
                    }
                }
                pictureBox3.BackgroundImage = temp.Bitmap;
                temp.UnlockBitmap();
                r.IMAGE.UnlockBitmap();
            }
            label8.Visible = false;
        }
    }
}
