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
        string tasking_setup_path       = null;
        string matlab_setup_path        = null;
        string smartgit_setup_path      = null;
        string ude_setup_path           = null;
        string inca_setup_path          = null;
        string sourceinsight_setup_path = null;
        string everything_setup_path    = null;
        string totalcommander_setup_path= null;

        string Org_tasking_setup_path        = null;
        string Org_matlab_setup_path         = null;
        string Org_smartgit_setup_path       = null;
        string Org_ude_setup_path            = null;
        string Org_inca_setup_path           = null;
        string Org_sourceinsight_setup_path  = null;
        string Org_everything_setup_path     = null;
        string Org_totalcommander_setup_path = null;

        string cfg_compiler_path        = null;
        string cfg_project_name         = null;
        string cfg_app_sw_ver           = null;
        string cfg_low_sw_ver           = null;
        string my_output                = null;

        //suffix path
        string matlab_suffix_path       = @"\bin\matlab.exe";
        string smartgit_suffix_path     = @"bin\smartgit.exe";
        string ude_suffix_path          = @"\UdeDesktop.exe";
        string inca_suffix_path         = @"\INCA.exe";
        string tasking_suffix_path      = @"\ctc\eclipse\eclipse.exe";
        string sourceinsight_suffix_path= @"Insight3.exe";

        //.ini cfg file 
        string Compiler_Path_Pattern    = "Compiler_Path";
        string Project_Name_Pattern     = "project_Name";
        string App_SW_Version_Pattern   = "App_SW_Version";
        string Low_SW_Version_Pattern   = "Low_SW_Version";

        //cfg path
        string cfg_file_path =  @"C:\Users\thinkpad\Desktop\proj.ini";
 
        //Build command
        string empty_command = "";

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
            return Regex.IsMatch(str, @"TASKING VX-toolset for TriCore v4.3r3");
        }
        public static bool SourceInsight_GetNum(string str)
        {
            return Regex.IsMatch(str, @"Source Insight 3.5");
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
            StreamReader sr = new StreamReader(cfg_file_path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //Get the path of the compiler.
                Match Compiler_Path_Result = Regex.Match(line, Compiler_Path_Pattern);
                if (Compiler_Path_Result.Success == true)
                {
                    string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);

                    resultString[1] = resultString[1].Trim();//remove the useless space
                    //this.toolStripTextBox4.Text = resultString[1];
                    cfg_compiler_path = resultString[1];
                }

                //Get the project name
                Match Project_Name_Result = Regex.Match(line, Project_Name_Pattern);
                if (Project_Name_Result.Success == true)
                {
                    string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);

                    resultString[1] = resultString[1].Trim();  //remove the useless space
                    this.toolStripTextBox1.Text = resultString[1];
                    cfg_project_name = resultString[1];
                }

                //Get the software version
                Match App_SW_Version_Result = Regex.Match(line, App_SW_Version_Pattern);
                if (App_SW_Version_Result.Success == true)
                {
                    string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);

                    resultString[1] = resultString[1].Trim();//remove the useless space
                    this.toolStripTextBox2.Text = resultString[1];
                    cfg_app_sw_ver = resultString[1];
                }

                Match Low_SW_Version_Result = Regex.Match(line, Low_SW_Version_Pattern);
                if (Low_SW_Version_Result.Success == true)
                {
                    string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);

                    resultString[1] = resultString[1].Trim();//remove the useless space
                    this.toolStripTextBox3.Text = resultString[1];
                    cfg_low_sw_ver = resultString[1];
                }
                my_output = my_output + line + "\r\n";
            }
            sr.Close(); // close the stream and the input file is released.

            Write_File(@"C:\Users\thinkpad\Desktop\my2.ini", my_output);
        }
        /*****************************************************************
        * Check the current directory  
        ******************************************************************/
        private void Check_Dir()
        {
            string curr_dir;
            curr_dir = System.Environment.CurrentDirectory;
            //toolStripTextBox3.Text = System.Environment.CurrentDirectory;
        }
        /*****************************************************************
        * Get the path of tools   
        ******************************************************************/
        private void Get_tools_paths()
        {
            string str = Getinstalledsoftware(); //Get the list of all the setup softwares
            string[] sArr = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries); //Store the list into array
            //Console.WriteLine("SystemDirectory: {0}", sArr[0]);

            //Search the list array for the avaliable setup path
            foreach (string element in sArr)
            {
                Console.WriteLine("SystemDirectory: {0}", element);
                if (Matlab_GetNum(element) == true)  ///<search the matlab 2013a for use>
                {
                    string[] string_local_matlab = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    Org_matlab_setup_path = string_local_matlab[1];
                    matlab_setup_path = Org_matlab_setup_path + matlab_suffix_path;
                }
                else if (SmartGit_GetNum(element) == true)  ///<search the smartgit for use>
                {
                    string[] string_local_SmartGit = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    Org_smartgit_setup_path = string_local_SmartGit[1];
                    smartgit_setup_path = Org_smartgit_setup_path + smartgit_suffix_path;
                }
                else if (Ude_GetNum(element) == true) ///<search the ude for use>
                {
                    string[] string_local_ude = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    Org_ude_setup_path = string_local_ude[1];
                    ude_setup_path = Org_ude_setup_path + ude_suffix_path;
                }
                else if (INCA_GetNum(element) == true) ///<search the inca for use>
                {
                    string[] string_local_inca = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    Org_inca_setup_path = string_local_inca[1];
                    inca_setup_path = Org_inca_setup_path + inca_suffix_path;
                }
                else if (TASKING_GetNum(element) == true) ///<search the tasking for use>
                {
                    string[] string_local_tasking = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    Org_tasking_setup_path = string_local_tasking[1];
                    tasking_setup_path = Org_tasking_setup_path + tasking_suffix_path;
                    
                    //test code
                    //this.toolStripTextBox4.Text = string_local_tasking[1];
                }
                else if (SourceInsight_GetNum(element) == true) ///<search the sourceinsight for use>
                {
                    string[] string_local_sourceinsight = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    Org_sourceinsight_setup_path = string_local_sourceinsight[1];
                    sourceinsight_setup_path = Org_sourceinsight_setup_path + sourceinsight_suffix_path;
                }
            } 
      
            //matlab:check if the path exsits
            if (System.IO.File.Exists(matlab_setup_path))
            {
                toolStripButton2.Enabled = true;
            }
            else
            {
                toolStripButton2.Enabled = false;
            }

            //smartgit:check if the path exsits
            if (System.IO.File.Exists(smartgit_setup_path))
            {
                toolStripButton3.Enabled = true;
            }
            else
            {
                toolStripButton3.Enabled = false;
            }

            //ude:check if the path exsits
            if (System.IO.File.Exists(ude_setup_path))
            {
                toolStripButton5.Enabled = true;
            }
            else
            {
                toolStripButton5.Enabled = false;
            }

            //inca:check if the path exsits
            if (System.IO.File.Exists(inca_setup_path))
            {
                toolStripButton6.Enabled = true;
            }
            else
            {
                toolStripButton6.Enabled = false;
            }

            //tasking:check if the path exsits
            if (System.IO.File.Exists(tasking_setup_path))
            {
                toolStripButton1.Enabled = true;
            }
            else
            {
                toolStripButton1.Enabled = false;
            }

            //totalcommander:check if the path exsits
            if (System.IO.File.Exists(totalcommander_setup_path))
            {
                toolStripButton19.Enabled = true;
            }
            else
            {
                toolStripButton19.Enabled = false;
            }

            //totalcommander:check if the path exsits
            if (System.IO.File.Exists(totalcommander_setup_path))
            {
                toolStripButton19.Enabled = true;
            }
            else
            {
                toolStripButton19.Enabled = false;
            }

            //everything:check if the path exsits
            if (System.IO.File.Exists(everything_setup_path))
            {
                toolStripButton20.Enabled = true;
            }
            else
            {
                toolStripButton20.Enabled = false;
            }

            //source insight:check if the path exsits
            if (System.IO.File.Exists(sourceinsight_setup_path))
            {
                toolStripButton18.Enabled = true;
            }
            else
            {
                toolStripButton18.Enabled = false;
            }
        }

        /*****************************************************************
        * Compare the real compiler path with configuration file(.ini)  
        ******************************************************************/
        private void Check_compiler_path()
        {  
            string temp_1 = tasking_setup_path.Trim();
            string temp_2 = cfg_compiler_path.Trim();

            if (temp_1 != temp_2)
            {   
                DialogResult dr;
                dr = MessageBox.Show(" The compiler path found in your computer is different from\n the configuration file(proj.ini)!\n\n\n Do you want to update the configuration file(proj.ini) with the compiler path found?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                if (dr == DialogResult.Yes)
                {   
                    cfg_compiler_path = tasking_setup_path;
                    //update the cfg file(.ini) with the newest path.
                    MessageBox.Show("The configuration file(proj.ini) has been updated!");
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }    
            }
            else
            { 
               //do nothing ,and keep unchanged
            }
           

        }
        /*****************************************************************
        * The form load  
        ******************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            Check_Dir(); //Check if the current dir is located in \01_Mak

            Get_tools_paths();  //Get the paths of tools from the register tables
            
            Parse_Project_Cfg_File(); // Parse the confiuration file

            //Check_compiler_path(); //Update the compiler path
        }

        /*****************************************************************
        * The End of the Form Load 
        ******************************************************************/

        /*****************************************************************
        * The compiler button 
        ******************************************************************/
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

        //compiler: open path
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Org_tasking_setup_path);
        }
        //compiler: copy path into Clipboard
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, Org_tasking_setup_path); //copy target into Clipboard
        }
        //compiler: end process
        private void endToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "eclipse")
                {
                    item.Kill();
                }
            }
        }
        /*****************************************************************
        * The compiler button end
        ******************************************************************/
        /*****************************************************************
        * The matlab button 
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
        //matlab: open path
        private void openPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Org_matlab_setup_path);
        }
        //matlab: copy path
        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, Org_matlab_setup_path); //copy target into Clipboard
        }
        //matlab: end process
        private void endprocess_matlab_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "MATLAB")
                {
                    item.Kill();
                }
            }
        }
        /*****************************************************************
        * The matlab button end
        ******************************************************************/

        /*****************************************************************
        * The smartgit button
        ******************************************************************/
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
        //smartgit: open path
        private void openPath_smartgit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Org_smartgit_setup_path);
        }
        //smartgit: copy path
        private void copyPath_smartgit_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, Org_smartgit_setup_path); //copy target into Clipboard
        }
        //smartgit: end process
        private void endprocess_smartgit_Click(object sender, EventArgs e)
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
        /*****************************************************************
        * The smartgit button end
        ******************************************************************/

        /*****************************************************************
        * The UDE button
        ******************************************************************/
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
        /*****************************************************************
        * The UDE button end
        ******************************************************************/

        /*****************************************************************
        * The Source Insight button
        ******************************************************************/
        private void toolStripButton18_ButtonClick(object sender, EventArgs e)
        {
            //source insight
            if (System.Diagnostics.Process.GetProcessesByName("Insight3").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The Insight3 is running now!\n Do you want to open anothor one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(sourceinsight_setup_path);
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                //no 
                Process.Start(sourceinsight_setup_path);
            }
        }

        //Source Insight: open path
        private void openPath_sourceinsight_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Org_sourceinsight_setup_path);
        }
        //Source Insight: copy path
        private void copyPath_sourceinsight_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, Org_sourceinsight_setup_path); //copy target into Clipboard
        }
        //Source Insight: end process
        private void endprocess_sourceinsight_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "Insight3")
                {
                    item.Kill();
                }
            }
        }
        /*****************************************************************
        * The Source Insight button end
        ******************************************************************/

        public void Write_File(string path,string info_stream)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(info_stream); //begin to write
            sw.Flush();       //clean buffer
            sw.Close(); //close stream
            fs.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        
        }

        //Open The Project Directory
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

                //Enable the build buttons
                buildbutton_control(true);

                //End the statusstrip
                statusstrip_info_control(false);
            }
        }

        /*****************************************************************
        * Description: launch a process for the command input.
        * Function name:launch_process.
        ******************************************************************/
        private void launch_process(string command1, string command2)
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

            p.StandardInput.WriteLine(command1);
            if (command2 != "")
            {
                p.StandardInput.WriteLine(command2);
            }

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
            //statusstrip_info_control(false);
        }
        /*****************************************************************
        * Description:buildbutton status.
        * Function name:buildbutton_control.
        ******************************************************************/
        private void buildbutton_control(bool actived_status)
        {
            toolStripButton7.Enabled  = actived_status;
            toolStripButton8.Enabled  = actived_status;
            toolStripButton9.Enabled  = actived_status;
            toolStripButton10.Enabled = actived_status;
            toolStripButton21.Enabled = actived_status;
            textBox2.Enabled          = actived_status;
            toolStripTextBox1.Enabled = actived_status;
            toolStripTextBox2.Enabled = actived_status;
        }

        /*****************************************************************
        * Description:buildbutton status.
        * Function name:buildbutton_control.
        ******************************************************************/
        private void statusstrip_info_control(bool actived_status)
        {
            toolStripStatusLabel1.Visible = actived_status;
            toolStripStatusLabel2.Visible = actived_status;
            toolStripStatusLabel3.Visible = actived_status;
            toolStripStatusLabel4.Visible = actived_status;
            toolStripProgressBar1.Visible = actived_status;

            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel4.Text = "";
            toolStripProgressBar1.Value = 0;
        }
        /*****************************************************************
        * Description:make clean.
        * Function name:toolStripButton21_Click.
        ******************************************************************/
        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            //clean all from the low insight...

            textBox1.Text = "";   //clear the info windows first.
            statusstrip_info_control(true);
            toolStripStatusLabel4.Text ="Info:Processing 'Clean...'";
            buildbutton_control(false);  //disable the build buttons
            timer1.Enabled = true;  //enable timer1 to update the progress and bar
            launch_process("make clean", empty_command);  //launch the command process
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
            statusstrip_info_control(true);
            toolStripStatusLabel4.Text = "Info:Processing 'Build Project'";       
            buildbutton_control(false); //disable the build buttons
            timer1.Enabled = true;  //enable timer1 to update the progress and bar
            launch_process("make all", empty_command);//launch the command process
        }
        /*****************************************************************
        * Description:make clean all.
        * Function name:toolStripButton8_Click.
        ******************************************************************/
        private void toolStripButton8_Click(object sender, EventArgs e)
        {   
            //rebuild all from the low insight...

            textBox1.Text = "";   //clear the info windows first.
            statusstrip_info_control(true);
            toolStripStatusLabel4.Text = "Info:Processing 'Rebuild Project'"; 
            buildbutton_control(false);  //disable the build buttons
            timer1.Enabled = true;  //enable timer1 to update the progress and bar 
            launch_process("make clean all", empty_command);  //launch the command process                 
        }

        /*****************************************************************
        * Description:build low library.
        * Function name:toolStripButton9_Click.
        ******************************************************************/
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            //build the low library from the low insight...
            textBox1.Text = "";
            statusstrip_info_control(true);
            toolStripStatusLabel4.Text = "Info:Processing 'Build low sources into library'"; 
            buildbutton_control(false);
            timer1.Enabled = true;  //enable timer1 to update the progress and bar 
            launch_process("make build_lib", empty_command);
        }

        /*****************************************************************
        * Description:make build_lib;make clean;make unall.
        * Function name:toolStripButton10_Click.
        ******************************************************************/
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            //build low library for release from the low insight...
            textBox1.Text = "";
            statusstrip_info_control(true);
            toolStripStatusLabel4.Text = "Info:Processing 'Build Unall'"; 
            buildbutton_control(false);
            //launch_process("make clean unall","");
            timer1.Enabled = true;  //enable timer1 to update the progress and bar 
            launch_process("make clean unall", empty_command);
        }

        /*****************************************************************
        * Description:make build_lib;make clean;make unall.
        * Function name:toolStripButton25_Click.
        ******************************************************************/
        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            //build low library for release from the low insight...
            textBox1.Text = "";
            statusstrip_info_control(true);
            toolStripStatusLabel4.Text = "Info:Processing 'Release'";
            buildbutton_control(false);
            //launch_process("make clean unall","");
            timer1.Enabled = true;  //enable timer1 to update the progress and bar 
            launch_process("make build_lib", "make clean unall");
        }
        /*****************************************************************
        * Description:command line input.
        * Function name:textBox2_KeyDown.
        ******************************************************************/
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

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

        //private void toolStripTextBox3_Click(object sender, EventArgs e)
        //{
          //  System.Diagnostics.Process.Start(System.Environment.CurrentDirectory);
        //}

        /*****************************************************************
        * 
        * Description:Update the configuration file(.ini)
        * 
        ******************************************************************/
        private void Get_last_cfg_project_nam()
        {
            //Get the last cfg_project_name
            StreamReader sr = new StreamReader(cfg_file_path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //Get the project name
                Match Project_Name_Result = Regex.Match(line, Project_Name_Pattern);
                if (Project_Name_Result.Success == true)
                {
                    string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);

                    resultString[1] = resultString[1].Trim();  //remove the useless space
                    cfg_project_name = resultString[1];
                }
            }
            sr.Close();
        }
        /*****************************************************************
        * Description:Update the projet name from the TextBox1_KeyDown
        ******************************************************************/
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {  
            string  out_into_file = null;

            //Update the project name if the input is changed and enter key is entered!
            if (e.KeyCode == Keys.Enter)
            {
                Get_last_cfg_project_nam();

                //check if the toolbox is changed!
                string temp_1 = cfg_project_name.Trim();
                string temp_2 = toolStripTextBox1.Text.Trim();

                if (temp_1 != temp_2) // toolbox is updated!
                {
                    DialogResult dr;
                    dr = MessageBox.Show("Do you really want to change the project name?", "Notice", MessageBoxButtons.YesNo,
                             MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.Yes)
                    {
                        StreamReader sr = new StreamReader(cfg_file_path, Encoding.Default);
                        String line;
                        string temp;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Match Project_Name_Rresult = Regex.Match(line, Project_Name_Pattern);
                            if (Project_Name_Rresult.Success == true)
                            {
                                temp = line.Replace(cfg_project_name, toolStripTextBox1.Text);
                            }
                            else
                            {
                                temp = line;
                            }
                            out_into_file = out_into_file + temp + "\r\n";
                        }
                        sr.Close();
                        Write_File(cfg_file_path, out_into_file);

                        statusstrip_info_control(false);
                        toolStripStatusLabel4.Visible = true;
                        toolStripStatusLabel4.Text = "Info:The project name has been changed!";
                        MessageBox.Show("The project name has been changed!");
                    }
                    else if (dr == DialogResult.No)
                    {
                        statusstrip_info_control(false);
                        toolStripStatusLabel4.Visible = true;
                        toolStripStatusLabel4.Text = "Info:The project name has been kept unchanged!";
                        MessageBox.Show("The project name has been kept unchanged!");
                    }
                }
                else
                { 
                   //do nothing
                    statusstrip_info_control(false);
                    toolStripStatusLabel4.Visible = true;
                    toolStripStatusLabel4.Text = "Info:The project name has not been changed!";
                    MessageBox.Show("The project name has not been changed!");
                }
            }
        }

        /*****************************************************************
        * Description:Update the app sw ver from the toolStripTextBox2
        ******************************************************************/
        private void Get_last_cfg_app_sw_ver()
        {  
            //Get the last cfg_project_name
            StreamReader sr = new StreamReader(cfg_file_path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //Get the project name
                Match App_SW_Version_Result = Regex.Match(line, App_SW_Version_Pattern);
                if (App_SW_Version_Result.Success == true)
                {
                    string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);

                    resultString[1] = resultString[1].Trim();  //remove the useless space
                    cfg_app_sw_ver  = resultString[1];
                }
            }
            sr.Close();
        }

        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            string out_into_file = null;

            //Update the software version
            if (e.KeyCode == Keys.Enter)
            {
                Get_last_cfg_app_sw_ver();

                //check if the toolbox is changed!
                string temp_1 = cfg_app_sw_ver.Trim();
                string temp_2 = toolStripTextBox2.Text.Trim();

                if (temp_1 != temp_2) // toolbox is updated!
                {
                    DialogResult dr;
                    dr = MessageBox.Show("Do you really want to change the version of the application software ?", "Notice", MessageBoxButtons.YesNo,
                             MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.Yes)
                    {
                        StreamReader sr = new StreamReader(cfg_file_path, Encoding.Default);
                        String line;
                        string temp;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Match App_SW_Version_result = Regex.Match(line, App_SW_Version_Pattern);
                            if (App_SW_Version_result.Success == true)
                            {
                                temp = line.Replace(cfg_app_sw_ver, toolStripTextBox2.Text);
                            }
                            else
                            {
                                temp = line;
                            }
                            out_into_file = out_into_file + temp + "\r\n";
                        }
                        sr.Close();
                        Write_File(cfg_file_path, out_into_file);
                        statusstrip_info_control(false);
                        toolStripStatusLabel4.Visible = true;
                        toolStripStatusLabel4.Text = "Info:The version of the application software has been changed!";
                        MessageBox.Show("The version of the application software has been changed!");
                    }
                    else if (dr == DialogResult.No)
                    {
                        statusstrip_info_control(false);
                        toolStripStatusLabel4.Visible = true;
                        toolStripStatusLabel4.Text = "Info:The version of the application software has been kept unchanged!";
                        MessageBox.Show("The version of the application software has been kept unchanged!");

                    }
                }
                else
                {
                    //do nothing
                    statusstrip_info_control(false);
                    toolStripStatusLabel4.Visible = true;
                    toolStripStatusLabel4.Text = "Info:The version of the application software has not been changed!";
                    MessageBox.Show("The version of the application software has not been changed!");
                }
            }  
        }
        /*****************************************************************
        * The End of the toolStripTextBox2
        ******************************************************************/

        /*****************************************************************
        * The low software version
        ******************************************************************/
        private void Get_last_cfg_low_sw_ver()
        {
            //Get the last cfg_project_name
            StreamReader sr = new StreamReader(cfg_file_path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                //Get the project name
                Match Low_SW_Version_Result = Regex.Match(line, Low_SW_Version_Pattern);
                if (Low_SW_Version_Result.Success == true)
                {
                    string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);

                    resultString[1] = resultString[1].Trim();  //remove the useless space
                    cfg_low_sw_ver  = resultString[1];
                }
            }
            sr.Close();
        }
        private void toolStripTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            string out_into_file = null;

            //Update the software version
            if (e.KeyCode == Keys.Enter)
            {
                Get_last_cfg_low_sw_ver();

                //check if the toolbox is changed!
                string temp_1 = cfg_low_sw_ver.Trim();
                string temp_2 = toolStripTextBox3.Text.Trim();

                if (temp_1 != temp_2) // toolbox is updated!
                {
                    DialogResult dr;
                    dr = MessageBox.Show("Do you really want to change the version of the low driver sofware?", "Notice", MessageBoxButtons.YesNo,
                             MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.Yes)
                    {
                        StreamReader sr = new StreamReader(cfg_file_path, Encoding.Default);
                        String line;
                        string temp;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Match Low_SW_Version_result = Regex.Match(line, Low_SW_Version_Pattern);
                            if (Low_SW_Version_result.Success == true)
                            {
                                temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                            }
                            else
                            {
                                temp = line;
                            }
                            out_into_file = out_into_file + temp + "\r\n";
                        }
                        sr.Close();
                        Write_File(cfg_file_path, out_into_file);
                        statusstrip_info_control(false);
                        toolStripStatusLabel4.Visible = true;
                        toolStripStatusLabel4.Text = "Info:The the version of the low driver sofware has been changed!";
                        MessageBox.Show("The the version of the low driver sofware has been changed!");
                    }
                    else if (dr == DialogResult.No)
                    {
                        statusstrip_info_control(false);
                        toolStripStatusLabel4.Visible = true;
                        toolStripStatusLabel4.Text = "Info:The the version of the low driver sofware has been kept unchanged!";
                        MessageBox.Show("The the version of the low driver sofware has been kept unchanged!");
                    }
                }
                else
                {
                    //do nothing
                    statusstrip_info_control(false);
                    toolStripStatusLabel4.Visible = true;
                    toolStripStatusLabel4.Text = "Info:The version of the low driver sofware has not been changed!";
                    MessageBox.Show("The version of the low driver sofware has not been changed!");
                }
            } 
        }

        /*****************************************************************
        * The End of the low software version
        ******************************************************************/

        private void toolStripButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripButton12_ButtonClick(object sender, EventArgs e)
        {
           //MessageBox.Show("ButtonClick!");
           // if (System.IO.File.Exists(tasking_setup_path))
            string local_path = @"c:\";
            if (Directory.Exists(local_path))
            {
               textBox1.Text = "true";
            }
            else
            {
               textBox1.Text = "false";
            }
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

        //Open The Configuration Directory
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            string path = @".";
            System.Diagnostics.Process.Start(path);
        }

        //Open Target File Directory
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            string path = @"..\04_Release\01_Output";
            System.Diagnostics.Process.Start(path);
        }

        //Open Simulink Directory
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            string path = @"..\03_Simulink_Workspace";
            System.Diagnostics.Process.Start(path);
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            string path = @"..\08_Documents";
            System.Diagnostics.Process.Start(path);
        }

        //Welcome is entered,a new window will be shown!
        private void welcomeToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            //show the welcome context!
            Form2 f = new Form2();
            f.Show();
        }

        private void learnAboutMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show the help context!
            Form3 f = new Form3();
            f.Show();
        }

        private void toolStripButton24_Click(object sender, EventArgs e)
        {
            //show the help context!
            Form3 f = new Form3();
            f.Show();
        }

        private void toolStripTextBox5_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
