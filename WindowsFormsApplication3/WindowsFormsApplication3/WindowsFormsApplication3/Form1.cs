using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//added
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static bool Matlab_GetNum(string str)
        {
            return Regex.IsMatch(str, @"MATLAB R2013a");
        }
        public static bool SmartGit_GetNum(string str)
        {
            return Regex.IsMatch(str, @"SmartGit");
        }
        public static bool Ude_GetNum(string str)
        {
            return Regex.IsMatch(str, @"Universal Debug Engine");
        }
        public static bool INCA_GetNum(string str)
        {
            return Regex.IsMatch(str, @"Universal Debug Engine");
        }
        public static bool TASKING_GetNum(string str)
        {
            return Regex.IsMatch(str, @"TASKING VX-toolset");
        }

        string tasking_setup_path = null;
        string matlab_setup_path = null;
        string smartgit_setup_path = null;
        string ude_setup_path = null;
        string inca_setup_path = null;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //matlab/simulink
            if (System.Diagnostics.Process.GetProcessesByName("MATLAB").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The MATLAB is running now!\n Do you want to open anothor one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(matlab_setup_path);
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                //no 
                Process.Start(matlab_setup_path);
            }
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {  
            //tasking
            if (System.Diagnostics.Process.GetProcessesByName("eclipse").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The TASKING is running now!\n Do you want to open anothor one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(tasking_setup_path);
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                //no 
                Process.Start(tasking_setup_path);
            }
        }

        Process p = new Process();
        int flag = 0;
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //UDE
            if (System.Diagnostics.Process.GetProcessesByName("UdeDesktop").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The UDE is running now!\n Do you want to open anothor one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(ude_setup_path);
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                //no 
                Process.Start(ude_setup_path);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        
        }
        int post;
        private void timer1_Tick(object sender, EventArgs e)
        {
            post++;
            string Result = post.ToString();
            //textBox2.Text = post.ToString();
            //if(p.Threads == true)
            //{
            //string Result = p.StandardOutput.ReadToEnd();
            //textBox2.Text = Result;
            //string Result = p.StandardOutput.;
            //}
            string Result_dos = p.StandardOutput.ReadLine();
            //textBox2.AppendText("skjfhskdfhs" + Result + Result_dos +"\n");
            textBox1.AppendText(Result_dos + "\n");
            //textBox2.Focus();
  
            //if (flag == 1)
            //{
            //    timer1.Enabled = false;
            ////}

            //int cfileCount = Directory.GetFiles(@"..\00_Codefiles\", "*.c", SearchOption.AllDirectories).Length;
            //int hfileCount = Directory.GetFiles(@"..\00_Codefiles\", "*.h", SearchOption.AllDirectories).Length;
            ///int ofileCount = Directory.GetFiles(@"..\05_Object_Files\", "*.o", SearchOption.AllDirectories).Length;

            //int progress   = 95 * ofileCount / cfileCount;

            //toolStripProgressBar1.Value = progress;
            //toolStripStatusLabel1.Text = "Status："  + "(" + progress.ToString() + "%" + ")";
           // toolStripStatusLabel2.Text = cfileCount.ToString() + " cfile" + "," + hfileCount.ToString() + "hfile";

            if (p.WaitForExit(0) == true)
            {
                timer1.Enabled = false;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string path = @"..";
            System.Diagnostics.Process.Start(path);
            //System.Diagnostics.Process.Start("eclipse.exe", path);
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        /// Gets a list of installed software and, if known, the software's install path.
        /// </summary>
        /// <returns></returns>
        private string Getinstalledsoftware()
        {
            //Declare the string to hold the list:
            string Software = null;

            //The registry key:
            string SoftwareKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(SoftwareKey))
            {
                //Let's go through the registry keys and get the info we need:
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            //If the key has value, continue, if not, skip it:
                            if (!(sk.GetValue("DisplayName") == null))
                            {
                                //Is the install location known?
                                if (sk.GetValue("InstallLocation") == null)
                                    Software += sk.GetValue("DisplayName") + " ; Install path not known\n"; //Nope, not here.
                                else
                                    Software += sk.GetValue("DisplayName") + ";" + sk.GetValue("InstallLocation") + "\n"; //Yes, here it is...
                            }
                        }
                        catch (Exception ex)
                        {
                            //No, that exception is not getting away... :P
                        }
                    }
                }
            }

            return Software;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Console.WriteLine();
            //  <-- Keep this information secure! -->
            //Console.WriteLine("SystemDirectory: {0}", Environment.SystemDirectory);
            //textBox1.Text = Environment.SystemDirectory;
            //textBox1.Text = Environment.CurrentDirectory;
            //textBox1.Text = Environment.GetEnvironmentVariable("path");
            //textBox1.Text = Environment.GetEnvironmentVariable("set");
            //textBox1.Text = System.Windows.Forms.Application.ExecutablePath;
            //textBox1.Text = System.Windows.Forms.Application.StartupPath;
            //textBox1.Text = Getinstalledsoftware();
            //Console.WriteLine("SystemDirectory: {0}", Getinstalledsoftware());
            string str = Getinstalledsoftware();
            string[] sArr = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //Console.WriteLine("SystemDirectory: {0}", sArr[0]);
            //textBox1.Text = sArr[0];
            foreach (string element in sArr)
            {
                //Console.WriteLine("SystemDirectory: {0}", element);
                string s = element;
                string pattern = "SmartGit";
                MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(s, pattern, RegexOptions.IgnoreCase);
                //Console.WriteLine("SystemDirectory: {0}", mc);
            }

            //search the matlab 2013a for use
            foreach (string element in sArr)
            {
                if (Matlab_GetNum(element) == true)
                {
                    //matlab_setup_path = ;
                    //string matlab_str = Getinstalledsoftware();
                    string[] string_local_matlab = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    matlab_setup_path = string_local_matlab[1] + @"\bin\matlab.exe";
                    //Console.WriteLine("SystemDirectory: {0}", matlab_setup_path);
                }
            }

            //search the smartgit for use
            foreach (string element in sArr)
            {
                if (SmartGit_GetNum(element) == true)
                {
                    //matlab_setup_path = ;
                    //string matlab_str = Getinstalledsoftware();
                    string[] string_local_SmartGit = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    smartgit_setup_path = string_local_SmartGit[1] + @"bin\smartgit.exe";
                    //Console.WriteLine("SystemDirectory: {0}", smartgit_setup_path);
                }
            }

            //search the ude for use
            foreach (string element in sArr)
            {
                if (Ude_GetNum(element) == true)
                {
                    //matlab_setup_path = ;
                    //string matlab_str = Getinstalledsoftware();
                    string[] string_local_ude = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    ude_setup_path = string_local_ude[1] + @"\UdeDesktop.exe";
                    //Console.WriteLine("SystemDirectory: {0}", smartgit_setup_path);
                }
            }

            foreach (string element in sArr)
            {
                if (INCA_GetNum(element) == true)
                {
                    //matlab_setup_path = ;
                    //string matlab_str = Getinstalledsoftware();
                    string[] string_local_inca = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    inca_setup_path = string_local_inca[1] + @"\INCA.exe";
                    //Console.WriteLine("SystemDirectory: {0}", smartgit_setup_path);
                }
            }

            foreach (string element in sArr)
            {
                if (TASKING_GetNum(element) == true)
                {
                    //matlab_setup_path = ;
                    //string matlab_str = Getinstalledsoftware();
                    string[] string_local_tasking = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    tasking_setup_path = string_local_tasking[1] + @"\ctc\eclipse\eclipse.exe";
                    //Console.WriteLine("SystemDirectory: {0}", smartgit_setup_path);
                }
            }
            /// <summary>
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //smartgit 
            if (System.Diagnostics.Process.GetProcessesByName("smartgit").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The SmartGit is running now!\n Do you want to open anothor one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(smartgit_setup_path);
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                //no 
                Process.Start(smartgit_setup_path);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //INCA 7.1
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //rebuild all from the low insight...

            //Enter the dos command.
            //Process p = new Process();
            textBox1.Text = "";

            //Set the program that will be started later!
            p.StartInfo.FileName = "cmd.exe";

            //Disable the shell to be started!
            p.StartInfo.UseShellExecute = false;

            //Set the redirect input
            p.StartInfo.RedirectStandardInput = true;

            //Set the redirect output
            p.StartInfo.RedirectStandardOutput = true;

            //Set the redirect error
            p.StartInfo.RedirectStandardError = true;

            //Don't show the process in window.
            //p.StartInfo.CreateNoWindow = false;
            p.StartInfo.CreateNoWindow = true;

            //Start the process;
            p.Start();

            //set the pass the parameter into the  process, and the show the system version.
            flag = 0;
            timer1.Enabled = true;
            //p.StandardInput.WriteLine("Ver");
            p.StandardInput.WriteLine("make all");
            //p.Close();
            //Close the dos window.
            //p.StandardInput.WriteLine("exit");
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {   
            //rebuild all from the low insight...

            //Enter the dos command.
            //Process p = new Process();
            textBox1.Text = "";

            //Set the program that will be started later!
            p.StartInfo.FileName = "cmd.exe";

            //Disable the shell to be started!
            p.StartInfo.UseShellExecute = false;

            //Set the redirect input
            p.StartInfo.RedirectStandardInput = true;

            //Set the redirect output
            p.StartInfo.RedirectStandardOutput = true;

            //Set the redirect error
            p.StartInfo.RedirectStandardError = true;

            //Don't show the process in window.
            //p.StartInfo.CreateNoWindow = false;
            p.StartInfo.CreateNoWindow = true;

            //Start the process;
            p.Start();

            //set the pass the parameter into the  process, and the show the system version.
            flag = 0;
            timer1.Enabled = true;
            //p.StandardInput.WriteLine("Ver");
            p.StandardInput.WriteLine("make clean all");

            //Close the dos window.
            p.StandardInput.WriteLine("exit");
            //flag = 1;
            textBox1.Text = "55555555555555555555555";
            //timer1.Enabled = false;
            //string Result = p.StandardOutput.ReadToEnd();
            //textBox1.Text = Result;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            //rebuild all from the low insight...

            //Enter the dos command.
            //Process p = new Process();
            textBox1.Text = "";

            //Set the program that will be started later!
            p.StartInfo.FileName = "cmd.exe";

            //Disable the shell to be started!
            p.StartInfo.UseShellExecute = false;

            //Set the redirect input
            p.StartInfo.RedirectStandardInput = true;

            //Set the redirect output
            p.StartInfo.RedirectStandardOutput = true;

            //Set the redirect error
            p.StartInfo.RedirectStandardError = true;

            //Don't show the process in window.
            //p.StartInfo.CreateNoWindow = false;
            p.StartInfo.CreateNoWindow = true;

            //Start the process;
            p.Start();

            //set the pass the parameter into the  process, and the show the system version.
            flag = 0;
            timer1.Enabled = true;
            //p.StandardInput.WriteLine("Ver");
            p.StandardInput.WriteLine("make build_lib");

            //Close the dos window.
            p.StandardInput.WriteLine("exit");
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            //rebuild all from the low insight...

            //Enter the dos command.
            //Process p = new Process();
            textBox1.Text = "";

            //Set the program that will be started later!
            p.StartInfo.FileName = "cmd.exe";

            //Disable the shell to be started!
            p.StartInfo.UseShellExecute = false;

            //Set the redirect input
            p.StartInfo.RedirectStandardInput = true;

            //Set the redirect output
            p.StartInfo.RedirectStandardOutput = true;

            //Set the redirect error
            p.StartInfo.RedirectStandardError = true;

            //Don't show the process in window.
            //p.StartInfo.CreateNoWindow = false;
            p.StartInfo.CreateNoWindow = true;

            //Start the process;
            p.Start();

            //set the pass the parameter into the  process, and the show the system version.
            flag = 0;
            timer1.Enabled = true;
            //p.StandardInput.WriteLine("Ver");
            p.StandardInput.WriteLine("make build_lib");
            p.StandardInput.WriteLine("make clean");
            p.StandardInput.WriteLine("make unall");

            //Close the dos window.
            p.StandardInput.WriteLine("exit");
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "smartgit")
                {
                    item.Kill();
                }
            }
        }
    }
}
