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

        /*****************************************************************
        * Description:Global Variables in the Form1 class
        ******************************************************************/
        string tasking_setup_path  = null;
        string matlab_setup_path   = null;
        string smartgit_setup_path = null;
        string ude_setup_path      = null;
        string inca_setup_path     = null;

        string compiler_path;
        string project_name;
        string project_ver;
        string my_output;

        //suffix path
        string matlab_suffix_path   = @"\bin\matlab.exe";
        string smartgit_suffix_path = @"bin\smartgit.exe";
        string ude_suffix_path      = @"\UdeDesktop.exe";
        string inca_suffix_path     = @"\INCA.exe";
        string tasking_suffix_path  = @"\ctc\eclipse\eclipse.exe";
        /*****************************************************************
        * The End of the Definitions  
        ******************************************************************/

        /*****************************************************************
        * Form Load
        ******************************************************************/
        /*****************************************************************
        * The definitons of the match functions  
        ******************************************************************/
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

        /*****************************************************************
        * The Getinstalledsoftware  
        ******************************************************************/
        /// Gets a list of installed software and, if known, the software's install path.
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

        /*****************************************************************
        * The Parse_Project_Cfg_File  
        ******************************************************************/
        private void Parse_Project_Cfg_File()
        {
            string s = @"Compiler_Path = D:\Program Files\Volcano\VSx\configuration\org.eclipse.e4.ui.css.swt.theme";
            string pattern = "Compiler_Path";
            Match result = Regex.Match(s, pattern);
            //textBox4.Text = result.Value;
            string path = @"C:\Users\thinkpad\Desktop\proj.ini";
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
            Write(@"C:\Users\thinkpad\Desktop\my.ini");
        }

        /*****************************************************************
        * The form load  
        ******************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            string str = Getinstalledsoftware();
            string[] sArr = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //Console.WriteLine("SystemDirectory: {0}", sArr[0]);

            foreach (string element in sArr)
            {
                if (Matlab_GetNum(element) == true)  ///<search the matlab 2013a for use>
                {
                    string[] string_local_matlab = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    matlab_setup_path = string_local_matlab[1] + matlab_suffix_path;
                }
                else if (SmartGit_GetNum(element) == true)  ///<search the smartgit for use>
                {
                    string[] string_local_SmartGit = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    smartgit_setup_path = string_local_SmartGit[1] + smartgit_suffix_path;                              
                }
                else if (Ude_GetNum(element) == true) ///<search the ude for use>
                {
                    string[] string_local_ude = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    ude_setup_path = string_local_ude[1] + ude_suffix_path;
                }
                else if (INCA_GetNum(element) == true) ///<search the inca for use>
                {
                    string[] string_local_inca = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    inca_setup_path = string_local_inca[1] + inca_suffix_path;
                }
                else if (TASKING_GetNum(element) == true) ///<search the tasking for use>
                {
                    string[] string_local_tasking = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    tasking_setup_path = string_local_tasking[1] + tasking_suffix_path;
                }
            }

            //Get the current project path
            toolStripTextBox3.Text = System.Environment.CurrentDirectory;
            Parse_Project_Cfg_File();
        }
        /*****************************************************************
        * The End of the Form Load 
        ******************************************************************/

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        
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

        /*****************************************************************
        * 
        *
        * 
        * Description:Code section for build method
        * 
        * 
        ******************************************************************/
        /*****************************************************************
        * Description: Get the status of the build progress.
        * Function name:update_progress_barstatus.
        ******************************************************************/
        private void update_progress_barstatus(int progress_phase)
        {
            int progress;

            int cfileCount = Directory.GetFiles(@"..\00_Codefiles\", "*.c", SearchOption.AllDirectories).Length;
            int hfileCount = Directory.GetFiles(@"..\00_Codefiles\", "*.h", SearchOption.AllDirectories).Length;
            int ofileCount = Directory.GetFiles(@"..\05_Object_Files\", "*.o", SearchOption.AllDirectories).Length;

            if (progress_phase == 0)
            {               
                progress = 95 * ofileCount / cfileCount;
            }
            else
            {
                progress = 100;     
            }

            toolStripProgressBar1.Value = progress;

            toolStripStatusLabel1.Text = "Status：" + "(" + progress.ToString() + "%" + ")";
            toolStripStatusLabel2.Text = cfileCount.ToString() + " c" + "," + hfileCount.ToString() + " h";
        }
        /*****************************************************************
        * Description: The timer1 is used to update the progress bar and status.
        * Function name:timer1_Tick.
        ******************************************************************/
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetProcessesByName("make").ToList().Count > 0)
            {   
                //update the progress bar and status in phase I
                update_progress_barstatus(0);
            }
            else
            {
                //disable timer1
                timer1.Enabled = false;

                //update the progress bar and status in phase I
                update_progress_barstatus(1);
                
                //Eable the build buttons
                buildbutton_control(true);
            }
        }

        /*****************************************************************
        * Description: launch a process for the command input.
        * Function name:launch_process.
        ******************************************************************/
        private void launch_process(string command)
        {
            Process p = new Process(); //Create a local process
            p.StartInfo.FileName = "cmd.exe";  //Set the program name
            //p.StartInfo.FileName = "amk.exe";  //Set the program name
            //p.StartInfo.Arguments = "clean all";  //Set the arguments for the program
            p.StartInfo.UseShellExecute = false;    //Disable the shell to be started!
            p.StartInfo.RedirectStandardInput = true;  //Set the redirect input
            p.StartInfo.RedirectStandardOutput = true; //Set the redirect output  
            p.StartInfo.RedirectStandardError = true;  //Set the redirect error      
            p.StartInfo.CreateNoWindow = true;  //Don't show the process in window.

            p.OutputDataReceived += OutputDataReceived;
            p.ErrorDataReceived += ErrorDataReceived;
            p.Start();    //start the process

            p.StandardInput.WriteLine(command);
            p.StandardInput.WriteLine("exit");

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            p.Close(); //Close the local process       
        }
        /*****************************************************************
        * Description: Receive the info for command line.
        * Function name:OutputDataReceived.
        ******************************************************************/
        void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                // Add the text to the collected output.
                textBox1.AppendText(e.Data + "\n");
            }
        }
        /*****************************************************************
        * Description: Receive the error info for command line.
        * Function name:ErrorDataReceived.
        ******************************************************************/
        void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                // Process line provided in e.Data
                textBox1.AppendText(e.Data + "\n");
            }
        }
        /*****************************************************************
        * Description:buildbutton status.
        * Function name:buildbutton_control.
        ******************************************************************/
        private void buildbutton_control(bool actived_status)
        {
            toolStripButton7.Enabled = actived_status;
            toolStripButton8.Enabled = actived_status;
            toolStripButton9.Enabled = actived_status;
            toolStripButton10.Enabled = actived_status;     
        }
        /*****************************************************************
        * Description:make clean.
        * Function name:toolStripButton21_Click.
        ******************************************************************/
        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            //rebuild all from the low insight...

            textBox1.Text = "";   //clear the info windows first.
            buildbutton_control(false);  //disable the build buttons
            launch_process("make clean");  //launch the command process
            //timer1.Enabled = true;  //enable timer1 to update the progress and bar  
        }
        /*****************************************************************
        * Description:make all.
        * Function name:toolStripButton7_Click.
        ******************************************************************/
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            // The button is used to test the process
            //toolStripProgressBar1.Value = 0;
            textBox1.Text = "";  //clear the info windows first.
            timer1.Enabled = true;  //enable timer1 to update the progress and bar
            buildbutton_control(false); //disable the build buttons
            launch_process("make all");//launch the command process
        }
        /*****************************************************************
        * Description:make clean all.
        * Function name:toolStripButton8_Click.
        ******************************************************************/
        private void toolStripButton8_Click(object sender, EventArgs e)
        {   
            //rebuild all from the low insight...

            textBox1.Text = "";   //clear the info windows first.
            buildbutton_control(false);  //disable the build buttons
            launch_process("make clean all");  //launch the command process
            timer1.Enabled = true;  //enable timer1 to update the progress and bar          
        }

        /*****************************************************************
        * Description:build low library.
        * Function name:toolStripButton9_Click.
        ******************************************************************/
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            //build the low library from the low insight...
            textBox1.Text = "";
            buildbutton_control(false);
            launch_process("make build_lib");
        }

        /*****************************************************************
        * Description:make build_lib;make clean;make unall.
        * Function name:toolStripButton10_Click.
        ******************************************************************/
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            //build low library for release from the low insight...
            textBox1.Text = "";
            buildbutton_control(false);
            launch_process("make clean unall");
            //launch_process("make clean unall");
        }

        /*****************************************************************
        * Description:command line input.
        * Function name:toolStripTextBox5_KeyDown.
        ******************************************************************/
        private void toolStripTextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            string command_line = this.toolStripTextBox5.Text.Trim();
            // If enter key is entered!
            if (e.KeyCode == Keys.Enter)
            {
                //Launch the command from the command line.
                launch_process(command_line);
            }
        }
        /*****************************************************************
        * Description:End the make process to stop the build process.
        * Function name:toolStripButton16_Click.
        ******************************************************************/
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "make")
                {
                    item.Kill();
                }
            }

            //Enable build buttons again.
            buildbutton_control(true);
        }
        /*****************************************************************
        * Description:End Code section for build method 
        ******************************************************************/

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
                    string path = @"C:\Users\thinkpad\Desktop\proj.ini";
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
                    Write2(@"C:\Users\thinkpad\Desktop\proj.ini");
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
                    string path = @"C:\Users\thinkpad\Desktop\proj.ini";
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
                    Write3(@"C:\Users\thinkpad\Desktop\my.ini");
                    MessageBox.Show("The software version has been changed!");
                }
                else if (dr == DialogResult.No)
                {
                    MessageBox.Show("The software version has been kept unchanged!");
                }
            }
        }

        private void toolStripButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripButton12_ButtonClick(object sender, EventArgs e)
        {
           //MessageBox.Show("ButtonClick!");
        }

        private void toolStripButton12_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
           // MessageBox.Show("DropDownItemClicked");
        }

        private void toolStripTextBox4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            Process.Start("cmd.exe");
        }
    }
}
