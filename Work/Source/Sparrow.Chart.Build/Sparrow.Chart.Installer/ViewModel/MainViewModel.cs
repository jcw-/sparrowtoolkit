using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Forms;
using System.Text;

namespace Sparrow.Chart.Installer
{

    public class MainViewModel : ViewModelBase
    {
        //constructor
        public MainViewModel(BootstrapperApplication bootstrapper)
        {
            
            this.IsThinking = false;
            
            this.Bootstrapper = bootstrapper;
            this.Bootstrapper.ApplyComplete += this.OnApplyComplete;
            this.Bootstrapper.DetectPackageComplete += this.OnDetectPackageComplete;
            this.Bootstrapper.PlanComplete += this.OnPlanComplete;
            this.Bootstrapper.ExecuteMsiMessage += OnExecuteMessage;            
        }        

        #region Properties

        private bool installEnabled;
        public bool InstallEnabled
        {
            get { return installEnabled; }
            set
            {
                installEnabled = value;
                RaisePropertyChanged("InstallEnabled");
            }
        }

        private bool uninstallEnabled;
        public bool UninstallEnabled
        {
            get { return uninstallEnabled; }
            set
            {
                uninstallEnabled = value;
                RaisePropertyChanged("UninstallEnabled");
            }
        }

        private bool isThinking;
        public bool IsThinking
        {
            get { return isThinking; }
            set
            {
                isThinking = value;
                RaisePropertyChanged("IsThinking");
            }
        }

       
        private bool isLicensePage;
        public bool IsLicensePage
        {
            get { return isLicensePage; }
            set { isLicensePage = value; RaisePropertyChanged("IsLicensePage"); }
        }

        private bool isUnInstallPage;
        public bool IsUnInstallPage
        {
            get { return isUnInstallPage; }
            set { isUnInstallPage = value; RaisePropertyChanged("IsUnInstallPage"); }
        }
       
        private bool isInstallPage;
        public bool IsInstallPage
        {
            get { return isInstallPage; }
            set { isInstallPage = value; RaisePropertyChanged("IsInstallPage"); }
        }

        private bool isProgressPage;
        public bool IsProgressPage
        {
            get { return isProgressPage; }
            set { isProgressPage = value; RaisePropertyChanged("IsProgressPage"); }
        }

        private bool isUnInstallProgressPage;
        public bool IsUnInstallProgressPage
        {
            get { return isUnInstallProgressPage; }
            set { isUnInstallProgressPage = value; RaisePropertyChanged("IsUnInstallProgressPage"); }
        }

        private bool isFinsihPage;
        public bool IsFinsihPage
        {
            get { return isFinsihPage; }
            set { isFinsihPage = value; RaisePropertyChanged("IsFinsihPage"); }
        }

        private bool isUnInstallFinsihPage;
        public bool IsUnInstallFinsihPage
        {
            get { return isUnInstallFinsihPage; }
            set { isUnInstallFinsihPage = value; RaisePropertyChanged("IsUnInstallFinsihPage"); }
        }

        private bool isAgree;
        public bool IsAgree 
        {
            get { return isAgree; }
            set { isAgree = value; RaisePropertyChanged("IsAgree"); }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; RaisePropertyChanged("Status"); }
        }

        private string installLocation;
        public string InstallLocation
        {
            get { return installLocation; }
            set { installLocation = value; RaisePropertyChanged("InstallLocation"); }
        }

        public BootstrapperApplication Bootstrapper { get; private set; }

        #endregion //Properties

        #region Methods

        void OnInstallComplete(object sender,ExecuteCompleteEventArgs e)
        {
            IsThinking = false;
            //IsLicensePage = false;
            //IsInstallPage = false;
            IsProgressPage = false;
            IsFinsihPage = true;  
        }

        void OnUnInstallComplete(object sender, ExecuteCompleteEventArgs e)
        {
            IsThinking = false;            
            IsUnInstallProgressPage = false;
            IsUnInstallFinsihPage = true;
        }

