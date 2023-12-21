using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace achbine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadExcludedISPs();
        }
        private void LoadExcludedISPs()
        {
            try
            {
                // Read the JSON content from the file
                string jsonContent = File.ReadAllText("provide.json");

                // Deserialize the JSON into a class
                ProvidersData providersData = JsonSerializer.Deserialize<ProvidersData>(jsonContent);

                // Get the excluded ISPs from the class
                excludedISPs = providersData?.Exclude ?? new List<string>();

                // Populate the ComboBox with excluded ISPs
                comboBox1.Items.AddRange(excludedISPs.ToArray());
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, JSON parsing error)
                MessageBox.Show($"Error loading excluded ISPs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Class representing the data structure in provide.json
        public class ProvidersData
        {
            public List<string> Exclude { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void extarctionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
                richTextBox1.Enabled = true;
                comboBox1.Enabled = false;

           
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            richTextBox1.Enabled = false;
            comboBox1.Enabled =true ;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // You can handle the selected files here if needed
                MessageBox.Show("File Uploude successfully.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0; // Reset progress bar
            progressBar1.Maximum = openFileDialog1.FileNames.Length; // Set maximum value for progress bar

            // Move the outputFile declaration outside the loop
            string outputFile = "output_emails.txt";

            foreach (string filePath in openFileDialog1.FileNames)
            {
                // Read each file and extract emails
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Use a regular expression to find emails in each line
                    MatchCollection matches = Regex.Matches(line, @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b");

                    // Save emails to a text file
                    foreach (Match match in matches)
                    {
                        File.AppendAllText(outputFile, match.Value + Environment.NewLine);
                    }
                }

                // Update progress bar
                progressBar1.Value++;
            }

            MessageBox.Show("Emails extracted and saved to output_emails.txt successfully.");

            // Open a SaveFileDialog to allow the user to choose where to save the file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.FileName = "output_emails.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Copy the content of the generated file to the selected destination
                File.Copy(outputFile, saveFileDialog.FileName, true);
                MessageBox.Show("File downloaded successfully.");
            }
        }

    }
}
