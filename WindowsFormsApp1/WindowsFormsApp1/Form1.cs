//09:09    6/4/2018
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

        string rExe(REngine engine, string expression)
        {
            try
            {
                
                var res = engine.Evaluate(expression);
                var test = res.AsNumeric();                                             
                
                if (test == null)
                    return "";
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

        void rPlot(REngine engine, string expression, PictureBox picBox)
        {
            engine.Evaluate("jpeg(\"rplot.jpg\")");
            try
            {
                engine.Evaluate(expression);
            }
            catch (Exception e)
            {
                pictureBox1.Hide();
            }
            engine.Evaluate("dev.off()");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            REngine.SetEnvironmentVariables();            
            
            textBox6.Text = "";
            textBox6.Hide();
            REngine engine = REngine.GetInstance();
            engine.Evaluate(".libPaths('C:\\\\Program Files\\\\R\\\\R-3.4.3\\\\library')");
            pictureBox1.Hide();
            string rExp, input, bl = "\r\n", its = "";
            float a, b, valorReal = 0, Stop, aux;

            if ((comboBox2.SelectedItem.Equals("ER") && float.Parse(textBox4.Text) != 0)||!comboBox2.SelectedItem.Equals("ER"))
            {
                if (comboBox2.SelectedItem.Equals("EA") || comboBox2.SelectedItem.Equals("ER"))
                {
                    try
                    {
                        valorReal = float.Parse(textBox4.Text);
                    }
                    catch (Exception l)
                    {
                        MessageBox.Show("Insira o Valor Real.");
                    }
                }

                #region METODO DA BISSEÇÃO
                if (comboBox1.SelectedItem.Equals("Método da Bisseção"))
                {
                    int count = 1;
                    float midT = 0, mid = 0;

                    try
                    {
                        input = textBox1.Text;
                        string aT = textBox2.Text;
                        string bT = textBox3.Text;
                        Stop = float.Parse(textBox5.Text);

                        if (comboBox2.SelectedItem.Equals("EA"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + aT + "," + bT + "]" + bl;

                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "x=" + aT + ";" + input;
                                a = float.Parse(rExe(engine, rExp));
                                its += "f(" + aT + ") = " + Convert.ToString(a);
                                if (a < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                rExp = "x=" + bT + ";" + input;
                                b = float.Parse(rExe(engine, rExp));
                                its += "f(" + bT + ") = " + Convert.ToString(b);
                                if (b < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                midT = (float.Parse(aT) + float.Parse(bT)) / 2;
                                rExp = "x=" + Convert.ToString(midT) + ";" + input;
                                mid = float.Parse(rExe(engine, rExp));
                                its += "(a+b)/2 = " + Convert.ToString(midT) + bl;
                                its += "f(" + midT + ") = " + Convert.ToString(mid);
                                if (mid < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;
                                its += "Erro absoluto: " + Convert.ToString(Math.Abs(valorReal - midT)) + bl;

                                if ((mid > 0 && b > 0 && a < 0) || (mid < 0 && b < 0 && a > 0))
                                {
                                    bT = Convert.ToString(midT);
                                }
                                else
                                {
                                    aT = Convert.ToString(midT);
                                }
                            }
                            while (Math.Abs(valorReal - midT) > Stop);
                            its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }
                        else if (comboBox2.SelectedItem.Equals("ER"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + aT + "," + bT + "]" + bl;

                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "x=" + aT + ";" + input;
                                a = float.Parse(rExe(engine, rExp));
                                its += "f(" + aT + ") = " + Convert.ToString(a);
                                if (a < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                rExp = "x=" + bT + ";" + input;
                                b = float.Parse(rExe(engine, rExp));
                                its += "f(" + bT + ") = " + Convert.ToString(b);
                                if (b < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                midT = (float.Parse(aT) + float.Parse(bT)) / 2;
                                rExp = "x=" + Convert.ToString(midT) + ";" + input;
                                mid = float.Parse(rExe(engine, rExp));
                                its += "(a+b)/2 = " + Convert.ToString(midT) + bl;
                                its += "f(" + midT + ") = " + Convert.ToString(mid);
                                if (mid < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;
                                its += "Erro relativo: " + Convert.ToString((Math.Abs(valorReal - midT)) / valorReal) + bl;

                                if ((mid > 0 && b > 0 && a < 0) || (mid < 0 && b < 0 && a > 0))
                                {
                                    bT = Convert.ToString(midT);
                                }
                                else
                                {
                                    aT = Convert.ToString(midT);
                                }
                            }
                            while ((Math.Abs(valorReal - midT) / valorReal) > Stop);
                            its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }
                        else if (comboBox2.SelectedItem.Equals("Nº de iterações"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + aT + "," + bT + "]" + bl;

                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "x=" + aT + ";" + input;
                                a = float.Parse(rExe(engine, rExp));
                                its += "f(" + aT + ") = " + Convert.ToString(a);
                                if (a < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                rExp = "x=" + bT + ";" + input;
                                b = float.Parse(rExe(engine, rExp));
                                its += "f(" + bT + ") = " + Convert.ToString(b);
                                if (b < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                midT = (float.Parse(aT) + float.Parse(bT)) / 2;
                                rExp = "x=" + Convert.ToString(midT) + ";" + input;
                                mid = float.Parse(rExe(engine, rExp));
                                its += "(a+b)/2 = " + Convert.ToString(midT) + bl;
                                its += "f(" + midT + ") = " + Convert.ToString(mid);
                                if (mid < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                its += "Erro absoluto: " + Convert.ToString(Math.Abs(valorReal - midT)) + bl;
                                its += "Erro relativo: " + Convert.ToString((Math.Abs(valorReal - midT)) / valorReal) + bl;

                                if ((mid > 0 && b > 0 && a < 0) || (mid < 0 && b < 0 && a > 0))
                                {
                                    bT = Convert.ToString(midT);
                                }
                                else
                                {
                                    aT = Convert.ToString(midT);
                                }
                            }
                            while (count <= Stop);
                            its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }
                        else if (comboBox2.SelectedItem.Equals("Valor no ponto"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + aT + "," + bT + "]" + bl;

                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "x=" + aT + ";" + input;
                                a = float.Parse(rExe(engine, rExp));
                                its += "f(" + aT + ") = " + Convert.ToString(a);
                                if (a < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                rExp = "x=" + bT + ";" + input;
                                b = float.Parse(rExe(engine, rExp));
                                its += "f(" + bT + ") = " + Convert.ToString(b);
                                if (b < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                midT = (float.Parse(aT) + float.Parse(bT)) / 2;
                                rExp = "x=" + Convert.ToString(midT) + ";" + input;
                                mid = float.Parse(rExe(engine, rExp));
                                its += "(a+b)/2 = " + Convert.ToString(midT) + bl;
                                its += "f(" + Convert.ToString(midT) + ") = " + Convert.ToString(mid);
                                if (mid < 0)
                                    its += " < 0" + bl;
                                else
                                    its += " > 0" + bl;

                                its += "Valor no ponto: " + Convert.ToString(mid);

                                if ((mid > 0 && b > 0 && a < 0) || (mid < 0 && b < 0 && a > 0))
                                {
                                    bT = Convert.ToString(midT);
                                }
                                else
                                {
                                    aT = Convert.ToString(midT);
                                }
                            }
                            while (Math.Abs(mid) > Stop);
                            its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }

                        textBox6.Show();
                        textBox6.Text = its;
                        #region PLOT
                        pictureBox1.Show();
                        rPlot(engine, "curve(" + textBox1.Text + "," + Convert.ToString(midT - (0.5 * midT)) + "," + Convert.ToString(midT + (0.5 * midT)) + ");abline(h=0,col=\"black\");abline(v=0,col=\"black\");grid()", pictureBox1);
                        FileStream stream = new FileStream("rplot.jpg", FileMode.Open, FileAccess.Read);
                        pictureBox1.Image = Image.FromStream(stream);
                        stream.Dispose();
                        File.Delete(@"rplot.jpg");
                        #endregion
                    }
                    catch (Exception d)
                    {
                        MessageBox.Show("Insira todos os valores corretamente");
                    }

                }
                #endregion

                #region POSIÇÃO FALSA
                else if (comboBox1.SelectedItem.Equals("Método da Posição Falsa"))
                {
                    float x = 0;
                    string rExpA, rExpB, rExpX;
                    int count = 0;

                    try
                    {
                        input = textBox1.Text;
                        a = float.Parse(s: textBox2.Text);
                        b = float.Parse(s: textBox3.Text);
                        if (a > b)
                        {
                            aux = a;
                            a = b;
                            b = aux;
                        }
                        Stop = float.Parse(textBox5.Text);

                        if (comboBox2.SelectedItem.Equals("EA"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + Convert.ToString(a) + "," + Convert.ToString(b) + "]" + bl;
                            
                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;
                                rExpA = "x=" + Convert.ToString(a) + ";" + input;
                                rExpB = "x=" + Convert.ToString(b) + ";" + input;
                                its += "((" + Convert.ToString(a) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)),4)) + ") - (" + Convert.ToString(b) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)),4)) + "))/(" + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)),4)) + " - " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)),4)) + ") =";
                                x = (a * float.Parse(rExe(engine, rExpB)) - b * float.Parse(rExe(engine, rExpA))) / (float.Parse(rExe(engine, rExpB)) - float.Parse(rExe(engine, rExpA)));
                                its += Convert.ToString(x)+bl;
                                rExpX = "x=" + Convert.ToString(x) + ";" + input;
                                its += "[a,b] = [" + Convert.ToString(Math.Round(a, 3)) + " , " + Convert.ToString(Math.Round(b, 3)) + "]" + bl;
                                if (x < b)
                                    a = x;
                                else
                                {
                                    a = b;
                                    b = x;
                                }
                                
                                its += "Erro absoluto: " + (Math.Abs(x - valorReal)) + bl;
                            }
                            while ((Math.Abs(x - valorReal)) >= Stop);
                        }

                        else if (comboBox2.SelectedItem.Equals("ER"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + Convert.ToString(a) + "," + Convert.ToString(b) + "]" + bl;
                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;
                                rExpA = "x=" + Convert.ToString(a) + ";" + input;
                                rExpB = "x=" + Convert.ToString(b) + ";" + input;
                                its += "((" + Convert.ToString(a) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)), 4)) + ") - (" + Convert.ToString(b) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)), 4)) + "))/(" + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)), 4)) + " - " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)), 4)) + ") =";
                                x = (a * float.Parse(rExe(engine, rExpB)) - b * float.Parse(rExe(engine, rExpA))) / (float.Parse(rExe(engine, rExpB)) - float.Parse(rExe(engine, rExpA)));
                                its += Convert.ToString(x) + bl;
                                rExpX = "x=" + Convert.ToString(x) + ";" + input;
                                its += "[a,b] = [" + Convert.ToString(Math.Round(a, 3)) + " , " + Convert.ToString(Math.Round(b, 3)) + "]" + bl;
                                if (x < b)
                                    a = x;
                                else
                                {
                                    a = b;
                                    b = x;
                                }
                                
                                its += "Erro relativo: " +Convert.ToString((Math.Abs(x - valorReal) / valorReal)) +bl;
                            } 
                            while ((Math.Abs(x - valorReal) / valorReal) > Stop);
                        }

                        else if (comboBox2.SelectedItem.Equals("Nº de iterações"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + Convert.ToString(a) + "," + Convert.ToString(b) + "]" + bl;
                            int i;
                            for (i = 0; i < Stop; i++)
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;
                                rExpA = "x=" + Convert.ToString(a) + ";" + input;
                                rExpB = "x=" + Convert.ToString(b) + ";" + input;
                                its += "((" + Convert.ToString(a) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)), 4)) + ") - (" + Convert.ToString(b) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)), 4)) + "))/(" + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)), 4)) + " - " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)), 4)) + ") = ";
                                x = (a * float.Parse(rExe(engine, rExpB)) - b * float.Parse(rExe(engine, rExpA))) / (float.Parse(rExe(engine, rExpB)) - float.Parse(rExe(engine, rExpA)));
                                its += Convert.ToString(x) + bl;
                                its += "[a,b] = [" + Convert.ToString(Math.Round(a, 3)) + " , " + Convert.ToString(Math.Round(b, 3)) + "]" + bl;
                                if (x < b)
                                    a = x;
                                else
                                {
                                    a = b;
                                    b = x;
                                }
                                
                            }
                        }

                        else if (comboBox2.SelectedItem.Equals("Valor no ponto"))
                        {
                            its += input + bl;
                            its += "[a,b] = [" + Convert.ToString(a) + "," + Convert.ToString(b) + "]" + bl;
                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;
                                rExpA = "x=" + Convert.ToString(a) + ";" + input;
                                rExpB = "x=" + Convert.ToString(b) + ";" + input;
                                its += "((" + Convert.ToString(a) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)), 4)) + ") - (" + Convert.ToString(b) + " x " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)), 4)) + "))/(" + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpB)), 4)) + " - " + Convert.ToString(Math.Round(float.Parse(rExe(engine, rExpA)), 4)) + ") =";
                                x = (a * float.Parse(rExe(engine, rExpB)) - b * float.Parse(rExe(engine, rExpA))) / (float.Parse(rExe(engine, rExpB)) - float.Parse(rExe(engine, rExpA)));
                                its += Convert.ToString(x) + bl;
                                rExpX = "x=" + Convert.ToString(value: x) + ";" + input;
                                its += "[a,b] = [" + Convert.ToString(Math.Round(a, 3)) + " , " + Convert.ToString(Math.Round(b, 3)) + "]" + bl;
                                if (x < b)
                                    a = x;
                                else
                                {
                                    a = b;
                                    b = x;
                                }
                                
                                its+="f(" + Convert.ToString(x) + ") = " + rExe(engine, rExpX)+bl;
                            }
                            while (Math.Abs(float.Parse(rExe(engine, rExpX))) > Stop);
                        }

                        #region PLOT
                        pictureBox1.Show();
                        rPlot(engine, "curve(" + textBox1.Text + "," + Convert.ToString(x - (0.5 * x)) + "," + Convert.ToString(x + (0.5 * x)) + ");abline(h=0,col=\"black\");abline(v=0,col=\"black\");grid()", pictureBox1);
                        FileStream stream = new FileStream("rplot.jpg", FileMode.Open, FileAccess.Read);
                        pictureBox1.Image = Image.FromStream(stream);
                        stream.Dispose();
                        File.Delete(@"rplot.jpg");
                        #endregion
                        //MessageBox.Show(Convert.ToString(value: x));
                        its += bl + "Resultado: " + Convert.ToString(x);
                        textBox6.Show();
                        textBox6.Text = its;
                    }
                    catch (Exception g)
                    {
                        MessageBox.Show("Insira todos os valores corretamente eu");
                    }
                }
                #endregion

                #region Metodo de Newton
                if (comboBox1.SelectedItem.Equals("Método de Newton-Raphson"))
                {
                    int count = 1;
                    float midT = 0, mid = 0;
                    float f_linha;
                    float f;                    
                    string valT;
                    input = textBox1.Text;
                    string aT = textBox2.Text;
                    
                    try
                    {                        
                        Stop = float.Parse(textBox5.Text);
                        midT = float.Parse(aT);
                        its += input + bl;
                        if (comboBox2.SelectedItem.Equals("EA"))
                        {
                            
                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "f = function(x) " + input + ";(f(" + Convert.ToString(midT) + "+0.0000001)-f(" + Convert.ToString(midT) + "))/(0.0000001);";
                                f_linha = float.Parse(rExe(engine, rExp));

                                rExp = "f(" + midT + ");";
                                f = float.Parse(rExe(engine, rExp));                               

                                its += "x" + Convert.ToString(count - 2)+" = "+midT+bl;
                                its += "f(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f)+bl;
                                its += "f'(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f_linha)+bl;

                                midT = midT - (f / f_linha);
                                its += "x" + Convert.ToString(count-1) + " = " + Convert.ToString(midT);

                                its += "Erro absoluto: " + Convert.ToString(Math.Abs(valorReal - midT)) + bl;                                
                            }
                            while (Math.Abs(valorReal - midT) > Stop);
                            its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }
                        else if (comboBox2.SelectedItem.Equals("ER"))
                        {                           

                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "f = function(x) " + input + ";(f(" + Convert.ToString(midT) + "+0.0000001)-f(" + Convert.ToString(midT) + "))/(0.0000001);";
                                f_linha = float.Parse(rExe(engine, rExp));

                                rExp = "f(" + midT + ");";
                                f = float.Parse(rExe(engine, rExp));



                                its += "x" + Convert.ToString(count - 2) + " = " + midT + bl;
                                its += "f(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f) + bl;
                                its += "f'(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f_linha) + bl;

                                midT = midT - (f / f_linha);
                                its += "x" + Convert.ToString(count - 1) + " = " + Convert.ToString(midT);

                                its += "Erro relativo: " + Convert.ToString((Math.Abs(valorReal - midT)) / valorReal) + bl;
                                
                            }
                            while ((Math.Abs(valorReal - midT) / valorReal) > Stop);
                           its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }
                        else if (comboBox2.SelectedItem.Equals("Nº de iterações"))
                        {                           
                            do
                            {                               
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "f = function(x) " + input + ";(f(" + Convert.ToString(midT) + "+0.0000001)-f(" + Convert.ToString(midT) + "))/(0.0000001);";
                                f_linha = float.Parse(rExe(engine, rExp));

                                rExp = "f(" + midT + ");";
                                f = float.Parse(rExe(engine, rExp));

                                its += "x" + Convert.ToString(count - 2) + " = " + midT + bl;
                                its += "f(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f) + bl;
                                its += "f'(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f_linha) + bl;

                                midT = midT - (f / f_linha);
                                its += "x" + Convert.ToString(count - 1) + " = " + Convert.ToString(midT);

                            }
                            while (count <= Stop);
                            its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }
                        else if (comboBox2.SelectedItem.Equals("Valor no ponto"))
                        {                                                        

                            do
                            {
                                its += bl + "-> Iteração " + Convert.ToString(count) + bl + bl;
                                count++;

                                rExp = "f = function(x) " + input + ";(f(" + Convert.ToString(midT) + "+0.0000001)-f(" + Convert.ToString(midT) + "))/(0.0000001);";
                                f_linha = float.Parse(rExe(engine, rExp));

                                rExp = "f(" + midT + ");";
                                f = float.Parse(rExe(engine, rExp));
                                its += "x" + Convert.ToString(count - 2) + " = " + midT + bl;
                                its += "f(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f) + bl;
                                its += "f'(x" + Convert.ToString(count - 2) + ") = " + Convert.ToString(f_linha) + bl;

                                midT = midT - (f / f_linha);
                                its += "x" + Convert.ToString(count - 1) + " = " + Convert.ToString(midT)+bl;

                                rExp = "x=" + Convert.ToString(midT) + ";" + input;                                
                                mid = float.Parse(rExe(engine, rExp));                                
                                
                                its += "Valor no ponto: " + Convert.ToString(mid)+bl;                                
                            }
                            while (Math.Abs(mid) > Stop);
                            its += bl + "Resultado: " + midT + bl;
                            its += bl;
                        }

                        textBox6.Show();
                        textBox6.Text = its;
                        #region PLOT
                        pictureBox1.Show();
                        rPlot(engine, "curve(" + textBox1.Text + "," + Convert.ToString(midT - (0.5 * midT)) + "," + Convert.ToString(midT + (0.5 * midT)) + ");abline(h=0,col=\"black\");abline(v=0,col=\"black\");grid()", pictureBox1);
                        FileStream stream = new FileStream("rplot.jpg", FileMode.Open, FileAccess.Read);
                        pictureBox1.Image = Image.FromStream(stream);
                        stream.Dispose();
                        File.Delete(@"rplot.jpg");
                        #endregion*/
                    }
                    catch (Exception d)
                    {
                        MessageBox.Show("Insira todos os valores corretamente");
                    }

                }

                #endregion


                /*
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
                else
                {

                }*/
                /*
                else if(!textBox1.Text.Equals(""))
                {
                    if(!rExe(engine, textBox1.Text).Equals(""))
                    {
                        MessageBox.Show(rExe(engine, textBox1.Text));
                    }
                }*/
            }
            else if(comboBox2.SelectedItem.Equals("ER") && float.Parse(textBox4.Text) == 0)            
                MessageBox.Show("Para erros relativos, insira um \"Valor Real\" diferente de 0");
            
            engine.ClearGlobalEnvironment();
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("Método da Bisseção") || comboBox1.SelectedItem.Equals("Método da Posição Falsa"))
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
            }else if(comboBox1.SelectedItem.Equals("Método de Newton-Raphson"))
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


                textBox1.Show();
                label3.Show();

                textBox2.Show();
                label1.Show();
                label1.Text = "x0";

                textBox5.Show();
                label6.Show();
                label6.Text = "Critério de parada";
                label5.Text = "Valor real";

                comboBox2.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox6.Hide();
            pictureBox1.Dispose();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("Método da Bisseção") || comboBox1.SelectedItem.Equals("Método da Posição Falsa")|| comboBox1.SelectedItem.Equals("Método de Newton-Raphson"))
            {
                if (comboBox2.SelectedItem.Equals("Nº de iterações") || comboBox2.SelectedItem.Equals("Valor no ponto"))
                {
                    textBox4.Hide();
                    label5.Hide();
                }
                else
                {
                    textBox4.Show();
                    label5.Show();
                }
            }
        }
    }
}