        void OnExecuteMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            string eMessage = string.Empty;
            switch (e.MessageType)
            {
                case InstallMessage.ActionStart:
                    ActionStartMessage message = (ActionStartMessage)ParseMessage(e.Message, e.MessageType);
                    ActionStartEventArgs eventArgs = new ActionStartEventArgs();
                    eventArgs.DateRun = message.DateRun;
                    eventArgs.Message = message.Message;
                    eventArgs.Name = message.Name;
                    eMessage = message.Name + ":" + message.Message;
                    break;
                default:
                    eMessage=e.Message;
                    break;
            }
            this.Status = eMessage;
        }

        private object ParseMessage(string Message, InstallMessage MessageType)
        {
            switch (MessageType)
            {
                case InstallMessage.ActionStart:
                    ActionStartMessage message = new ActionStartMessage();
                    String[] items = Message.Trim().Split(new char[] { ' ' });

                    string dateRun = items[1];
                    string actionName = items[2];

                    if (dateRun.EndsWith(":"))
                    {
                        dateRun = dateRun.Trim().TrimEnd(new char[] { ':' });
                    }

                    if (actionName.EndsWith("."))
                    {
                        actionName = actionName.Trim().TrimEnd(new char[] { '.' });
                    }

                    StringBuilder messageInfo = new StringBuilder();
                    if (items.Length > 3)
                    {

                        for (int i = 3; i < items.Length; i++)
                        {
                            messageInfo.Append(items[i] + " ");
                        }
                    }

                    DateTime.TryParse(dateRun, out message.DateRun);
                    message.Name = actionName.Trim();
                    message.Message = messageInfo.ToString().Trim();

                    return message;

                    break;
            }

            return null;
        }
  

        private void BrowseExecute()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Please select the Installation folder for Sparrow Chart Toolkit";
            //dialog.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                InstallLocation = dialog.SelectedPath;
            }
                    
        }

        private void PreviousExecute()
        {
            IsLicensePage = true;
            IsInstallPage = false;
            IsProgressPage = false;
            IsFinsihPage = false;            
        }

        private void NextExecute()
        {            
            IsLicensePage = false;
            IsInstallPage = true;
            IsFinsihPage = false;
            IsProgressPage = false;
            //InstallLocation = Bootstrapper.Engine.StringVariables["INSTALLDIR"];
            InstallLocation = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        }

        private void InstallExecute()
        {
            IsInstallPage = false;
            IsProgressPage = true;  
            IsThinking = true;
            Bootstrapper.ExecuteComplete += OnInstallComplete;
            Bootstrapper.Engine.StringVariables["INSTALLDIR"] = InstallLocation;
            Bootstrapper.Engine.Plan(LaunchAction.Install);            

        }

        private void UninstallExecute()
        {
            IsThinking = true;
            IsUnInstallPage = false;
            IsUnInstallProgressPage = true;
            Bootstrapper.Engine.Plan(LaunchAction.Uninstall);
            Bootstrapper.ExecuteComplete += OnUnInstallComplete;
        }

        private void ExitExecute()
        {
            SparrowInstaller.BootstrapperDispatcher.InvokeShutdown();            
        }

        private void AgreeExecute()
        {
            if (IsAgree)
                IsAgree = false;
            else
                IsAgree = true;
        }

        /// <summary>
        /// Method that gets invoked when the Bootstrapper ApplyComplete event is fired.
        /// This is called after a bundle installation has completed. Make sure we updated the view.
        /// </summary>
        private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            IsThinking = false;
            InstallEnabled = false;
            UninstallEnabled = false;
        }

        /// <summary>
        /// Method that gets invoked when the Bootstrapper DetectPackageComplete event is fired.
        /// Checks the PackageId and sets the installation scenario. The PackageId is the ID
        /// specified in one of the package elements (msipackage, exepackage, msppackage,
        /// msupackage) in the WiX bundle.
        /// </summary>
        private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            if (e.PackageId == "SparrowChartPackageId")
            {
                if (e.State == PackageState.Absent)
                {
                    InstallEnabled = true;
                    this.IsLicensePage = true;
                }

                else if (e.State == PackageState.Present)
                {
                    UninstallEnabled = true;
                    this.IsUnInstallPage = true;
                }
            }
        }

        /// <summary>
        /// Method that gets invoked when the Bootstrapper PlanComplete event is fired.
        /// If the planning was successful, it instructs the Bootstrapper Engine to 
        /// install the packages.
        /// </summary>
        private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (e.Status >= 0)
                Bootstrapper.Engine.Apply(System.IntPtr.Zero);            
           
        }

        #endregion //Methods

        #region RelayCommands

        private RelayCommand browseCommand;
        public RelayCommand BrowseCommand
        {
            get
            {
                if (browseCommand == null)
                    browseCommand = new RelayCommand(() => BrowseExecute());

                return browseCommand;
            }
        }

        private RelayCommand previousCommand;
        public RelayCommand PreviousCommand
        {
            get
            {
                if (previousCommand == null)
                    previousCommand = new RelayCommand(() => PreviousExecute());

                return previousCommand;
            }
        }

        private RelayCommand nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                    nextCommand = new RelayCommand(() => NextExecute());

                return nextCommand;
            }
        }

        private RelayCommand installCommand;
        public RelayCommand InstallCommand
        {
            get
            {
                if (installCommand == null)
                    installCommand = new RelayCommand(() => InstallExecute(), () => InstallEnabled == true);

                return installCommand;
            }
        }

        private RelayCommand uninstallCommand;
        public RelayCommand UninstallCommand
        {
            get
            {
                if (uninstallCommand == null)
                    uninstallCommand = new RelayCommand(() => UninstallExecute(), () => UninstallEnabled == true);

                return uninstallCommand;
            }
        }

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new RelayCommand(() => ExitExecute());

                return exitCommand;
            }
        }

        private RelayCommand agreeCommand;
        public RelayCommand AgreeCommand
        {
            get
            {
                if (agreeCommand == null)
                    agreeCommand = new RelayCommand(() => AgreeExecute());

                return agreeCommand;
            }
        }
        
        #endregion //RelayCommands
    }

   

  public class ActionStartMessage 
  { 
    public string Name; 
    public DateTime DateRun; 
    public string Message; 
  } 

  public class ActionStartEventArgs : EventArgs 
  { 
    private string fName; 
    private DateTime fDateRun; 
    private string fMessage; 
    private Microsoft.Deployment.WindowsInstaller.Session fSession; 

    public string Name 
    { 
      get { return fName; } 
      set { fName = value; } 
    } 

    public DateTime DateRun 
    { 
      get { return fDateRun; } 
      set { fDateRun = value; } 
    } 

    public string Message 
    { 
      get { return fMessage; } 
      set { fMessage = value; } 
    } 

    public Microsoft.Deployment.WindowsInstaller.Session Session 
    { 
      get { return fSession; } 
      set { fSession = value; } 
    } 
  }
}