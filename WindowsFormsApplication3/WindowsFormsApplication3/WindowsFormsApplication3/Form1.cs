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
using System.Threading;

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
        string compiler_path;
        string project_name;
        string project_ver;
        string my_output;

        public void Write(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(my_output);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        private void Parse_Project_Cfg_File()
        {
            string s = @"Compiler_Path = D:\Program Files\Volcano\VSx\configuration\org.eclipse.e4.ui.css.swt.theme";
            string pattern = "Compiler_Path";
            Match result = Regex.Match(s, pattern);
            //textBox4.Text = result.Value;
            string path = @"C:\Users\bai\Desktop\proj.ini";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //Console.WriteLine(line.ToString());
                // read a effective line.
                //Get the path of the compiler.
                Match result_2 = Regex.Match(line, pattern);
                //textBox4.Text = result.Value;
                if (result_2.Success == true)
                {
                    ///string content = "agcsmallmacsmallgggsmallytx";
                    string content = line;
                    string[] resultString = Regex.Split(content, "=", RegexOptions.IgnoreCase);
                    //remove the useless space
                    //str = str.Trim();
                    //resultString[1] =;
                    resultString[1] = resultString[1].Trim();
                    this.toolStripTextBox4.Text = resultString[1];
                    compiler_path = resultString[1];
                    //Console.WriteLine(resultString[1]);
                    //foreach (string i in resultString)
                    //{
                        ///i = i.Trim();
                        //Console.WriteLine(i.ToString());
                    //}
                }
                string pattern_project = "project_Name";
                //Get the project name
                Match result_3 = Regex.Match(line, pattern_project);
                if (result_3.Success == true)
                {
                    string content_project = line;
                    string[] resultString = Regex.Split(content_project, "=", RegexOptions.IgnoreCase);
                    //remove the useless space
                    //str = str.Trim();
                    //resultString[1] =;
                    resultString[1] = resultString[1].Trim();
                    this.toolStripTextBox1.Text = resultString[1];
                    project_name = resultString[1];
                }
                string pattern_sw_ver = "Software_Version";
                //Get the project name
                Match result_sw_ver = Regex.Match(line, pattern_sw_ver);
                if (result_sw_ver.Success == true)
                {
                    string content_sw_ver = line;
                    string[] resultString = Regex.Split(content_sw_ver, "=", RegexOptions.IgnoreCase);
                    //remove the useless space
                    //str = str.Trim();
                    //resultString[1] =;
                    resultString[1] = resultString[1].Trim();
                    this.toolStripTextBox2.Text = resultString[1];
                    project_ver = resultString[1];
                }
                my_output = my_output + line + "\n";
            }
            Write(@"C:\Users\bai\Desktop\my.ini");

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
            //if ((p.StandardOutput.ReadLine() == null)&&(p.WaitForExit(0) == true))
            //{
            //    timer1.Enabled = false;
            //}
           
            //textBox2.Focus();
  
            //if (flag == 1)
            //{
            //    timer1.Enabled = false;
            ////}

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
            /// 

            //Get the current project path
            toolStripTextBox3.Text = System.Environment.CurrentDirectory;
            Parse_Project_Cfg_File();
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
            toolStrip1.Enabled = false;
            //rebuild all from the low insight...

            //Enter the dos command.
            //Process p = new Process();
            textBox1.Text = "";
            toolStripProgressBar1.Value = 0;

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
            //timer1.Enabled = true;
            //p.StandardInput.WriteLine("Ver");
            //p.StandardInput.WriteLine("make all");
            p.StandardInput.WriteLine("make clean all");
            p.StandardInput.WriteLine("exit");
            //p.Close();
            //Close the dos window.
            //p.StandardInput.WriteLine("exit");
            //////////////////////
            //test code
            //string q = "";
            while (!p.HasExited)
            {
              //q += p.StandardOutput.ReadLine();
              string Result_dos = p.StandardOutput.ReadLine();
              //textBox2.AppendText("skjfhskdfhs" + Result + Result_dos +"\n");
              textBox1.AppendText(Result_dos + "\n");
                //textBox1.AppendText(p.StandardOutput.ReadToEnd() + "\n");
              int cfileCount = Directory.GetFiles(@"..\00_Codefiles\", "*.c", SearchOption.AllDirectories).Length;
              int hfileCount = Directory.GetFiles(@"..\00_Codefiles\", "*.h", SearchOption.AllDirectories).Length;
              int ofileCount = Directory.GetFiles(@"..\05_Object_Files\", "*.o", SearchOption.AllDirectories).Length;
              int progress = 95 * ofileCount / cfileCount;

              toolStripProgressBar1.Value = progress;
              toolStripStatusLabel1.Text = "Status：" + "(" + progress.ToString() + "%" + ")";
              toolStripStatusLabel2.Text = cfileCount.ToString() + " cfile" + "," + hfileCount.ToString() + "hfile";
              Thread.Sleep(500);
            }
            toolStripProgressBar1.Value = 100;
            //textBox1.Text = q;
            /////////////////////
            toolStrip1.Enabled = true;
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

        private void toolStripTextBox5_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory);
        }

        private void toolStripTextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            string command_line = this.toolStripTextBox5.Text.Trim();
            // If enter key is entered!
            if (e.KeyCode == Keys.Enter)
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
                p.StandardInput.WriteLine(command_line);

                //Close the dos window.
                p.StandardInput.WriteLine("exit");
            }
        }

        string toolStripTextBox2_output;
        public void Write2(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(toolStripTextBox2_output);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public void Write3(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(software_version_output);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //Update the project name
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult dr;
                dr = MessageBox.Show("Do you really want to change the project name???", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    toolStripTextBox2_output = null;
                    //toolStripTextBox2.Text = "The first button is clicked!";
                    // = toolStripTextBox2.Text;
                    string path = @"C:\Users\bai\Desktop\proj.ini";
                    StreamReader sr = new StreamReader(path, Encoding.Default);
                    String line;
                    string temp;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string pattern_project = "project_Name";
                        Match result_3 = Regex.Match(line, pattern_project);
                        //string sourceString = "Ni hao 123";
                        if (result_3.Success == true)
                        {
                            temp = line.Replace(project_name, toolStripTextBox1.Text);
                        }
                        else
                        {
                            temp = line;
                        }
                        toolStripTextBox2_output = toolStripTextBox2_output + temp + "\n";
                        //textBox4.Text = sourceString;
                        //Console.WriteLine(sourceString);//这个时候打印出来的还是 Ni hao 123;
                    }
                    Write2(@"C:\Users\bai\Desktop\proj.ini");
                    MessageBox.Show("The project name has been changed!");
                }
                else if (dr == DialogResult.No)
                {
                    MessageBox.Show("The project name has been kept unchanged!");
                }
            }
        }

        string software_version_output;
        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            //Update the software version
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult dr;
                dr = MessageBox.Show("Do you really want to change the software version???", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    software_version_output = null;
                    //toolStripTextBox2.Text = "The first button is clicked!";
                    // = toolStripTextBox2.Text;
                    string path = @"C:\Users\bai\Desktop\proj.ini";
                    StreamReader sr = new StreamReader(path, Encoding.Default);
                    String line;
                    string temp;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string pattern_project = "Software_Version";
                        Match software_version_result = Regex.Match(line, pattern_project);
                        //string sourceString = "Ni hao 123";
                        if (software_version_result.Success == true)
                        {
                            temp = line.Replace(project_ver, toolStripTextBox2.Text);
                        }
                        else
                        {
                            temp = line;
                        }
                        software_version_output = software_version_output + temp + "\n";
                        //textBox4.Text = sourceString;
                        //Console.WriteLine(sourceString);//这个时候打印出来的还是 Ni hao 123;
                    }
                    Write3(@"C:\Users\bai\Desktop\my.ini");
                    MessageBox.Show("The software version has been changed!");
                }
                else if (dr == DialogResult.No)
                {
                    MessageBox.Show("The software version has been kept unchanged!");
                }
            }
        }
    }
}
