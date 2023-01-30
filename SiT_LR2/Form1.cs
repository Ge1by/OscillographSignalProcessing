using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SiT_LR2
{
    public partial class Form1 : Form
    {
        List<int> ASignals = new List<int>();
        List<int> BSignals = new List<int>();
        List<int> CSignals = new List<int>();
        List<int> DSignals = new List<int>();

        List<int> SignalSensCode = new List<int>();
        List<int> ZeroLevel = new List<int>();

        int[] impulses = new int[4] { 145, 145, 145, 145 };
        int[] impulseCoord = new int[4] {0, 0, 0, 0 };
        int[] zeroLv = new int[4] { 0, 0, 0, 0 };
        int[] front = new int[4] { 0, 0, 0, 0 };
        int[] fall = new int[4] { 0, 0, 0, 0 };
        int[] amplitude = new int[4] { 0, 0, 0, 0 };
        int[] minAmp = new int[4] { 0, 0, 0, 0 };
        int[] maxAmp = new int[4] { 0, 0, 0, 0 };
        int[] impLength = new int[4] { 0, 0, 0, 0 };
        int[] leftZeroLvCoord = new int[4] { 0, 0, 0, 0 };
        int[] rightZeroLvCoord = new int[4] { 0, 0, 0, 0 };
       
        public Form1()
        {
            InitializeComponent();

            try 
            {
                string[] strings = File.ReadAllLines(@"C:\Users\stud\Desktop\SiT_LR2\SiT_LR2\4-556.CSV");
                //string[] strings = File.ReadAllLines(@"H:\SiT_LR2\SiT_LR2\4-556.CSV");

                string[] settings = new string[11];
                for (int i = 1; i < 12; i++)
                {
                    settings[i - 1] = strings[i];
                }

                for (int i = 0; i < settings.Length; i++)
                {
                    if (settings[i].Contains(','))
                    {
                        settings[i] = settings[i].Substring(0, settings[i].IndexOf(','));
                        strings[i + 1] = settings[i]; // для внешнего вида в текстбоксе.
                    }
                }

                SignalSensCode.Add(Convert.ToInt32(settings[1]));
                SignalSensCode.Add(Convert.ToInt32(settings[2]));
                ZeroLevel.Add(Convert.ToInt32(settings[3]));
                ZeroLevel.Add(Convert.ToInt32(settings[4]));
                SignalSensCode.Add(Convert.ToInt32(settings[5]));
                SignalSensCode.Add(Convert.ToInt32(settings[6]));
                ZeroLevel.Add(Convert.ToInt32(settings[7]));
                ZeroLevel.Add(Convert.ToInt32(settings[8]));
                
                

                for (int i = 12; i < strings.Length; i++)
                {
                    string[] str = strings[i].Split(',');
                    int result;
                    for (int j = 0; j < str.Length; j++)
                    {
                        int.TryParse(str[j], out result);

                        switch (j) 
                        {
                            case 0:
                                ASignals.Add(result);
                                zeroLv[0] = zeroLv[0] + result;
                                if(result < impulses[0])
                                {
                                    impulses[0] = result;
                                    impulseCoord[0] = i - 12;
                                }
                                break;
                            case 1:
                                BSignals.Add(result);
                                zeroLv[1] = zeroLv[1] + result;
                                if (result < impulses[1])
                                {
                                    impulses[1] = result;
                                    impulseCoord[1] = i - 12;
                                }
                                break;
                            case 2:
                                CSignals.Add(result);
                                zeroLv[2] = zeroLv[2] + result;
                                if (result < impulses[2])
                                {
                                    impulses[2] = result;
                                    impulseCoord[2] = i - 12;
                                }
                                break;
                            case 3:
                                DSignals.Add(result);
                                zeroLv[3] = zeroLv[3] + result;
                                if (result < impulses[3])
                                {
                                    impulses[3] = result;
                                    impulseCoord[3] = i - 12;
                                }
                                break;
                        }  
                    }   
                }

                zeroLv[0] = zeroLv[0] / ASignals.Count();
                zeroLv[1] = zeroLv[1] / BSignals.Count();
                zeroLv[2] = zeroLv[2] / CSignals.Count();
                zeroLv[3] = zeroLv[3] / DSignals.Count();


                for (int i = 0; i < strings.Length; i++)
                {
                    richTextBox1.Text += string.Format("{0}) {1}\n", i, strings[i]);
                }
            }
            catch (FileNotFoundException e)
            {
                richTextBox1.Text = e.ToString();
            }

            chart1.ChartAreas.Add(new ChartArea("Signal"));
           

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;


            Series ASignalPoints = new Series("ASignal");
            ASignalPoints.ChartType = SeriesChartType.Line;
            ASignalPoints.ChartArea = "Signal";

            for (int i = 0; i < ASignals.Count; i++)
            {
                ASignalPoints.Points.AddXY(i, ASignals[i]);
            }
            chart1.Series.Add(ASignalPoints);

            Series BSignalPoints = new Series("BSignal");
            BSignalPoints.ChartType = SeriesChartType.Line;
            BSignalPoints.ChartArea = "Signal";

            for (int i = 0; i < BSignals.Count; i++)
            {
                BSignalPoints.Points.AddXY(i, BSignals[i]);
            }
            chart1.Series.Add(BSignalPoints);

            Series CSignalPoints = new Series("CSignal");
            CSignalPoints.ChartType = SeriesChartType.Line;
            CSignalPoints.ChartArea = "Signal";

            for (int i  = 0; i < CSignals.Count; i++)
            {
                CSignalPoints.Points.AddXY(i, CSignals[i]);
            }
            chart1.Series.Add(CSignalPoints);

            Series DSignalPoints = new Series("DSignal");
            DSignalPoints.ChartType = SeriesChartType.Line;
            DSignalPoints.ChartArea = "Signal";

            for (int i  = 0; i < DSignals.Count; i++)
            {
                DSignalPoints.Points.AddXY(i, DSignals[i]);
            }
            chart1.Series.Add(DSignalPoints);

            Series zeroLevelPoints = new Series("zeroLevel");
            zeroLevelPoints.ChartType = SeriesChartType.Line;
            zeroLevelPoints.ChartArea = "Signal";

            for (int i = 0; i < BSignals.Count; i++)
            {
                zeroLevelPoints.Points.AddXY(i, 128);
            }
            chart1.Series.Add(zeroLevelPoints);


            chart1.ChartAreas[0].AxisY.Minimum = 100;
            chart1.ChartAreas[0].AxisY.Maximum = 145;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 26000;


            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                chart1.Series[0].Enabled = false;
            }
            else
            {
                chart1.Series[0].Enabled = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                chart1.Series[1].Enabled = false;
            }
            else
            {
                chart1.Series[1].Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == false)
            {
                chart1.Series[2].Enabled = false;
            }
            else
            {
                chart1.Series[2].Enabled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == false)
            {
                chart1.Series[3].Enabled = false;
            }
            else
            {
                chart1.Series[3].Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Длительность фронта:";
            label2.Text = "Длительность спада:";
            label3.Text = "Амплитуда импульса:";
            label4.Text = "Длительность импульса:";


            int frontTime9 = 0;
            int frontTime1 = 0;
            int fallTime9 = 0;
            int fallTime1 = 0;


            amplitude[0] = zeroLv[0] - impulses[0];
            minAmp[0] = Convert.ToInt32(Math.Round(zeroLv[0] - 0.1 * amplitude[0]));
            maxAmp[0] = Convert.ToInt32(Math.Round(zeroLv[0] - 0.9 * amplitude[0]));

            for (int i = 0; i < impulseCoord[0]; i++)
            {
                if (ASignals[i] == zeroLv[0])
                {
                    leftZeroLvCoord[0] = i;
                }
            }

            for (int i = ASignals.Count() - 1; i > impulseCoord[0]; i--)
            {
                if (ASignals[i] == zeroLv[0])
                {
                    rightZeroLvCoord[0] = i;
                }
            }

            for (int i = impulseCoord[0]; i > 0; i--)
            {
                if (ASignals[i] >= minAmp[0])
                {
                    frontTime1 = i;
                    break;
                }
            }

            for (int i = impulseCoord[0]; i > 0; i--)
            {
                if (ASignals[i] >= maxAmp[0])
                {
                    frontTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[0]; i < ASignals.Count(); i++)
            {
                if (ASignals[i] >= maxAmp[0])
                {
                    fallTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[0]; i < ASignals.Count(); i++)
            {
                if (ASignals[i] >= minAmp[0])
                {
                    fallTime1 = i;
                    break;
                }
            }

            front[0] = frontTime9 - frontTime1;
            fall[0] = fallTime1 - fallTime9;

            impLength[0] = rightZeroLvCoord[0] - leftZeroLvCoord[0];

            label1.Text += (front[0] / 500000.0 * 1000000) + " мкс";
            label2.Text += (fall[0] / 500000.0 * 1000000) + " мкс";
            label3.Text += amplitude[0] / 32.0 * 1 + " В";
            label4.Text += (impLength[0] / 500000.0 * 1000000) + " мкс";

            chart1.ChartAreas[0].AxisY.Minimum = 100;
            chart1.ChartAreas[0].AxisY.Maximum = 136;
            chart1.ChartAreas[0].AxisX.Minimum = 5040;
            chart1.ChartAreas[0].AxisX.Maximum = 5150;

            chart1.Series[0].Enabled = true;
            chart1.Series[1].Enabled = false;
            chart1.Series[2].Enabled = false;
            chart1.Series[3].Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Длительность фронта:";
            label2.Text = "Длительность спада:";
            label3.Text = "Амплитуда импульса:";
            label4.Text = "Длительность импульса:";


            int frontTime9 = 0;
            int frontTime1 = 0;
            int fallTime9 = 0;
            int fallTime1 = 0;


            amplitude[1] = zeroLv[1] - impulses[1];
            minAmp[1] = Convert.ToInt32(Math.Round(zeroLv[1] - 0.1 * amplitude[1]));
            maxAmp[1] = Convert.ToInt32(Math.Round(zeroLv[1] - 0.9 * amplitude[1]));

            for (int i = 0; i < impulseCoord[1]; i++)
            {
                if (BSignals[i] == zeroLv[1])
                {
                    leftZeroLvCoord[1] = i;
                }
            }

            for (int i = BSignals.Count() - 1; i > impulseCoord[1]; i--)
            {
                if (BSignals[i] == zeroLv[1])
                {
                    rightZeroLvCoord[1] = i;
                }
            }

            for (int i = impulseCoord[1]; i > 0; i--)
            {
                if (BSignals[i] >= minAmp[1])
                {
                    frontTime1 = i;
                    break;
                }
            }

            for (int i = impulseCoord[1]; i > 0; i--)
            {
                if (BSignals[i] >= maxAmp[1])
                {
                    frontTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[1]; i < BSignals.Count(); i++)
            {
                if (BSignals[i] >= maxAmp[1])
                {
                    fallTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[1]; i < BSignals.Count(); i++)
            {
                if (BSignals[i] >= minAmp[1])
                {
                    fallTime1 = i;
                    break;
                }
            }

            front[1] = frontTime9 - frontTime1;
            fall[1] = fallTime1 - fallTime9;

            impLength[1] = rightZeroLvCoord[1] - leftZeroLvCoord[1];

            label1.Text += (front[1] / 500000.0 * 1000000) + " мкс";
            label2.Text += (fall[1] / 500000.0 * 1000000) + " мкс";
            label3.Text += amplitude[1] / 32.0 * 1 + " В";
            label4.Text += (impLength[1] / 500000.0 * 1000000) + " мкс";

            chart1.ChartAreas[0].AxisY.Minimum = 100;
            chart1.ChartAreas[0].AxisY.Maximum = 145;
            chart1.ChartAreas[0].AxisX.Minimum = 7150;
            chart1.ChartAreas[0].AxisX.Maximum = 7270;

            chart1.Series[0].Enabled = false;
            chart1.Series[1].Enabled = true;
            chart1.Series[2].Enabled = false;
            chart1.Series[3].Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "Длительность фронта:";
            label2.Text = "Длительность спада:";
            label3.Text = "Амплитуда импульса:";
            label4.Text = "Длительность импульса:";


            int frontTime9 = 0;
            int frontTime1 = 0;
            int fallTime9 = 0;
            int fallTime1 = 0;


            amplitude[2] = zeroLv[2] - impulses[2];
            minAmp[2] = Convert.ToInt32(Math.Round(zeroLv[2] - 0.1 * amplitude[2]));
            maxAmp[2] = Convert.ToInt32(Math.Round(zeroLv[2] - 0.9 * amplitude[2]));

            for (int i = 0; i < impulseCoord[2]; i++)
            {
                if (CSignals[i] == zeroLv[2])
                {
                    leftZeroLvCoord[2] = i;
                }
            }

            for (int i = CSignals.Count() - 1; i > impulseCoord[2]; i--)
            {
                if (CSignals[i] == zeroLv[2])
                {
                    rightZeroLvCoord[2] = i;
                }
            }

            for (int i = impulseCoord[2]; i > 0; i--)
            {
                if (CSignals[i] >= minAmp[2])
                {
                    frontTime1 = i;
                    break;
                }
            }

            for (int i = impulseCoord[2]; i > 0; i--)
            {
                if (CSignals[i] >= maxAmp[2])
                {
                    frontTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[2]; i < CSignals.Count(); i++)
            {
                if (CSignals[i] >= maxAmp[2])
                {
                    fallTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[2]; i < CSignals.Count(); i++)
            {
                if (CSignals[i] >= minAmp[2])
                {
                    fallTime1 = i;
                    break;
                }
            }

            front[2] = frontTime9 - frontTime1;
            fall[2] = fallTime1 - fallTime9;

            impLength[2] = rightZeroLvCoord[2] - leftZeroLvCoord[2];

            label1.Text += (front[2] / 500000.0 * 1000000) + " мкс";
            label2.Text += (fall[2] / 500000.0 * 1000000) + " мкс";
            label3.Text += amplitude[2] / 32.0 * 1 + " В";
            label4.Text += (impLength[2] / 500000.0 * 1000000) + " мкс";

            chart1.ChartAreas[0].AxisY.Minimum = 109;
            chart1.ChartAreas[0].AxisY.Maximum = 136;
            chart1.ChartAreas[0].AxisX.Minimum = 13010;
            chart1.ChartAreas[0].AxisX.Maximum = 13050;

            chart1.Series[0].Enabled = false;
            chart1.Series[1].Enabled = false;
            chart1.Series[2].Enabled = true;
            chart1.Series[3].Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "Длительность фронта:";
            label2.Text = "Длительность спада:";
            label3.Text = "Амплитуда импульса:";
            label4.Text = "Длительность импульса:";


            int frontTime9 = 0;
            int frontTime1 = 0;
            int fallTime9 = 0;
            int fallTime1 = 0;


            amplitude[3] = zeroLv[3] - impulses[3];
            minAmp[3] = Convert.ToInt32(Math.Round(zeroLv[3] - 0.1 * amplitude[3]));
            maxAmp[3] = Convert.ToInt32(Math.Round(zeroLv[3] - 0.9 * amplitude[3]));

            for (int i = 0; i < impulseCoord[3]; i++)
            {
                if (DSignals[i] == zeroLv[3])
                {
                    leftZeroLvCoord[3] = i;
                }
            }

            for (int i = DSignals.Count() - 1; i > impulseCoord[3]; i--)
            {
                if (DSignals[i] == zeroLv[3])
                {
                    rightZeroLvCoord[3] = i;
                }
            }

            for (int i = impulseCoord[3]; i > 0; i--)
            {
                if (DSignals[i] >= minAmp[3])
                {
                    frontTime1 = i;
                    break;
                }
            }

            for (int i = impulseCoord[3]; i > 0; i--)
            {
                if (DSignals[i] >= maxAmp[3])
                {
                    frontTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[3]; i < DSignals.Count(); i++)
            {
                if (DSignals[i] >= maxAmp[3])
                {
                    fallTime9 = i;
                    break;
                }
            }

            for (int i = impulseCoord[3]; i < DSignals.Count(); i++)
            {
                if (DSignals[i] >= minAmp[3])
                {
                    fallTime1 = i;
                    break;
                }
            }

            front[3] = frontTime9 - frontTime1;
            fall[3] = fallTime1 - fallTime9;

            impLength[3] = rightZeroLvCoord[3] - leftZeroLvCoord[3];

            label1.Text += (front[3] / 500000.0 * 1000000) + " мкс";
            label2.Text += (fall[3] / 500000.0 * 1000000) + " мкс";
            label3.Text += amplitude[3] / 32.0 * 1 + " В";
            label4.Text += (impLength[3] / 500000.0 * 1000000) + " мкс"; 

            chart1.ChartAreas[0].AxisY.Minimum = 105;
            chart1.ChartAreas[0].AxisY.Maximum = 135;
            chart1.ChartAreas[0].AxisX.Minimum = 15135;
            chart1.ChartAreas[0].AxisX.Maximum = 15170;
            
            chart1.Series[0].Enabled = false;
            chart1.Series[1].Enabled = false;
            chart1.Series[2].Enabled = false;
            chart1.Series[3].Enabled = true;
        }
    }
}
