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

namespace LogsMerger
{
    public partial class Form1 : Form
    {
        private string m_FirstLog;
        private string m_LastLog;

        public Form1()
        {
            InitializeComponent();

            saveFileDialog1.Filter = openFileDialog1.Filter = "Лог | *.csv";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = m_FirstLog = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Text = m_LastLog = openFileDialog1.FileName;
            }
        }

        private void MergeLogsButton_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            using (StreamReader stream = new StreamReader(m_FirstLog, encoding: Encoding.Default))
            {
                CreateNewCalibr(stream, list);

                using (StreamReader stream2 = new StreamReader(m_LastLog, encoding: Encoding.Default))
                {
                    CreateNewCalibr(stream2, list, true);
                }

                UpdateIndexCalibr(list);

                UpdateTimeCalibr(list);
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter streamWriter = new StreamWriter(File.Create(saveFileDialog1.FileName), Encoding.Default))
                {
                    foreach (string t in list)
                    {
                        streamWriter.Write(t);
                    }
                }
            }
        }

        private static void CreateNewCalibr(StreamReader stream, List<string> list, bool skipFrstIndex = false)
        {
            string[] array = stream.ReadToEnd().TrimEnd('\n').Split('\n');
            for (int i1 = 0; i1 < array.Length - 1; i1++)
            {
                if(skipFrstIndex)
                {
                    skipFrstIndex = false;

                    continue;
                }

                string t = array[i1];
                list.Add(t);
            }
        }

        private static void UpdateIndexCalibr(List<string> list)
        {
            int index = 0;
            for (int i1 = 1; i1 < list.Count; i1++)
            {
                string te = list[i1];
                var te2 = te.Split(',')[0];

                try
                {
                    list[i1] = te.Remove(0, te2.Length).Insert(0, $"{index+=1}");
                }
                catch
                {
                    continue;
                }
            }
        }

        private static void UpdateTimeCalibr(List<string> list)
        {
            int i = 0;

            for (int i1 = 1; i1 < list.Count; i1++)
            {
                string te = list[i1];

                var te2 = te.Split(',').Last();
                string stringNew = new string(new string(te.Reverse().ToArray()).Remove(0, te2.Length).Insert(0, "0").Reverse().ToArray());
                te2 = stringNew.Split(',').Last();
                list[i1] = stringNew.Remove(stringNew.Length - 1, te2.Length).Insert(stringNew.Length - 1, $"{i += 1}") + "\n";
            }
        }
    }
}