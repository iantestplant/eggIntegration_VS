using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Process = System.Diagnostics.Process;
using Microsoft.Win32;

namespace TestPlant.EggPlantVSPackage
{
    public partial class ePF_TestCreation : Form
    {
        public string SuitePath;
        private Project _activeProject;
        private string _activeProjectName;
        private IPAddress[] _addr;
        private AppSettings _appSettings;
        private string _commandRunScript;
        private string _strHostName;

        public ePF_TestCreation()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // get the local hostname and IP addresses
            _strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(_strHostName);
            _addr = ipEntry.AddressList;

            // populate from the application sessions
            _appSettings = new AppSettings();
            hostValue.Text = _appSettings.Host;
            spin_portValue.Value = _appSettings.Port;
            SuitePath_text.Text = _appSettings.Suite;
            radioButton_drive.Enabled = _appSettings.DriveMode;
            EnableControls();
        }

        private void suitePath_btn_Click(object sender, EventArgs e)
        {
            folderBrowserDialog_suite.Description = Resources.ePF_folder_description;
            folderBrowserDialog_suite.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog_suite.SelectedPath = @"C:\";
            if (folderBrowserDialog_suite.ShowDialog() != DialogResult.OK)
                return;
            SuitePath_text.Clear();
            SuitePath_text.AppendText(folderBrowserDialog_suite.SelectedPath);
        }

        private void addScript_button_Click(object sender, EventArgs e)
        {
            string initialFolder = @"C:\";
            if (!String.IsNullOrEmpty(SuitePath_text.Text) && Directory.Exists(SuitePath_text.Text))
            {
                initialFolder = SuitePath_text.Text;
                string folder = Path.Combine(SuitePath_text.Text, "Scripts");
                if (Directory.Exists(folder))
                    initialFolder = folder;
            }

            openFileDialog_suite.InitialDirectory = initialFolder;
            openFileDialog_suite.Filter = @"Script files (*.script)|*.script|All files (*.*)|*.*";
            openFileDialog_suite.FilterIndex = 1;
            openFileDialog_suite.RestoreDirectory = true;
            openFileDialog_suite.Multiselect = true;
            openFileDialog_suite.FileName = "";

            if (openFileDialog_suite.ShowDialog() != DialogResult.OK)
                return;
            foreach (string scriptName in openFileDialog_suite.SafeFileNames)
            {
                string script = Path.GetFileNameWithoutExtension(scriptName);
                bool found =
                    dataGridView1.Rows.Cast<DataGridViewRow>()
                        .Any(row => !row.IsNewRow && row.Cells[0] != null && row.Cells[0].Value.ToString() == script);
                if (!found)
                    dataGridView1.Rows.Add(new object[] {script});
            }
        }

