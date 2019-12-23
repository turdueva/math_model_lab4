using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace lab4_diff_equations
{
    public partial class Form1 : Form
    {
        double T, tay, fi, m, y1, y2, y3;
        double[] t;
        double[] U;
        double[] UStep;


        public Form1()
        {
            InitializeComponent();
            for (int k = 1; k < 10; k++)
            {
                comboBox2.Items.Add(Math.Pow(2, -k));
            }
            for (int k = 2; k < 10; k++)
            {
                comboBox1.Items.Add(1 + Math.Pow(2, k));
            }

            GraphPane pane = zedGraphControl1.GraphPane;
            pane.XAxis.Title.Text = "Ось t";
            pane.XAxis.Title.FontSpec.FontColor = Color.Black;
            pane.YAxis.Title.Text = "Ось Y";
            pane.YAxis.Title.FontSpec.FontColor = Color.Black;
            pane.Title.Text = "Графики";
            pane.Title.FontSpec.FontColor = Color.Black;
        }
        //Метод Эйлера 
        private void EulerMethod()
        {
            T = Convert.ToDouble(1);
            m = Convert.ToDouble(comboBox1.Text);
            tay = T / (m - 1);
            t = new double[Convert.ToInt32(m + 1)];
            for (int k = 0; k <= m; k++)
            {
                t[k] = k * tay;
            }
            U = new double[Convert.ToInt32(m + 1)];
            fi = 0;
            fi = Convert.ToDouble(textBox1.Text);
            U[0] = fi;
            for (int i = 0; i <= m - 1; i++)
            {
                U[i + 1] = U[i] + tay * F(t[i], U[i]);
            }
            GraphPane pane = zedGraphControl1.GraphPane;
            PointPairList list2 = new PointPairList();

            for (int i = 0; i < m; i++)
            {
                list2.Add(t[i], U[i]);
            }

            LineItem curve = pane.AddCurve("Метод Эйлера", list2, Color.Green, SymbolType.Circle);
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.Size = 3;
            curve.Line.Width = 1;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
        //метод Эйлера с коррекцией шага 
        private void EulerMethodMod()
        {
            t = new double[Convert.ToInt32(m + 1)];
            int s = 0;
            double max = 0;
            for (int k = 0; k <= m; k++)
            {
                t[k] = k * tay;
            }
            U = new double[Convert.ToInt32(m + 1)];
            UStep = new double[Convert.ToInt32(m + 1)];
            fi = 0;
            fi = Convert.ToDouble(textBox1.Text);
            U[0] = fi;

            while (s <= 500)
            {
                max = 0;
                for (int i = 0; i < m - 1; i++)
                {
                    U[i + 1] = U[i] + tay * F(t[i], U[i]);
                    UStep[i + 1] = U[i] + (tay / 2.0) * (F(t[i], U[i]) + F(t[i] + tay, U[i + 1]));
                    if ((s > 1) && (i > 0))
                    {
                        if (Math.Abs(UStep[i + 1] - U[i + 1]) > max)
                        {
                            max = Math.Abs(UStep[i + 1] - U[i + 1]);
                        }
                    }
                    U[i + 1] = UStep[i + 1];
                }
                s++;
            }
            GraphPane pane = zedGraphControl1.GraphPane;
            PointPairList list2step = new PointPairList();

            for (int i = 0; i < m; i++)
            {
                list2step.Add(t[i], U[i]);
            }

            LineItem curve = pane.AddCurve("Метод Эйлера c коррекцией шага", list2step, Color.Red, SymbolType.Circle);
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.Size = 3;
            curve.Line.Width = 1;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            
        }

        private void RungeKuttaMethod()
        {
            t = new double[Convert.ToInt32(m + 1)];
            for (int k = 0; k <= m; k++)
            {
                t[k] = k * tay;
            }
            U = new double[Convert.ToInt32(m + 1)];
            fi = 0;
            fi = Convert.ToDouble(textBox1.Text);
            U[0] = fi;
            y1 = 0;
            y2 = 0;
            y3 = 0;
            for (int i = 0; i <= m - 1; i++)
            {
                y1 = U[i] + (tay * F(t[i], U[i])) / 4.0;
                y2 = U[i] + (tay * F(t[i] + (tay / 4.0), y1)) / 2.0;
                y3 = U[i] + tay * (F(t[i], U[i]) - 2.0 * F(t[i] + (tay / 4.0), y1) + 2.0 * F(t[i] + (tay / 2.0), y2));
                U[i + 1] = U[i] + (tay / 6.0) * (F(t[i], U[i]) + (4.0 * F(t[i] + (tay / 2.0), y2)) + F(t[i + 1], y3));
            }
            GraphPane pane = zedGraphControl1.GraphPane;
            PointPairList list4 = new PointPairList();
            for (int i = 0; i < m; i++)
            {

                list4.Add(t[i], U[i]);
            }
            LineItem curve = pane.AddCurve("Метод Рунге-Кутта", list4, Color.Violet, SymbolType.Circle);
            curve.Symbol.Fill.Type = FillType.Solid;
            curve.Symbol.Size = 3;
            curve.Line.Width = 1;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private double F(double t, double u)
        {
           return Math.Cos(u);            
        }

        private void calc_btn_Click(object sender, EventArgs e)
        {
               EulerMethod();
               EulerMethodMod();
               RungeKuttaMethod();
        }

    }
}
