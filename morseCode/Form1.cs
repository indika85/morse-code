using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace morseCode
{
    public partial class Form1 : Form
    {
        [DllImport("Kernel32.dll")]
        static extern bool Beep(uint dwFreq, uint dwDuration);

        private bool finish = false;

        private string text = "";
        private string signs = "";

        private const uint DAH_DURATION = 150;
        private const uint DIT_DURATION = 60;
        private const int GAP_BETWEEN_LETTERS = 210;
        private const int GAP_WITHIN = 90;
        private uint FREQUENCY = 256;

        public Form1()
        {
            InitializeComponent();
            label2.Text = "";
            label4.Text = "";
            richTextBox1.Focus();
            textBox2.Text = trackBar1.Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.Focus();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                MessageBox.Show("No text to genarate the Morse code!", "Genarator Error");
                richTextBox1.Focus();
                return;
            }
            text = richTextBox1.Text;
            finish = false;
            if(!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            int textCount = text.Length;
            for (int index = 0; index < textCount; index++)
            {
                backgroundWorker1.ReportProgress(index);
                if (text[index] == ' ')
                {
                    Thread.Sleep(GAP_BETWEEN_LETTERS);
                }
                else
                {
                    //DecodeCharacter(text[index].ToString());
                    string temp = text[index].ToString();
                    string code = "";
                    temp = temp.ToLower();
                    switch (temp)
                    {
                        case "a": code = "sl"; break;
                        case "b": code = "lsss"; break;
                        case "c": code = "lsls"; break;
                        case "d": code = "lss"; break;
                        case "e": code = "s"; break;
                        case "f": code = "ssls"; break;
                        case "g": code = "lls"; break;
                        case "h": code = "ssss"; break;
                        case "i": code = "ss"; break;
                        case "j": code = "slll"; break;
                        case "k": code = "lsl"; break;
                        case "l": code = "slss"; break;
                        case "m": code = "ll"; break;
                        case "n": code = "ls"; break;
                        case "o": code = "lll"; break;
                        case "p": code = "slls"; break;
                        case "q": code = "llsl"; break;
                        case "r": code = "sls"; break;
                        case "s": code = "sss"; break;
                        case "t": code = "l"; break;
                        case "u": code = "ssl"; break;
                        case "v": code = "sssl"; break;
                        case "w": code = "sll"; break;
                        case "x": code = "lssl"; break;
                        case "y": code = "lsll"; break;
                        case "z": code = "llss"; break;

                        case ".": code = "slslsl"; break;
                        case ",": code = "llssll"; break;
                        case "/": code = "lssls"; break;
                        case "@": code = "sllsls"; break;
                        case "?": code = "ssllss"; break;

                        case "1": code = "sllll"; break;
                        case "2": code = "sslll"; break;
                        case "3": code = "sssll"; break;
                        case "4": code = "ssssl"; break;
                        case "5": code = "sssss"; break;
                        case "6": code = "lssss"; break;
                        case "7": code = "llsss"; break;
                        case "8": code = "lllss"; break;
                        case "9": code = "lllls"; break;
                        case "0": code = "lllll"; break;
                    }
                    signs = code;
                    uint duration = 0;
                    
                    for (int i = 0; i < code.Length; i++)
                    {                      
                        if (code[i] == 's')
                        {
                            duration = DIT_DURATION;
                        }
                        else if (code[i] == 'l')
                        {
                            duration = DAH_DURATION;
                        }
                        
                        Beep(FREQUENCY, duration);
                        Thread.Sleep(GAP_WITHIN);
                    }
                   
                }
                
                Thread.Sleep(GAP_BETWEEN_LETTERS);
            }
            finish = true;
            backgroundWorker1.ReportProgress(100);
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (finish)
            {
                richTextBox1.Text = text;
                label2.Text = "";
                label4.Text = "";
                richTextBox1.Focus();
                richTextBox1.SelectAll();
                return;
            }
            signs = signs.Replace("s", "-");
            signs = signs.Replace("l", "−");
            label4.Text = signs;
            string t = richTextBox1.Text;
            t = t.ToLower();
            string c = t[e.ProgressPercentage].ToString();
            c = c.ToUpper();

            t=t.Remove(e.ProgressPercentage, 1);
            t=t.Insert(e.ProgressPercentage, c);

            richTextBox1.Text = t;
            label2.Text=c;
            
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            richTextBox1.Focus();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //FREQUENCY = (uint)trackBar1.Value;
            //textBox2.Text = trackBar1.Value.ToString();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            int val = int.Parse(textBox2.Text);
            if (e.KeyCode == Keys.Enter)
            {
                if (val < trackBar1.Minimum) val = trackBar1.Minimum;
                if (val > trackBar1.Maximum) val = trackBar1.Maximum;
                trackBar1.Value = val;
                textBox2.Text = val.ToString();
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            int val = int.Parse(textBox2.Text);
            if (val < trackBar1.Minimum) val = trackBar1.Minimum;
            if (val > trackBar1.Maximum) val = trackBar1.Maximum;
            trackBar1.Value = val;
            textBox2.Text = val.ToString();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            FREQUENCY = (uint)trackBar1.Value;
            textBox2.Text = trackBar1.Value.ToString();
        }
    }
}