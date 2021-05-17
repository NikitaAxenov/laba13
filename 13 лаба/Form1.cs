using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _13_лаба
{
    public partial class Form1 : Form
    {
        const double b = 4294967299;
        const double m = 9223372036854775808;
        double xNext = b;
        double average2 = 0;
        double average3 = 0;
        double variance2 = 0;
        double variance3 = 0;
        double chi = 0;
        double xBefore, xNow;
        double param = 0;
        int experiments = 0;
        List<double> prob = new List<double>();
        List<double> stat = new List<double>();
        double[] forChi = new double[20];
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 20; i++)
            {
                xBefore = xNext;
                xNext = (b * xBefore) % m;
                xNow = xNext / m;
            }
            forChi[0] = 3.841;
            forChi[1] = 5.991;
            forChi[2] = 7.815;
            forChi[3] = 9.488;
            forChi[4] = 11.070;
            forChi[5] = 12.592;
            forChi[6] = 14.067;
            forChi[7] = 15.507;
            forChi[8] = 16.919;
            forChi[9] = 18.307;
            forChi[10] = 19.675;
            forChi[11] = 21.026;
            forChi[12] = 22.362;
            forChi[13] = 23.685;
            forChi[14] = 24.996;
            forChi[15] = 26.296;
            forChi[16] = 27.587;
            forChi[17] = 28.869;
            forChi[18] = 30.144;
            forChi[19] = 31.410;
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            param = (double)prob1.Value;
            experiments = (int)numericUpDown1.Value;
            average2 = 0;
            average3 = 0;
            variance2 = 0;
            variance3 = 0;
            chi = 0;
            chart1.Series[0].Points.Clear();
            prob.Clear();
            stat.Clear();

            int n = 0;
            double min = 1;
            double temp;
            double temp2 = 1;
            double sum = 0;
            while(true)
            {
                if (1 - sum < min && n >= param)
                {
                    prob.Add(1 - sum);
                    stat.Add(0);
                    break;
                }
                if (n != 0)
                {
                    temp2 *= n;
                }
                temp = (double)(Math.Pow(param, n) / temp2) * (double)Math.Exp(-param);
                sum += temp; 
                prob.Add(temp);
                stat.Add(0);
                if(temp < min)
                {
                    min = temp;
                }
                n++;
            }
            for (int i = 0; i < experiments; i++)
            {
                xBefore = xNext;
                xNext = (b * xBefore) % m;
                xNow = xNext / m;
                double tempNow = xNow;
                for (int j = 0; j < prob.Count; j++)
                {
                    tempNow -= prob[j];
                    if (tempNow <= 0)
                    {
                        stat[j]++;
                        break;
                    }
                }
            }
            for (int i = 0; i < stat.Count; i++)
            {
                if (stat[i] != 0)
                {
                    average2 += stat[i] / experiments * (i + 1);
                    variance2 += stat[i] / experiments * (i + 1) * (i + 1);
                    chi += (stat[i] * stat[i]) / (experiments * prob[i]);
                }
            }
            variance2 -= (average2 * average2);
            chi -= experiments;
            for (int i = 0; i < stat.Count; i++)
            {
                chart1.Series[0].Points.AddXY(i + 1, stat[i] / experiments);
            }
            average3 = (Math.Abs(average2 - param) / Math.Abs(param)) * 100;
            variance3 = (Math.Abs(variance2 - param) / Math.Abs(param)) * 100;
            double tempChi;
            if(prob.Count >= 20)
            {
                tempChi = forChi[19];
            }
            else
            {
                tempChi = forChi[prob.Count];
            }
            if (chi <= tempChi)
            {
                label17.Text = "false";
                label17.ForeColor = Color.Red;
            }
            else
            {
                label17.Text = "true";
                label17.ForeColor = Color.Green;
            }
            label13.Text = Math.Round(chi, 2) + " " + ">" + " " + tempChi + " " + "?";
            label6.Text = Math.Round(average2, 2) + " " + "(error = " + Math.Round(average3, 0) + "%)";
            label12.Text = Math.Round(variance2, 2) + " " + "(error = " + Math.Round(variance3, 0) + "%)";
        }
    }
}
