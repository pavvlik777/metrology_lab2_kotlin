using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Lab
{
    public partial class Form1 : Form
    {
        public static string FileName = "None";

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (openFileDialog1)
                {
                    openFileDialog1.FileName = String.Empty;
                    if (openFileDialog1.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    try
                    {
                        using (var fileStream = File.OpenRead(openFileDialog1.FileName))
                        {
                            TextReader textReader = new StreamReader(fileStream);

                            textReader.Close();
                            fileStream.Close();
                        }
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            string temp = "";
            for (int i = openFileDialog1.FileName.Length - 1; i > 0; i--)
            {
                if (openFileDialog1.FileName[i] == '\\')
                {
                    temp = openFileDialog1.FileName.Substring(i + 1);
                    break;
                }
            }
            bool isKotlin = false;
            for (int i = temp.Length - 1; i > 0; i--)
            {
                if (temp[i] == '.')
                    if (temp.Substring(i + 1) == "kt")
                    {
                        isKotlin = true;
                        break;
                    }
            }
            if (isKotlin)
            {
                FileName = openFileDialog1.FileName;
                using (StreamReader sr = File.OpenText(FileName))
                {
                    string fileCode = sr.ReadToEnd();
                    FileCodeTextBox.Text = fileCode;
                }
            }
            else
            {
                FileCodeTextBox.Text = "";
                MessageBox.Show("Wrong format", "Fail");
                FileName = "None";
            }
        }

        private void AbsolDiffButton_Click(object sender, EventArgs e)
        {
            if (FileName == "None") return;
            Metrics metrics = new Metrics();
            int absolDiff = metrics.FindAbsolutDiff(FileName, true);
            OutputBox.Text = $"Абсолютная сложность - {absolDiff}";
        }

        private void RelatDiffButton_Click(object sender, EventArgs e)
        {
            if (FileName == "None") return;
            Metrics metrics = new Metrics();
            double relativeDiff = metrics.FindRelativeDiff(FileName, true);
            OutputBox.Text = $"Относительная сложность - {relativeDiff}";
        }

        private void MaxNesting_Click(object sender, EventArgs e)
        {
            if (FileName == "None") return;
            Metrics metrics = new Metrics();
            int maxLevel = metrics.FindMaxNestingLevel(FileName);
            OutputBox.Text = $"Максимальный уровень вложенности - {maxLevel}";
        }
    }
}
