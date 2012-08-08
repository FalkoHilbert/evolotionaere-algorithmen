using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace SystemOfEquations
{
    public partial class Form1 : Form
    {
        private List<Tierchen> Elterngeneration = new List<Tierchen>();
        private List<Tierchen> Kindgeneration = new List<Tierchen>();
        private List<Tierchen> TierchenHistory = new List<Tierchen>();
        private int elternSize = 0;
        private int binärStringLenght = 0;
        private int gene = 0;
        private Problem problem = null;
        List<Intervall> intervalle = new List<Intervall>();
        private bool loadDocument;
        private generator m_Generator;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random randomizer = new Random();
            Elterngeneration = new List<Tierchen>();
            // erzeuge Elterngeneration
            
            Int32.TryParse(textBox1.Text, out elternSize);
            Int32.TryParse(textBox5.Text, out binärStringLenght);
            Int32.TryParse(textBox3.Text, out gene);
            problem = new Problem(Int32.Parse(comboBox1.SelectedItem.ToString().Substring(1,1)));

            var matches = Regex.Matches(textBox2.Text, @"\[(.*?)\]");

            foreach (Match m in matches) {
                Intervall intervall;
                Intervall.TryParse(m.Groups[1].Value,out intervall);
                intervalle.Add(intervall);
            }
            if (checkBox1.Checked && intervalle.Count() == 1)
            {
                while (intervalle.Count() < gene)
                {
                    intervalle.Add(intervalle[0]);
                }
            }
            progressBar1.Enabled = true;
            progressBar1.Value = 0;
            while (Elterngeneration.Count < elternSize)
            {
                Elterngeneration.Add(Tierchen.RandomTier(randomizer,binärStringLenght, intervalle, gene, problem));
                /*
                    * ### Wie kommt das mit dem Distinct zustande ? ###
                    * ### Ist ToList() eine Methode einer über TierchenComparer liegenden Klasse ? ###
                    * ### Welche voraussetzungen müssen erfüllt sein ? ###
                    * !!! Hatten wir ja geklärt, oder?
                    */
                Elterngeneration = Elterngeneration.Distinct(new TierchenComparer()).ToList();
                progressBar1.Value = Elterngeneration.Count() * 100 / elternSize;
            }
            progressBar1.Enabled = false;
            m_Generator = new generator(Elterngeneration, Kindgeneration, TierchenHistory, problem); 
            try
            {
                // Elterngeneration im Programm Verzeichnis unter dem Namen "last.xml" speichern
                // xml-Dokument erzeugen
                XDocument doc = new XDocument(
                    // erzeuge des Wurzelelement "generation"
                    new XElement("generation",
                        // für jedes tier in der Generation
                        Elterngeneration.Select(tier =>
                            // erzeuge "tier"-Element
                            new XElement("tier",
                                // erzeuge Problem-Attribute
                                new XAttribute("problemType",
                                    (int)(tier.Problem.ProblemType)
                                    ),
                                // für jedes gen im tierchen
                                tier.GenCode.Select(gen =>
                                    // erzeuge "gen"-Element
                                    new XElement("gen",
                                        // füge "interval_start"-Attribut hinzu
                                        new XAttribute("interval_start",
                                            gen.Interval.start
                                        ),
                                        // füge "interval_end"-Attribut hinzu
                                        new XAttribute("interval_end",
                                            gen.Interval.end
                                        ),
                                        // für jedes allel (Bit) im Gen
                                        gen.BinärCode.Select(allel =>
                                            // speichere bool'schen Wert 
                                            new XElement("allel",
                                                allel.ToString()
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                );
                // dokument speichern
                doc.Save("last.xml");
            }
            catch (Exception) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            loadDocument = true;
            try
            {
                // Elterngeneration einladen
                XDocument doc = XDocument.Load(openFileDialog1.FileName);
                Elterngeneration =
                    // lade alle "tier"-Elemente 
                    doc.Root.Descendants("tier").Select(tier =>
                        // erzeuge für jedes ein Tierchen Object
                        new Tierchen(
                            // lade Problem
                            new Problem(int.Parse(tier.Attribute("problemType").Value)),
                            // lade innerhalb des "tier"-Elementes alle "gen"-Elemente
                            tier.Descendants("gen").Select(gen =>
                                // erzeuge für jedes ein Allel
                                new Allel(
                                    // lade innerhalb des "gen"-Elementes alle "allel"-Elemente
                                    gen.Descendants("allel").Select(allel =>
                                        // parse den bool-Wert
                                        bool.Parse(allel.Value)).ToList(),
                                    // Intervall steht als Attribut innerhalb des "gen"-Elementes
                                    new Intervall(
                                        int.Parse(gen.Attribute("interval_start").Value),
                                        int.Parse(gen.Attribute("interval_end").Value))
                                )
                            ).ToList()
                        )
                    ).ToList();
                elternSize = Elterngeneration.Count();
                binärStringLenght = Elterngeneration.SelectMany(tier => tier.GenCode.Select(allel => allel.Size)).Distinct().FirstOrDefault();
                gene = Elterngeneration.Select(tier => tier.GenCode.Count()).Distinct().FirstOrDefault();
                problem = Elterngeneration.Select(tier => tier.Problem).Distinct().FirstOrDefault();
                intervalle = Elterngeneration.SelectMany(tier => tier.GenCode.Select(allel => allel.Interval).Distinct(new IntervallComparer())).Distinct(new IntervallComparer()).ToList();

                textBox1.Text = elternSize.ToString();
                textBox2.Text = String.Join(",", intervalle.Select(o => o.ToString()).ToArray());
                textBox3.Text = gene.ToString();
                textBox5.Text = binärStringLenght.ToString();
                int index = 0;
                foreach(var item in comboBox1.Items)
                {
                    if (((string)item).StartsWith(string.Format("[{0}]", (int)problem.ProblemType)))
                        comboBox1.SelectedIndex = index;
                    index++;
                }
                m_Generator = new generator(Elterngeneration, Kindgeneration, TierchenHistory, problem);

            }
            catch (Exception) { loadDocument = false; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (m_Generator == null) m_Generator = new generator(Elterngeneration, Kindgeneration, TierchenHistory, problem);
            int mutationType = 0;
            int mutationRateStart = (int)numericUpDown1.Value;
            int mutationRateEnd = (int)numericUpDown2.Value;
            int rekombinationsPunkte = 0;
            int anzahlGenerationen = 0;
            int anzahlKinder = 0;
            int historySize = 0;
            int selectionStrategie = 0;
            int wahlVerfahren = 0;

            Int32.TryParse(comboBox3.SelectedItem.ToString().Substring(1,1), out mutationType);
            Int32.TryParse(textBox7.Text, out rekombinationsPunkte);
            Int32.TryParse(textBox4.Text, out anzahlGenerationen);
            Int32.TryParse(textBox10.Text, out anzahlKinder);  
            Int32.TryParse(textBox4.Text, out historySize);
            Int32.TryParse(comboBox2.SelectedItem.ToString().Substring(1, 1), out selectionStrategie);
            Int32.TryParse(comboBox4.SelectedItem.ToString().Substring(1, 1), out wahlVerfahren);
            progressBar1.Enabled = true;
            progressBar1.Value = 0;
            backgroundWorker1.RunWorkerAsync(new object[] { mutationType, mutationRateStart, mutationRateEnd, historySize, anzahlGenerationen, anzahlKinder, rekombinationsPunkte, selectionStrategie, wahlVerfahren });
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            m_Generator.generateNewGenerations(backgroundWorker1, (MutationType)(int)((object[])e.Argument)[0], (int)((object[])e.Argument)[1], (int)((object[])e.Argument)[2], (int)((object[])e.Argument)[3], (int)((object[])e.Argument)[4], (int)((object[])e.Argument)[5], (int)((object[])e.Argument)[6], (SelectionsStrategie)(int)((object[])e.Argument)[7], (Wahlverfahren)(int)((object[])e.Argument)[8]);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (checkBox2.Checked)
            {
                var tmpList = Elterngeneration.ToList();
                lock (tmpList)
                {
                    Ausgabe.Text += "\r\nGeneration: " + ((int)e.UserState).ToString() + "\r\n" + String.Join("\r\n", tmpList.Select(o => o.ToNicerString()).ToArray());
                }
                Ausgabe.SelectionStart = Ausgabe.Text.Length;
                Ausgabe.ScrollToCaret();
                Ausgabe.Refresh();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0)
            {
                numericUpDown2.Enabled = true;
                label13.Enabled = true;
                label15.Enabled = true;
            }
            else
            {
                numericUpDown2.Enabled = false;
                label13.Enabled = false;
                label15.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Ausgabe.Text += "\r\nElterngeneration:\r\n" + String.Join("\r\n", Elterngeneration.Select(o => o.ToNicerString()).ToArray());
            Ausgabe.SelectionStart = Ausgabe.Text.Length;
            Ausgabe.ScrollToCaret();
            Ausgabe.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int besten = 0;
            Int32.TryParse(textBox8.Text, out besten);
            Ausgabe.Text += "\r\nDie " + textBox8.Text + " besten Individuen:\r\n" + String.Join("\r\n", TierchenHistory.OrderBy(tier => tier.Wert).Take(besten).Select(o => o.ToNicerString()).ToArray());
            Ausgabe.SelectionStart = Ausgabe.Text.Length;
            Ausgabe.ScrollToCaret();
            Ausgabe.Refresh();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Enabled = false;
            progressBar1.Value = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Ausgabe.Text = "";
        }
    }
}
