using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDotNet;

namespace WindowsFormsApp1
{    

    public partial class Form1 : Form
    {       

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Hide();
            label4.Hide();
            label5.Hide();
            label6.Hide();
            label3.Hide();
            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            textBox4.Hide();
            textBox5.Hide();
            comboBox2.Hide();
            textBox6.Hide();

        }
        
        string rExe(REngine engine,string expression)
        {            
            try
            {
                var res = engine.Evaluate(expression);
                var test = res.AsNumeric();
                if (test == null)
                {
                    return "";
                }
                else
                {
                    var ret = test.First();
                    return Convert.ToString(ret);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Expressão com erro. Tente novamente");
                return "";
            }
                        
            
        }

        void rPlot(REngine engine, string expression,PictureBox picBox)
        {
            engine.Evaluate("jpeg(\"rplot.jpg\")");
            try
            {
                engine.Evaluate(expression);
            }
            catch (Exception e)
            {
                MessageBox.Show("Expressão com erro. Tente novamente");
            }            
            engine.Evaluate("dev.off()");           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            REngine.SetEnvironmentVariables();                        
            REngine engine = REngine.GetInstance();
            pictureBox1.Hide();
            string rExp;
            string input;
            #region METODO DA BISSEÇÃO
            if(comboBox1.SelectedItem.Equals("Método da Bisseção"))
            {
                
                int count=1;
                string aT = textBox2.Text;
                string bT = textBox3.Text;
                string bl = "\r\n";
                float a, b;
                float valorReal = float.Parse(textBox4.Text);
                float midT;
                float mid;
                float Stop = float.Parse(textBox5.Text);
                string its="";
                input = textBox1.Text;

                if (comboBox2.SelectedItem.Equals("EA"))
                {
                    its += input + bl;
                    its += "[a,b] = [" + aT + "," + bT + "]" + bl;

                    do
                    {
                        its += "-> Iteração " + Convert.ToString(count)+bl+bl;
                        count++;

                        

                        rExp = "x=" + aT + ";" + input;
                        a = float.Parse(rExe(engine, rExp));
                        its += "f("+aT+") = " + Convert.ToString(a);
                        if (a < 0)
                            its += " < 0"+bl;
                        else
                            its += " > 0"+bl;

                        rExp = "x=" + bT + ";" + input;
                        b = float.Parse(rExe(engine, rExp));
                        its += "f("+bT+") = " + Convert.ToString(b);
                        if (b < 0)
                            its += " < 0"+bl;
                        else
                            its += " > 0"+bl;

                        midT = (float.Parse(aT) + float.Parse(bT)) / 2;
                        rExp = "x=" + Convert.ToString(midT) + ";" + input;
                        mid = float.Parse(rExe(engine, rExp));
                        its += "(a+b)/2 = " + Convert.ToString(midT)+bl;
                        its += "f("+midT+") = " + Convert.ToString(mid);
                        if (mid < 0)
                            its += " < 0" + bl;
                        else
                            its += " > 0" + bl;
                        its += "Erro: " + Math.Abs(valorReal - midT)+bl;

                        
                        
                        if ((mid>0 && b>0 && a<0)|| (mid < 0 && b < 0 && a > 0))
                        {
                            bT = Convert.ToString(midT);
                        }
                        else
                        {
                            aT = Convert.ToString(midT);
                        }                        
                    } while ( Math.Abs(valorReal-midT)> Stop);
                    its += bl+"Resultado: " + midT + bl;
                    its += bl;
                } else if (comboBox2.SelectedItem.Equals("ER"))
                {
                    do
                    {
                        rExp = "x=" + aT + ";" + input;
                        a = float.Parse(rExe(engine, rExp));

                        rExp = "x=" + aT + ";" + input;
                        b = float.Parse(rExe(engine, rExp));
                        midT = (float.Parse(aT) + float.Parse(bT)) / 2;
                        rExp = "x=" + Convert.ToString(midT) + ";" + input;
                        mid = float.Parse(rExe(engine, rExp));
                    } while ((Math.Abs(valorReal - mid)/valorReal) > Stop);
                }
                else if (comboBox2.SelectedItem.Equals("Nº de iterações"))
                {

                }
                
                textBox6.Show();
                textBox6.Text = its;
            }
            #endregion
            if (textBox1.Text.Contains("curve") || textBox1.Text.Contains("plot"))
            {
                //PLOT SECTION
                #region PLOT
                pictureBox1.Show();                
                rPlot(engine, textBox1.Text, pictureBox1);
                FileStream stream = new FileStream("rplot.jpg", FileMode.Open, FileAccess.Read);
                pictureBox1.Image = Image.FromStream(stream);
                stream.Dispose();
                File.Delete(@"rplot.jpg");
                #endregion
            }
            /*
            else if(!textBox1.Text.Equals(""))
            {
                if(!rExe(engine, textBox1.Text).Equals(""))
                {
                    MessageBox.Show(rExe(engine, textBox1.Text));
                }                
            }*/

            engine.ClearGlobalEnvironment();
            
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem.Equals("Método da Bisseção"))
            {
                label1.Show();
                label1.Text = "a0";
                label4.Show();
                label4.Text = "b0";
                label5.Show();
                label5.Text = "Valor real";
                label6.Show();
                label6.Text = "Critério de parada";
                label3.Show();
               
                textBox1.Show();
                textBox2.Show();
                textBox3.Show();
                textBox4.Show();
                textBox5.Show();
                comboBox2.Show();
                comboBox2.SelectedIndex = 0;
            }
        }
    }
}
