using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace OpenALPRQueueConsumer
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller() : base()
        {
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call

            // Attach the 'Committed' event.
            //Committed += ProjectInstaller_Committed;
            // Attach the 'Committing' event.
            //Committing += ProjectInstaller_Committing;

            // NOTE: Setting these properties here does not make them immediately effective.  
            // The ServiceInstallerEx configures the service AFTER the base installer is done 
            // doing its job. That is when the Committed event is fired from the base installer

            // Set a description
            ServiceInstaller.Description = "Consume beanstalk queue and create Milestone bookmarks";
            ServiceProcessInstaller.Account = ServiceAccount.NetworkService;
            //ServiceProcessInstaller.Username = @"NT AUTHORITY\NetworkService";
            //ServiceProcessInstaller.Password = string.Empty;

            // The fail run command is used to spawn another process when this service fails
            // it should include the entire command line as would be passed to Win32::CreateProcess()
            //ServiceInstaller.FailRunCommand = string.Empty ;//SomeCommand.exe";

            // The fail count reset time resets the failure count after N seconds of no failures
            // on the service.  This value is set in seconds, though note that the SCM GUI only
            // displays it in increments of days.
            //ServiceInstaller.FailCountResetTime = 0;// 60 * 60 * 24 * 4;

            // The fail reboot message is used when a reboot action is specified and works in 
            // conjunction with the RecoverAction.Reboot type.

            //ServiceInstaller.FailRebootMsg = string.Empty;//Whitney Houston! We have a problem";

            // Set some failure actions : Isn't this easy??
            // Do note that if you specify less than three actions, the remaining actions will take on
            // the value of the last action.  For example, if you only set one action to RunCommand,
            // failure 2 and failure 3 will also take on the default action of RunCommand. This is 
            // a "feature" of the ChangeServiceConfig2() method; Use RecoverAction.None to disable
            // unwanted actions.

            //ServiceInstaller.FailureActions.Add(new FailureAction(RecoverAction.Restart, 60000));
            //ServiceInstaller.FailureActions.Add(new FailureAction(RecoverAction.Restart, 60000));
            //ServiceInstaller.FailureActions.Add(new FailureAction(RecoverAction.None, 3000));

            // Configure the service to start right after it is installed.  We do not want the user to
            // have to reboot their machine or to have some other process start it.  Do be careful because
            // if this service is dependent upon other services, they must be installed PRIOR to this one
            // for the service to be started properly

            //serviceInstaller2.StartOnInstall = true;
            // serviceInstallerEx.ServiceName = "";
        }


        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

    }
}