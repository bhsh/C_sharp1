/*****************************************************************
* Description:The C sharp application is created to manager the
*             software platform 
* Author: YP 
* Time: 17/09/2017 
******************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//added classes
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
        string tasking_setup_path = "";
        string matlab_setup_path = "";
        string smartgit_setup_path = "";
        string ude_setup_path = "";
        string inca_setup_path = "";
        string sourceinsight_setup_path = "";
        string everything_setup_path = "";
        string totalcommander_setup_path = "";

        string Org_tasking_setup_path = "";
        string Org_matlab_setup_path = "";
        string Org_smartgit_setup_path = "";
        string Org_ude_setup_path = "";
        string Org_inca_setup_path = "";
        string Org_sourceinsight_setup_path = "";
        string Org_everything_setup_path = "";
        string Org_totalcommander_setup_path = "";

        string cfg_compiler_path = "";
        string cfg_project_name = "";
        string cfg_app_sw_ver = "";
        string cfg_low_sw_ver = "";
        string my_output = "";

        //suffix path
        string matlab_suffix_path = @"\bin\matlab.exe";
        string smartgit_suffix_path = @"bin\smartgit.exe";
        string ude_suffix_path = @"\UdeDesktop.exe";
        string inca_suffix_path = @"\Inca.exe";
        string tasking_suffix_path = @"\ctc\eclipse\eclipse.exe";
        string sourceinsight_suffix_path = @"Insight3.exe";
        string everything_suffix_path = @"\Everything.exe";
        string totalcmd_suffix_path = @"\TOTALCMD.EXE";

        string inca_arguments = @"-ietas.icx";

        //.ini cfg file 
        string Compiler_Path_Pattern = "Compiler_Path";
        string Project_Name_Pattern = "project_Name";
        string App_SW_Version_Pattern = "App_SW_Version";
        string Low_SW_Version_Pattern = "Low_SW_Version";

        //cfg path
        string cfg_file_path = @"C:\Users\bai\Desktop\proj.ini";

        //Build command
        string empty_command = "";

        //background search
        //Because the paths of everything,inca and totalcommand can not be found in the
        //register table of window, a background thread should be started to search the
        //paths in all the directories of WINDOWS logicDrivers.
        string bkgd_search_everything_dir = @"Everything64";
        string bkgd_search_inca_dir = @"ETAS\INCA7.1";
        string bkgd_search_totalcommand_dir = @"totalcmd";

        //Old definition: public static string[] Setup_SW_Name = new string[]
        public static string[] Setup_SW_Name = new string[] { "Matlab", "SmartGit", "UDE", "INCA", "TASKING", "Source Insight", "Total Commander", "Everything" };
        public static string[] Setup_SW_Path = new string[] { "", "", "", "", "", "", "", "", "", "" };
        string Not_Detected_Result = "Not be detected";
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

            Write_File(@"C:\Users\bai\Desktop\my2.ini", my_output);
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
            int index;
            //Console.WriteLine("SystemDirectory: {0}", sArr[0]);

            //Search the list array for the avaliable setup path
            foreach (string element in sArr)
            {
                Console.WriteLine("SystemDirectory: {0}", element);
                if (Matlab_GetNum(element) == true)  ///<search the matlab 2013a for use>
                {
                    if (matlab_cfgfile_detected == false)
                    { 
                        string[] string_local_matlab = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        Org_matlab_setup_path = string_local_matlab[1];
                        matlab_setup_path = Org_matlab_setup_path + matlab_suffix_path;

                        Setup_SW_Name[0] = string_local_matlab[0].Trim();
                        Setup_SW_Path[0] = string_local_matlab[1].Trim();
                    }
                }
                else if (SmartGit_GetNum(element) == true)  ///<search the smartgit for use>
                {
                    if (smartgit_cfgfile_detected == false)
                    {
                        string[] string_local_SmartGit = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        Org_smartgit_setup_path = string_local_SmartGit[1];
                        smartgit_setup_path = Org_smartgit_setup_path + smartgit_suffix_path;

                        Setup_SW_Name[1] = string_local_SmartGit[0].Trim();
                        Setup_SW_Path[1] = string_local_SmartGit[1].Trim();
                    }
                }
                else if (Ude_GetNum(element) == true) ///<search the ude for use>
                {
                    if (ude_cfgfile_detected == false)
                    {
                        string[] string_local_ude = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        Org_ude_setup_path = string_local_ude[1];
                        ude_setup_path = Org_ude_setup_path + ude_suffix_path;

                        Setup_SW_Name[2] = string_local_ude[0].Trim();
                        Setup_SW_Path[2] = string_local_ude[1].Trim();
                    }
                }
                else if (INCA_GetNum(element) == true) ///<search the inca for use>
                {
                    string[] string_local_inca = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    Org_inca_setup_path = string_local_inca[1];
                    inca_setup_path = Org_inca_setup_path + inca_suffix_path;

                    Setup_SW_Name[3] = string_local_inca[0].Trim();
                    Setup_SW_Path[3] = string_local_inca[1].Trim();
                }
                else if (TASKING_GetNum(element) == true) ///<search the tasking for use>
                {
                    if(tasking_cfgfile_detected == false)
                    {
                        string[] string_local_tasking = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        Org_tasking_setup_path = string_local_tasking[1];
                        tasking_setup_path = Org_tasking_setup_path + tasking_suffix_path;

                        Setup_SW_Name[4] = string_local_tasking[0].Trim();
                        Setup_SW_Path[4] = string_local_tasking[1].Trim();
                    }
                    //test code
                    //this.toolStripTextBox4.Text = string_local_tasking[1];
                }
                else if (SourceInsight_GetNum(element) == true) ///<search the sourceinsight for use>
                {
                    if(sourceinsight_cfgfile_detected == false)
                    {
                        string[] string_local_sourceinsight = element.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        Org_sourceinsight_setup_path = string_local_sourceinsight[1];
                        sourceinsight_setup_path = Org_sourceinsight_setup_path + sourceinsight_suffix_path;

                        Setup_SW_Name[5] = string_local_sourceinsight[0].Trim();
                        Setup_SW_Path[5] = string_local_sourceinsight[1].Trim();
                    }
                }

                for (index = 0; index < Setup_SW_Name.Length; index++)
                {
                    if (Setup_SW_Path[index] == "")
                    {
                        Setup_SW_Path[index] = Not_Detected_Result;
                    }
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
            string temp_1 = Org_tasking_setup_path.Trim();  //Org_tasking_setup_path
            string temp_2 = cfg_compiler_path.Trim();

            if (temp_1 != temp_2)
            {
                DialogResult dr;
                dr = MessageBox.Show(" The compiler path found in your computer is different from\n the configuration file(proj.ini)!\n\n\n Do you want to update the configuration file(proj.ini) with the compiler path found?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                if (dr == DialogResult.Yes)
                {
                    cfg_compiler_path = Org_tasking_setup_path;

                    //update the cfg file(.ini) with the newest path.

                    //read the configured file
                    string out_info = "";
                    StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\proj.ini", Encoding.Default);
                    String line;
                    string temp;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Match result = Regex.Match(line, Compiler_Path_Pattern);
                        if (result.Success == true)
                        {
                            //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                            temp = @"Compiler_Path    = " + Org_tasking_setup_path;
                        }
                        else
                        {
                            temp = line;
                        }
                        out_info = out_info + temp + "\r\n";
                    }
                    sr.Close();
                    Write_File(@"C:\Users\bai\Desktop\proj.ini", out_info);

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

            everytool_cfg_file_read(@"C:\Users\bai\Desktop\Everytool.ini");

            Get_tools_paths();  //Get the paths of tools from the register tables

            Parse_Project_Cfg_File(); // Parse the confiuration file

            Check_compiler_path(); //Update the compiler path

            initialize_file_list(); // Init the file list

            //start the backwork1 for the paths search of everything,inca and total commander
            backgroundWorker1.RunWorkerAsync();


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
                dr = MessageBox.Show(" The TASKING is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(tasking_setup_path);
                    statusstrip_info_print("Info:The compiler has been opened!");
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
                statusstrip_info_print("Info:The compiler has been opened!");
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
                dr = MessageBox.Show(" The MATLAB is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(matlab_setup_path);
                    statusstrip_info_print("Info:The MATLAB has been opened!");
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
                statusstrip_info_print("Info:The MATLAB has been opened!");
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
                dr = MessageBox.Show(" The SmartGit is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(smartgit_setup_path);
                    statusstrip_info_print("Info:The SmartGit has been opened!");
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
                statusstrip_info_print("Info:The SmartGit has been opened!");
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
                dr = MessageBox.Show(" The UDE is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(ude_setup_path);
                    statusstrip_info_print("Info:The UDE has been opened!");
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
                statusstrip_info_print("Info:The UDE has been opened!");
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
                dr = MessageBox.Show(" The Insight3 is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(sourceinsight_setup_path);
                    statusstrip_info_print("Info:The Source Insight has been opened!");
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
                statusstrip_info_print("Info:The Source Insight has been opened!");
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

        /*****************************************************************
        * Total Commander button
        ******************************************************************/
        private void toolStripButton19_ButtonClick(object sender, EventArgs e)
        {
            //total commander
            if (System.Diagnostics.Process.GetProcessesByName("TOTALCMD").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The totalcmd is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(totalcommander_setup_path);
                    statusstrip_info_print("Info:The totalcmd has been opened!");
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                //no 
                Process.Start(totalcommander_setup_path);
                statusstrip_info_print("Info:The totalcmd has been opened!");
            }
        }
        //totalcmd: open path
        private void openPath_totalcmd_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Org_totalcommander_setup_path);
        }
        //totalcmd: copy path
        private void copyPath_totalcmd_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, Org_totalcommander_setup_path); //copy target into Clipboard
        }
        //totalcmd: end process
        private void endprocess_totalcmd_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "TOTALCMD")
                {
                    item.Kill();
                }
            }
        }
        /*****************************************************************
        * Total Commander end
        ******************************************************************/

        /*****************************************************************
        * Everything button
        ******************************************************************/
        private void toolStripButton20_ButtonClick(object sender, EventArgs e)
        {
            //everything
            if (System.Diagnostics.Process.GetProcessesByName("Everything").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The Everything is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process.Start(everything_setup_path);
                    statusstrip_info_print("Info:The Everything has been opened!");
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                //no 
                Process.Start(everything_setup_path);
                statusstrip_info_print("Info:The Everything has been opened!");
            }
        }
        //everything: open path
        private void openPath_everything_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Org_everything_setup_path);
        }
        //everything: copy path
        private void copyPath_everything_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, Org_everything_setup_path); //copy target into Clipboard
        }
        //everything: end process
        private void endprocess_everything_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "Everything")
                {
                    item.Kill();
                }
            }
        }
        /*****************************************************************
        * Everything button end
        ******************************************************************/
        /*****************************************************************
        * inca button
        ******************************************************************/
        private void toolStripButton6_ButtonClick(object sender, EventArgs e)
        {
            //inca
            if (System.Diagnostics.Process.GetProcessesByName("Inca").ToList().Count > 0)
            {
                //yes 
                DialogResult dr;
                dr = MessageBox.Show(" The INCA is running now!\n Do you want to open another one?", "Notice", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    Process p = new Process(); //Create a local process
                    p.StartInfo.FileName = "cmd.exe";  //Set the program name
                    p.StartInfo.UseShellExecute = false;    //Disable the shell to be started!
                    p.StartInfo.RedirectStandardInput = true;  //Set the redirect input
                    p.StartInfo.RedirectStandardOutput = true; //Set the redirect output  
                    p.StartInfo.RedirectStandardError = true;  //Set the redirect error      
                    p.StartInfo.CreateNoWindow = true;  //Don't show the process in window.

                    p.Start();    //start the process

                    //p.StandardInput.WriteLine(@"cd /d C:\ETAS\INCA7.1");
                    p.StandardInput.WriteLine("cd" + "/d" + Org_inca_setup_path);

                    //p.StandardInput.WriteLine(@"C:\ETAS\INCA7.1\Inca.exe -ietas.icx");
                    p.StandardInput.WriteLine(inca_setup_path + " " + inca_arguments);

                    p.Close(); //Close the local process

                    statusstrip_info_print("Info:The INCA has been opened!");
                }
                else if (dr == DialogResult.No)
                {
                    //do nothing!
                }
            }
            else
            {
                Process p = new Process(); //Create a local process
                p.StartInfo.FileName = "cmd.exe";  //Set the program name
                p.StartInfo.UseShellExecute = false;    //Disable the shell to be started!
                p.StartInfo.RedirectStandardInput = true;  //Set the redirect input
                p.StartInfo.RedirectStandardOutput = true; //Set the redirect output  
                p.StartInfo.RedirectStandardError = true;  //Set the redirect error      
                p.StartInfo.CreateNoWindow = true;  //Don't show the process in window.

                p.Start();    //start the process

                //p.StandardInput.WriteLine(@"cd /d C:\ETAS\INCA7.1");
                p.StandardInput.WriteLine("cd" + "/d" + Org_inca_setup_path);

                //p.StartInfo.Verb = "runas";
                //p.StandardInput.WriteLine(@"C:\ETAS\INCA7.1\Inca.exe -ietas.icx");
                p.StandardInput.WriteLine(inca_setup_path + " " + inca_arguments);

                p.Close(); //Close the local process

                statusstrip_info_print("Info:The INCA has been opened!" + inca_setup_path + inca_arguments);
            }
        }

        //inca: open path
        private void openPath_inca_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Org_inca_setup_path);
        }
        //inca: copy path
        private void copyPath_inca_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, Org_inca_setup_path); //copy target into Clipboard
        }
        //inca: end process
        private void endprocess_inca_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName == "Inca")
                {
                    item.Kill();
                }
            }
        }
        /*****************************************************************
        * inca button end
        ******************************************************************/

        public void Write_File(string path, string info_stream)
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
                textBox1.AppendText(e.Data + "\r\n");
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
                textBox1.AppendText(e.Data + "\r\n");
            }
            //statusstrip_info_control(false);
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
            toolStripButton21.Enabled = actived_status;
            toolStripButton25.Enabled = actived_status;
            textBox2.Enabled = actived_status;
            toolStripTextBox1.Enabled = actived_status;
            toolStripTextBox2.Enabled = actived_status;
            toolStripTextBox3.Enabled = actived_status;

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
            toolStripStatusLabel4.Text = "Info:Processing 'Clean...'";
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
            //launch_process(@"cd /d C:\ETAS\INCA7.1",@"C:\ETAS\INCA7.1\Inca.exe -ietas.icx");//launch the command process
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
        * Description:Update the configuration file(.ini)
        ******************************************************************/
        private void statusstrip_info_print(string info)
        {
            statusstrip_info_control(false);
            toolStripStatusLabel4.Visible = true;
            toolStripStatusLabel4.Text = info;
        }
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
            string out_into_file = null;

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

                        statusstrip_info_print("Info:The project name has been changed!");
                        MessageBox.Show("The project name has been changed!");
                    }
                    else if (dr == DialogResult.No)
                    {
                        statusstrip_info_print("Info:The project name has been kept unchanged!");
                        MessageBox.Show("The project name has been kept unchanged!");
                    }
                }
                else
                {
                    //do nothing
                    statusstrip_info_print("Info:The project name has not been changed!");
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
                    cfg_app_sw_ver = resultString[1];
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

                        statusstrip_info_print("Info:The version of the application software has been changed!");
                        MessageBox.Show("The version of the application software has been changed!");
                    }
                    else if (dr == DialogResult.No)
                    {
                        statusstrip_info_print("Info:The version of the application software has been kept unchanged!");
                        MessageBox.Show("The version of the application software has been kept unchanged!");
                    }
                }
                else
                {
                    //do nothing
                    statusstrip_info_print("Info:The version of the application software has not been changed!");
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
                    cfg_low_sw_ver = resultString[1];
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

                        statusstrip_info_print("Info:The the version of the low driver sofware has been changed!");
                        MessageBox.Show("The the version of the low driver sofware has been changed!");
                    }
                    else if (dr == DialogResult.No)
                    {
                        statusstrip_info_print("Info:The the version of the low driver sofware has been kept unchanged!");
                        MessageBox.Show("The the version of the low driver sofware has been kept unchanged!");
                    }
                }
                else
                {
                    //do nothing
                    statusstrip_info_print("Info:The version of the low driver sofware has not been changed!");
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
            statusstrip_info_print("Info:The command line of WINDOWS has been opened!");
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

        //Clean the current screen.
        private void toolStripButton11_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";  //clear the info windows first.
            statusstrip_info_print("Info:The screen has been cleared!");

            everytool_cfg_file_read(@"C:\Users\bai\Desktop\Everytool.ini");
            //initialize_file_list();
            return;
            //test code
            //String path = @"E:\ECU\WorkArea\K245";

            //第一种方法
            //var files = Directory.GetFiles(path, "*.*");
            //var files = Directory.GetFiles(path);

            //var files = Directory.GetDirectories(path);
            //var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            //foreach (var file in files)
            //{
            //Console.WriteLine(System.IO.Path.GetFileName(file));
            //   Console.WriteLine(file);
            //}
            //Console.WriteLine(files.Length);


            //string[] fileNames = Directory.GetFiles(path);
            //string[] directories = Directory.GetDirectories(path); 
            //test code
        }

        private void asToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\thinkpad\Desktop\makefile";
            System.Diagnostics.Process.Start(path);
        }

        //shortcuts
        /*****************************************************************
        *
        * shortcuts
        * 
        ******************************************************************/
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        //Setup List
        private void setupListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show the help context!
            Form4 f = new Form4();
            f.Show();
        }


        /*****************************************************************
        *
        * Background Tasking used to search the paths of everything total commander and inca
        * 
        ******************************************************************/
        int everything_search_count;
        int inca_search_count;
        int totalcmd_search_count;

        string[] everything_search_path_array = new string[30];
        string[] inca_search_path_array = new string[30];
        string[] totalcmd_search_path_array = new string[30];
        string[] temp_search_path_array = new string[30];

        int FindDirectory(String dirname)
        {
            String[] logicDrivers = Environment.GetLogicalDrives();
            int count = 0;
            for (int i = 0; i < logicDrivers.Length; i++)
            {
                List<String> dirlist = new List<string>();
                getDirs(logicDrivers[i], dirname, dirlist);
                String[] dirs = dirlist.ToArray();
                for (int j = 0; j < dirs.Length; j++)
                {
                    Console.WriteLine(dirs[j]);
                    temp_search_path_array[count] = dirs[j];
                    count++;
                    //this.textBox1.AppendText(dirs[j] + "\n");
                }
            }
            return count;
        }

        void getDirs(String dirpath, String dirname, List<String> dirlist)
        {
            try
            {
                dirlist.AddRange(Directory.GetDirectories(dirpath, dirname, SearchOption.TopDirectoryOnly));
                String[] dirs = Directory.GetDirectories(dirpath);
                for (int i = 0; i < dirs.Length; i++)
                {
                    getDirs(dirs[i], dirname, dirlist);
                }
            }
            catch
            {
                return;
            }
        }

        //background task intended to deal with the path search.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //@Everything
            //backgroundWorker1.ReportProgress(0);
            //string bkgd_search_everything_dir = @"Everything";
            if (everything_cfgfile_detected == false)
            {
                everything_search_count = FindDirectory(bkgd_search_everything_dir);
                if (everything_search_count > 0)
                {
                    Array.Copy(temp_search_path_array, 0, everything_search_path_array, 0, everything_search_count);
                }

            }


            //@INCA
            //backgroundWorker1.ReportProgress(1);
            //string bkgd_search_inca_dir = @"ETAS\INCA7.1";
            if (inca_cfgfile_detected == false)
            {
                inca_search_count = FindDirectory(bkgd_search_inca_dir);
                if (inca_search_count > 0)
                {
                    Array.Copy(temp_search_path_array, 0, inca_search_path_array, 0, inca_search_count);
                }
            }


            //@Totalcommander
            //backgroundWorker1.ReportProgress(2);
            //string bkgd_search_totalcommand_dir = @"totalcmd";
            if (totalcmd_cfgfile_detected == false)
            {
                totalcmd_search_count = FindDirectory(bkgd_search_totalcommand_dir);
                if (totalcmd_search_count > 0)
                {
                    Array.Copy(temp_search_path_array, 0, totalcmd_search_path_array, 0, totalcmd_search_count);
                }
            }
            //backgroundWorker1.ReportProgress(3);
        }

        //Report the info to main thread.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Everything
            if (everything_cfgfile_detected == false)
            {
                if (everything_search_count > 0)
                {
                    int index;
                    for (index = 0; index < everything_search_count; index++)
                    {
                        string temp_everything_setup_path;

                        temp_everything_setup_path = everything_search_path_array[index] + everything_suffix_path;
                        if (System.IO.File.Exists(temp_everything_setup_path))
                        {
                            Org_everything_setup_path = everything_search_path_array[index]; //Update path
                            everything_setup_path = temp_everything_setup_path; //Update path
                            toolStripButton20.Enabled = true; //The button is enabled because of the the valid everything path is found!
                            break; // jump out of the for loop.
                        }
                        else
                        {
                            toolStripButton20.Enabled = false;
                        }
                    }
                }
            }

            //inca
            if (inca_cfgfile_detected == false)
            {
                if (inca_search_count > 0)
                {
                    int index;
                    for (index = 0; index < inca_search_count; index++)
                    {
                        string temp_inca_setup_path;

                        temp_inca_setup_path = inca_search_path_array[index] + inca_suffix_path;
                        if (System.IO.File.Exists(temp_inca_setup_path))
                        {
                            Org_inca_setup_path = inca_search_path_array[index]; //Update path
                            inca_setup_path = temp_inca_setup_path; //Update path
                            toolStripButton6.Enabled = true; //The button is enabled because of the the valid inca path is found!
                            break; // jump out of the for loop.
                        }
                        else
                        {
                            toolStripButton6.Enabled = false;
                        }
                    }
                }
            }


            //Totalcommander
            if (totalcmd_cfgfile_detected == false)
            {
                if (totalcmd_search_count > 0)
                {
                    int index;
                    for (index = 0; index < totalcmd_search_count; index++)
                    {
                        string temp_totalcmd_setup_path;

                        temp_totalcmd_setup_path = totalcmd_search_path_array[index] + totalcmd_suffix_path;
                        if (System.IO.File.Exists(temp_totalcmd_setup_path))
                        {
                            Org_totalcommander_setup_path = totalcmd_search_path_array[index]; //Update path
                            totalcommander_setup_path = temp_totalcmd_setup_path; //Update path
                            toolStripButton19.Enabled = true; //The button is enabled because of the the valid totalcmd path is found!
                            break; // jump out of the for loop.
                        }
                        else
                        {
                            toolStripButton19.Enabled = false;
                        }
                    }
                }
            }

            //MessageBox.Show("异步执行完毕");
            //Console.WriteLine("xiyanpeng_length: {0}", everything_search_count);
            //foreach(string element in everything_search_path_array)
            //{
            //    Console.WriteLine("xiyanpeng: {0}", element);
            //}
            update_cfg_file_everytool();

        }

        // Update the cfg file of everytool
        private void update_cfg_file_everytool()
        {
            //bool tasking_cfgfile_detected = false;
            //bool matlab_cfgfile_detected = false;
            //bool ude_cfgfile_detected = false;
            //bool smartgit_cfgfile_detected = false;
            //bool everything_cfgfile_detected = false;
            //bool inca_cfgfile_detected = false;
            //bool totalcmd_cfgfile_detected = false;
            //bool sourceinsight_cfgfile_detected = false;

            //string Org_tasking_setup_path = "";
            //string Org_matlab_setup_path = "";
            //string Org_smartgit_setup_path = "";
            //string Org_ude_setup_path = "";
            //string Org_inca_setup_path = "";
            //string Org_sourceinsight_setup_path = "";
            //string Org_everything_setup_path = "";
            //string Org_totalcommander_setup_path = "";

            //tasking
            if (tasking_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[8]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[8] + "=" + Org_tasking_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }

            //matlab
            if (matlab_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[9]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[9] + "=" + Org_matlab_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }

            //ude
            if (ude_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[10]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[10] + "=" + Org_ude_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }

            //smartgit
            if (smartgit_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[11]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[11] + "=" + Org_smartgit_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }

            //everything
            if (everything_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[12]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[12] + "=" + Org_everything_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }

            //inca
            if (inca_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[13]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[13] + "=" + Org_inca_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }

            //totalcmd
            if (totalcmd_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[14]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[14] + "=" + Org_totalcommander_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }

            //source insight
            if (sourceinsight_cfgfile_detected == false)
            {
                //read the configured file
                string out_info = "";
                StreamReader sr = new StreamReader(@"C:\Users\bai\Desktop\Everytool.ini", Encoding.Default);
                String line;
                string temp;
                while ((line = sr.ReadLine()) != null)
                {
                    Match result = Regex.Match(line, cfg_attribute[15]);
                    if (result.Success == true)
                    {
                        //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                        temp = cfg_attribute[15] + "=" + Org_sourceinsight_setup_path;
                    }
                    else
                    {
                        temp = line;
                    }
                    out_info = out_info + temp + "\r\n";
                }
                sr.Close();
                Write_File(@"C:\Users\bai\Desktop\Everytool.ini", out_info);
            }
        }

        //The progress report thread is processed by the main.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    statusstrip_info_print("Info: Searching WINDOWS for Everything...");
                    break;
                case 1:
                    statusstrip_info_print("Info: Searching WINDOWS for INCA...");
                    break;
                case 2:
                    statusstrip_info_print("Info: Searching WINDOWS for totalcmd");
                    break;
                case 3:
                    statusstrip_info_print("Info: Everything, INCA and totalcmd have be detected in the PC!");
                    break;
                default:
                    break;
            }
        }

        /*****************************************************************
        *
        * Background Tasking used to search the paths of everything total commander and inca
        * 
        ******************************************************************/
        string[] FILE_NAME_LIST;
        string[] FILE_PATH_LIST;
        int FILE_LIST_SIZE;

        //the function is called in background.
        private void initialize_file_list()
        {
            // String path = @"E:\WorkArea\K245ECU\01_Mak";
            String path = @"E:\ECU\WorkArea\K245";
            //String path = @"E:\ECU\WorkArea\K245";
            int i = 0;

            FILE_PATH_LIST = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            //FILE_NAME_LIST = FILE_PATH_LIST;

            ImageList imgLst = new ImageList(); // define icon list
            //*****************************************************************
            //Update listview
            this.listView1.BeginUpdate();
            while (i < FILE_PATH_LIST.Length)
            {

                ListViewItem lvi = new ListViewItem();
                FileInfo f = new FileInfo(FILE_PATH_LIST[i]);

                //add icon
                //     System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(FILE_PATH_LIST[i]); 
                //     imgLst.Images.Add(fileIcon);
                //     listView1.SmallImageList = imgLst;//小图标模式下 显示这个图标

                //     lvi.ImageIndex = i;   
                lvi.Text = System.IO.Path.GetFileName(FILE_PATH_LIST[i]);

                lvi.SubItems.Add(FILE_PATH_LIST[i]);
                //     lvi.SubItems.Add(f.Length.ToString());
                lvi.SubItems.Add("");
                lvi.SubItems.Add(f.LastWriteTime.ToString());

                this.listView1.Items.Add(lvi);

                //listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                i++;
            }
            this.listView1.EndUpdate();
            //this.listView1.Items.Clear();
            //Update listview end
            //*****************************************************************

            Console.WriteLine(FILE_PATH_LIST.Length);

            //string[] fileNames = Directory.GetFiles(path);
            //string[] directories = Directory.GetDirectories(path); 
            //test code      
        }

        //when the search changed update the listview
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            //MessageBox.Show("selected"); 
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //MessageBox.Show("selected"); 
        }

        int selectCount = 0;
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                selectCount = listView1.SelectedItems.Count; //SelectedItems.Count
                if (selectCount > 0)//if selectcount >0 ,there is item selected!
                {
                    // Console.WriteLine("xiyanpeng: {0}", listView1.SelectedItems[0].SubItems[0].Text);
                    // Console.WriteLine("xiyanpeng: {0}", listView1.SelectedItems[0].SubItems[1].Text);
                    // Console.WriteLine("xiyanpeng: {0}", listView1.SelectedItems[0].SubItems[2].Text);
                    // Console.WriteLine("xiyanpeng: {0}", selectCount);

                    //txtName.Text =  listView1.SelectedItems[0].SubItems[0].Text;
                    //txtAge.Text  =  listView1.SelectedItems[0].SubItems[1].Text;
                    //txtSex.Text  =  listView1.SelectedItems[0].SubItems[2].Text;

                    //show menu strip in listview
                    selectCount = 0;
                    contextMenuStrip1.Show(listView1, e.Location);
                }

                //listView1.ContextMenuStrip = null;
                //contextMenuStrip2.Show(listView1, e.Location);

                //MessageBox.Show("MouseButton Right Clicked");

            }
        }

        /*****************************************************************
        *
        * Context menu strip
        * 
        ******************************************************************/
        //open file
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("xiyanpeng: {0}", listView1.SelectedItems[0].SubItems[0].Text);
            //Console.WriteLine("xiyanpeng: {0}", listView1.SelectedItems[0].SubItems[1].Text);
            //Console.WriteLine("xiyanpeng: {0}", listView1.SelectedItems[0].SubItems[2].Text);

            //Console.WriteLine("xiyanpeng: {0}", info.Parent.FullName);
            //Console.WriteLine("xiyanpeng: {0}", selectCount);

            Process.Start(listView1.SelectedItems[0].SubItems[1].Text);
        }

        //open path
        private void openPathToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            //DirectoryInfo info  = new DirectoryInfo(listView1.SelectedItems[0].SubItems[1].Text);
            //Console.WriteLine("xiyanpeng: {0}", info.Parent.FullName);

            string fileToSelect = listView1.SelectedItems[0].SubItems[1].Text;
            string args = string.Format("/Select, {0}", fileToSelect);

            ProcessStartInfo pfi = new ProcessStartInfo("Explorer.exe", args);
            System.Diagnostics.Process.Start(pfi);
        }

        //copy file full name into Clipboard
        private void copyFullNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();//clear Clipboard 
            Clipboard.SetData(DataFormats.Text, listView1.SelectedItems[0].SubItems[1].Text); //copy target into Clipboard
        }

        //double click open

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            selectCount = listView1.SelectedItems.Count; //SelectedItems.Count
            if (selectCount > 0)//if selectcount >0 ,there is item selected!
            {
                //show menu strip in listview
                selectCount = 0;
                Process.Start(listView1.SelectedItems[0].SubItems[1].Text);
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            //Update the software version
            if (e.KeyCode == Keys.Enter)
            {
                int i = 0;
                ///MessageBox.Show("changed");  test ok

                //clear the listview
                this.listView1.Items.Clear();

                //Get the pattern input from textbox input
                string input_pattern = textBox3.Text.Trim();

                ImageList imgLst = new ImageList(); // define icon list
                this.listView1.BeginUpdate();
                foreach (string element in FILE_PATH_LIST)
                {
                    string str = System.IO.Path.GetFileName(element);
                    //*****************************************************************
                    //Update listview
                    if (str.IndexOf(input_pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    //if (Regex.IsMatch(str, input_pattern, RegexOptions.IgnoreCase))
                    {
                        //this.listView1.BeginUpdate();
                        ListViewItem lvi = new ListViewItem();
                        FileInfo f = new FileInfo(element);

                        //System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(element);
                        //imgLst.Images.Add(fileIcon);
                        //listView1.SmallImageList = imgLst;//小图标模式下 显示这个图标

                        //lvi.ImageIndex = i;
                        lvi.Text = System.IO.Path.GetFileName(element);

                        lvi.SubItems.Add(element);
                        //lvi.SubItems.Add(f.Length.ToString
                        lvi.SubItems.Add("");
                        lvi.SubItems.Add(f.LastWriteTime.ToString());

                        this.listView1.Items.Add(lvi);
                        //this.listView1.EndUpdate();
                        //listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                        i++;
                    }
                    this.listView1.EndUpdate();
                    //Update listview end
                    //*****************************************************************
                }

                //backup
                // foreach (string element in FILE_PATH_LIST)
                // {
                //     string str = System.IO.Path.GetFileName(element);
                //*****************************************************************
                //Update listview
                //      if (Regex.IsMatch(str, input_pattern, RegexOptions.IgnoreCase))
                //      {
                //this.listView1.BeginUpdate();
                //          ListViewItem lvi = new ListViewItem();
                //          FileInfo f = new FileInfo(element);

                //          System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(element);
                //          imgLst.Images.Add(fileIcon);
                //          listView1.SmallImageList = imgLst;//小图标模式下 显示这个图标

                //          lvi.ImageIndex = i;
                //          lvi.Text = System.IO.Path.GetFileName(element);

                //          lvi.SubItems.Add(element);
                //          lvi.SubItems.Add(f.Length.ToString());
                //          lvi.SubItems.Add(f.LastWriteTime.ToString());

                //          this.listView1.Items.Add(lvi);
                //this.listView1.EndUpdate();
                //         //listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                //           i++;
                //      }
                //      this.listView1.EndUpdate();
                //Update listview end
                //*****************************************************************
                //   }
            }
        }

        private void copyPathToolStripMenuItem6_Click(object sender, EventArgs e)
        {

        }

        /*****************************************************************
        *
        * Tool Check
        * 
        ******************************************************************/
        private void update_everytool_cfg_file(string cfg_path,string attri, string value)
        {
            //read the configured file
            string out_info = "";
            StreamReader sr = new StreamReader(cfg_path, Encoding.Default);
            String line;
            string temp;
            while ((line = sr.ReadLine()) != null)
            {
                Match result = Regex.Match(line, attri);
                if (result.Success == true)
                {
                    //temp = line.Replace(cfg_low_sw_ver, toolStripTextBox3.Text);
                    temp = attri + "=" + value;
                }
                else
                {
                    temp = line;
                }
                out_info = out_info + temp + "\r\n";
            }
            sr.Close();
            Write_File(cfg_path, out_info);
        }
        private void tASKINGToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (tASKINGToolStripMenuItem.Checked == true)
            {
                toolStripButton1.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[0], "1");

            }
            else
            {
                toolStripButton1.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[0], "0");
            }
        }

        private void matlabToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (matlabToolStripMenuItem.Checked == true)
            {
                toolStripButton2.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[1], "1");
            }
            else
            {
                toolStripButton2.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[1], "0");
            }
        }

        private void smartGitToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (smartGitToolStripMenuItem.Checked == true)
            {
                toolStripButton3.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[3], "1");
            }
            else
            {
                toolStripButton3.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[3], "0");
            }
        }

        private void uDEToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (uDEToolStripMenuItem.Checked == true)
            {
                toolStripButton5.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[2], "1");
            }
            else
            {
                toolStripButton5.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[2], "0");
            }
        }

        private void sourceInsightToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (sourceInsightToolStripMenuItem.Checked == true)
            {
                toolStripButton18.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[7], "1");
            }
            else
            {
                toolStripButton18.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[7], "0");
            }
        }

        private void totalCommanderToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (totalCommanderToolStripMenuItem.Checked == true)
            {
                toolStripButton19.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[6], "1");
            }
            else
            {
                toolStripButton19.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[6], "1");
            }
        }

        private void everythingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (everythingToolStripMenuItem.Checked == true)
            {
                toolStripButton20.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[4], "1");
            }
            else
            {
                toolStripButton20.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[4], "0");
            }
        }

        private void iNCAToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (iNCAToolStripMenuItem.Checked == true)
            {
                toolStripButton6.Visible = true;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[5], "1");
            }
            else
            {
                toolStripButton6.Visible = false;
                update_everytool_cfg_file(@"C:\Users\bai\Desktop\Everytool.ini", cfg_attribute[5], "0");
            }
        }


        /*****************************************************************
        *
        * Everytool cfg file 
        * 
        ******************************************************************/
        string[] cfg_attribute = new string[16]
        {
          "compiler_visible",           //cfg option
          "matlab_visible",             //cfg option
          "ude_visible",                //cfg option
          "smartgit_visible",           //cfg option
          "everything_visible",         //cfg option  
          "inca_visible",               //cfg option
          "totalcmd_visible",           //cfg option
          "source_insight_visible",     //cfg option
          "compiler_path",              //cfg tool
          "matlab_path",                //cfg tool
          "ude_path",                   //cfg tool 
          "smartgit_path",              //cfg tool
          "everything",                 //cfg tool
          "inca_path",                  //cfg tool 
          "totalcmd_path",              //cfg tool
          "source_insight_path"         //cfg tool
        };
        string[] cfg_value = new string[16]; //store the value of cfg attributes
        bool tasking_cfgfile_detected = false;
        bool matlab_cfgfile_detected = false;
        bool ude_cfgfile_detected = false;
        bool smartgit_cfgfile_detected = false;
        bool everything_cfgfile_detected = false;
        bool inca_cfgfile_detected = false;
        bool totalcmd_cfgfile_detected = false;
        bool sourceinsight_cfgfile_detected = false;

        private void everytool_cfg_file_read(string cfg_path)
        {
            //check if the file exits
            if (System.IO.File.Exists(cfg_path) == false)
            {
                //create the file with the current file configuration or create it after all paths have
                //be detected1
            }
            else
            {
                StreamReader sr = new StreamReader(cfg_path, Encoding.Default);
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.IndexOf(cfg_attribute[0]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[0] = resultString[1].Trim();

                        Console.WriteLine(resultString[0]);
                    }
                    else if (line.IndexOf(cfg_attribute[1]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[1] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[2]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[2] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[3]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[3] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[4]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[4] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[5]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[5] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[6]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[6] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[7]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[7] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[8]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[8] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[9]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[9] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[10]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[10] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[11]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[11] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[12]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[12] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[13]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[13] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[14]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[14] = resultString[1].Trim();
                    }
                    else if (line.IndexOf(cfg_attribute[15]) >= 0)
                    {
                        string[] resultString = Regex.Split(line, "=", RegexOptions.IgnoreCase);
                        cfg_value[15] = resultString[1].Trim();
                    }
                }
                sr.Close();

                int index;

                //for (index = 0; index < cfg_value.Length; index++)
                for (index = 0; index < 8; index++)
                {
                    Console.WriteLine(cfg_attribute[index]);
                    Console.WriteLine(cfg_value[index]);
                }


                //valid or invalid the visible options
                //check index 0 - 7 update the check options of tools
                //tasking
                if (cfg_value[0] == "1")
                {
                    tASKINGToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[0] == "0")
                {
                    tASKINGToolStripMenuItem.Checked = false;
                }

                //matlab
                if (cfg_value[1] == "1")
                {
                    matlabToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[1] == "0")
                {
                    matlabToolStripMenuItem.Checked = false;
                }
                //ude
                if (cfg_value[2] == "1")
                {
                    uDEToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[2] == "0")
                {
                    uDEToolStripMenuItem.Checked = false;
                }
                //smartgit
                if (cfg_value[3] == "1")
                {
                    smartGitToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[3] == "0")
                {
                    smartGitToolStripMenuItem.Checked = false;
                }

                if (cfg_value[4] == "1")
                {
                    everythingToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[4] == "0")
                {
                    everythingToolStripMenuItem.Checked = false;
                }

                if (cfg_value[5] == "1")
                {
                    iNCAToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[5] == "0")
                {
                    iNCAToolStripMenuItem.Checked = false;
                }

                if (cfg_value[6] == "1")
                {
                    totalCommanderToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[6] == "0")
                {
                    totalCommanderToolStripMenuItem.Checked = false;
                }
                if (cfg_value[7] == "1")
                {
                    sourceInsightToolStripMenuItem.Checked = true;
                }
                else if (cfg_value[7] == "0")
                {
                    sourceInsightToolStripMenuItem.Checked = false;
                }

                //check if the paths valid
                //tasking 
                string temp_path;
                //taskling
                temp_path = cfg_value[8] + tasking_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    tasking_setup_path = temp_path;
                    Org_tasking_setup_path = cfg_value[8];
                    tasking_cfgfile_detected = true;
                }
                else
                {
                    tasking_cfgfile_detected = false;
                }
                
                //matlab
                temp_path = cfg_value[9] + matlab_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    matlab_setup_path = temp_path;
                    matlab_cfgfile_detected = true;
                }
                else
                {
                    matlab_cfgfile_detected = false;
                }

                //ude
                temp_path = cfg_value[10] + ude_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    ude_setup_path = temp_path;
                    ude_cfgfile_detected = true;
                }
                else
                {
                    ude_cfgfile_detected = false;
                }

                temp_path = cfg_value[11] + smartgit_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    smartgit_setup_path = temp_path;
                    smartgit_cfgfile_detected = true;
                }
                else
                {
                    smartgit_cfgfile_detected = false;
                }

                temp_path = cfg_value[12] + everything_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    everything_setup_path = temp_path;
                    everything_cfgfile_detected = true;
                }
                else
                {
                    everything_cfgfile_detected = false;
                }

                temp_path = cfg_value[13] + inca_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    inca_setup_path = temp_path;
                    inca_cfgfile_detected = true;
                }
                else
                {
                    inca_cfgfile_detected = false;
                }

                temp_path = cfg_value[14] + totalcmd_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    totalcommander_setup_path = temp_path;
                    totalcmd_cfgfile_detected = true;
                }
                else
                {
                    totalcmd_cfgfile_detected = false;
                }

                temp_path = cfg_value[15] + sourceinsight_suffix_path;
                if (System.IO.File.Exists(temp_path))
                {
                    sourceinsight_setup_path = temp_path;
                    sourceinsight_cfgfile_detected = true;
                }
                else
                {
                    sourceinsight_cfgfile_detected = false;
                }
                //yes use the path
                //no search the new one 
            }

        }

    }
}
