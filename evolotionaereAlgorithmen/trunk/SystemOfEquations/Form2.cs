using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Printing;

namespace SystemOfEquations
{
    public partial class Form2 : Form
    {
        private Dictionary<string,List<double>> m_Verläufe;

        public Form2()
        {
            m_Verläufe = new Dictionary<string, List<double>>();
            InitializeComponent();
        }


        public Form2(Dictionary<string, List<double>> verläufe)
        {
            m_Verläufe = verläufe;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (m_Verläufe != null && m_Verläufe.Count() > 0 )
            {
                // erzeuge für jede Liste ein LineChart
                chart1.Series.Clear();
                var table = new System.Data.DataTable();
                table.Columns.Add("Generation", typeof(int));
                int seriesNumber = 1;
                foreach(var entry in m_Verläufe )
                {
                    table.Columns.Add("Fitnesswert" + seriesNumber, typeof(Double));
                    int generation = 0;
                    foreach(var listEntry in entry.Value)
                    {
                        if (table.Rows.Count > generation && table.Rows[generation] != null) table.Rows[generation].SetField("Fitnesswert" + seriesNumber, listEntry);
                        else table.Rows.Add(new object[] { generation, listEntry });
                        generation++;
                    }

                    var serie = new Series(entry.Key);
                    serie.ChartType = SeriesChartType.Spline;
                    serie.XValueMember = "Generation";
                    serie.YValueMembers = "Fitnesswert" + seriesNumber;
                    chart1.Series.Add(serie);
                    seriesNumber++;
                }
                chart1.DataSource = table;
                chart1.DataBind();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
            printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Create and initialize print font 
            System.Drawing.Font printFont = new System.Drawing.Font("Arial", 10);
            // Create Rectangle structure, used to set the position of the chart Rectangle 
            var visibleClip = e.Graphics.VisibleClipBounds;
            var myRec = new Rectangle((int)visibleClip.X, (int)visibleClip.Y, (int)visibleClip.Width, (int)visibleClip.Height);
            //var myRec = new System.Drawing.Rectangle(10, 30, 150, 150);
            // Draw a line of text, followed by the chart, and then another line of text 
            chart1.Printing.PrintPaint(e.Graphics, myRec);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
        }
    }
}