        private void button_removeScript_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow)
                    dataGridView1.Rows.Remove(row);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            button_removeScript.Enabled = (dataGridView1.SelectedRows.Count > 0);
            button_OK.Enabled = (dataGridView1.SelectedRows.Count > 0);
        }

        private void host_TextChanged(object sender, EventArgs e)
        {
            button_suitePath.Enabled = button_addScript.Enabled = false;
            string host = hostValue.Text.Trim().ToLowerInvariant();
            button_suitePath.Enabled = button_addScript.Enabled =
                (host.Equals("localhost") ||
                 host.Equals("127.0.0.1")) ||
                host.Equals(_strHostName)
                ;
            foreach (IPAddress ip in _addr)
            {
                if (ip.ToString() == host)
                    button_suitePath.Enabled = button_addScript.Enabled = true;
            }
        }

        public bool CheckForProjects()
        {
            IList<Project> projects = SolutionProjects.Projects();
            if (projects.Count == 0)
            {
                MessageBox.Show("No Projects are open", Resources.ePF_Msgbox_Title, MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }

            _activeProject = projects[0];
            _activeProjectName = projects[0].FullName;
            var projs = SolutionProjects.GetActiveIDE().ActiveSolutionProjects as Array;
            if (projs != null && projs.Length > 0)
            {
                IEnumerator item = projs.GetEnumerator();
                if (item.MoveNext())
                {
                    var project = item.Current as Project;
                    if (project != null)
                    {
                        _activeProjectName = project.FullName;
                        _activeProject = project;
                    }
                }
            }

            // Find RunEPFPackage.exe
            return GetCommandPath();
        }

        private bool GetCommandPath()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\TestPlant\eggIntegrationVS");
                if (key != null)
                {
                    Object o = key.GetValue("InstallPath");
                    if (o != null)
                    {
                        string installPath = o as String;
                        _commandRunScript = Path.Combine(installPath, "RunEPFScriptVS.exe");
                        if (File.Exists(_commandRunScript))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //(@"Failed to read HKLM\Software\TestPlant\eggIntegrationVS\InstallPath: " + ex.ToString());
                // Ignore the error and look for RunEPFScriptVS.exe in well know places
            }

            var folders = new List<string>
            {
                Environment.GetEnvironmentVariable("ProgramFiles"),
                Environment.GetEnvironmentVariable("ProgramFiles(x86)"),
                Environment.GetEnvironmentVariable("EGGPLANT_VS") // as a last resort, allow use of EGGPLANT_VS
            };
            foreach (string f in folders.Where(f => !String.IsNullOrEmpty(f)))
            {
                _commandRunScript = Path.Combine(f, "eggIntegration for Visual Studio", "RunEPFScriptVS.exe");
            }

            if (!File.Exists(_commandRunScript))
            {
                MessageBox.Show(
                    "Failed to find required installed files. Check the installation of eggIntegration for Visual Studio\n" +
                    "If the install folder \"eggIntegration for Visual Studio\" is not under %ProgramFiles% or%ProgramFiles(x86)%\n" +
                    "you can set the environment variable %EGGPLANT_VS% to the parent of this folder.",
                    Resources.ePF_Msgbox_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void AddGenericTests()
        {
            if (dataGridView1.Rows.Count < 2) // only contains the 'new' row?
                return;

            // Get solution service
            //var solutionService = Package.GetGlobalService(typeof (SVsSolution)) as IVsSolution;
            // Force-save the solution
            //if (solutionService!=null) solutionService.SaveSolutionElement((uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave, null, 0);

            File.ReadAllLines(_activeProjectName).ToList();

            string arguments;
            if (radioButton_drive.Checked)
            {
                arguments = String.Format("-v --host {0}:{1} --suite {2} --script ", hostValue.Text,
                    spin_portValue.Value, SuitePath_text.Text);
            }
            else
            {
                arguments = String.Format("-v --suite {0} --script ", SuitePath_text.Text);
            }
            var alreadyExitScripts = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    var script = (string) row.Cells[0].Value;
                    string folder = Path.Combine(Path.GetDirectoryName(_activeProjectName), "eggPlantTests");
                    Directory.CreateDirectory(folder);
                    string fileFullPath = Path.Combine(folder, script + ".GenericTest");
                    if (File.Exists(fileFullPath))
                    {
                        alreadyExitScripts.Add(script);
                        continue;
                    }

                    EggPlantTestXml.Generate(folder, script, _commandRunScript, arguments + script);
                    if (CreateProjectFile(fileFullPath) == false)
                        return;
                }
            }
            if (alreadyExitScripts.Count > 0)
            {
                MessageBox.Show(String.Format("{0} {1} {2} already in the project.",
                    alreadyExitScripts.Count == 1 ? "Script: " : "Scripts: \n",
                    String.Join(", ", alreadyExitScripts),
                    alreadyExitScripts.Count == 1 ? "is" : "are"),
                    Resources.ePF_Msgbox_Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SuitePath_text_TextChanged(object sender, EventArgs e)
        {
            button_addScript.Enabled = Directory.Exists(SuitePath_text.Text); // only enable if the local folder exists
        }

        private void ePF_TestCreation_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Save the current setting on form close
            _appSettings.Host = hostValue.Text;
            _appSettings.Port = (int) spin_portValue.Value;
            _appSettings.Suite = SuitePath_text.Text;
            _appSettings.Save();
        }

        private void radioButton_local_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void EnableControls()
        {
            label_host.Enabled =
                hostValue.Enabled = label_port.Enabled = spin_portValue.Enabled = radioButton_drive.Checked;

            button_OK.Enabled = button_removeScript.Enabled = (dataGridView1.SelectedRows.Count > 0);
        }

        private void doclink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var si = new ProcessStartInfo(Resources.DocLinkEggDrive); // http://docs.testplant.com/?q=content/eggdrive
            Process.Start(si);
            linklabel_drive.LinkVisited = true;
        }

        private void linkLabel_commandline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var si = new ProcessStartInfo(Resources.DocLinkCommandLine); // docs.testplant.com/?q=content/running-command-line
            Process.Start(si);
            linkLabel_commandline.LinkVisited = true;
        }

        private bool CreateProjectFile(string fileFullPath)
        {
            ProjectItem folderItem;
            try
            {
                folderItem = _activeProject.ProjectItems.Item("eggPlantTests");
            }
            catch (Exception)
            {
                folderItem = _activeProject.ProjectItems.AddFolder("eggPlantTests") ??
                             _activeProject.ProjectItems.Item("eggPlantTests"); //,vsProjectItemKindPhysicalFolder 
            }
            if (folderItem != null)
            {
                _activeProject.ProjectItems.AddFromFile(fileFullPath);
                _activeProject.ProjectItems.AddFromFile(fileFullPath);
            }
            return true;
        }
    }

    public static class SolutionProjects
    {
        public static DTE2 GetActiveIDE()
        {
            // Get an instance of currently running Visual Studio IDE.
            var dte2 = Package.GetGlobalService(typeof (DTE)) as DTE2;
            return dte2;
        }

        public static IList<Project> Projects()
        {
            Projects projects = GetActiveIDE().Solution.Projects;
            var list = new List<Project>();
            IEnumerator item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            var list = new List<Project>();
            for (int i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                Project subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }
            return list;
        }
    }
}