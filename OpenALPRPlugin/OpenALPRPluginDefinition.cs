// Copyright OpenALPR Technology, Inc. 2018

using OpenALPRPlugin.Background;
using OpenALPRPlugin.Client;
using OpenALPRPlugin.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

namespace OpenALPRPlugin
{
    public class OpenALPRPluginDefinition :  PluginDefinition
    {
        internal static Guid SampleTopNode = new Guid("01353203-1FCB-47D6-9AEF-A1910B48B646");
        internal static Guid OpenALPRPluginId = new Guid("B82B74FE-1FF6-4050-B75B-5F6E84273904");
        internal static Guid ExportPluginKind = new Guid("FDCBB0CC-28A5-45C8-B6B9-5B54045B345C");
        internal static Guid OpenALPRViewItemPlugin = new Guid("0991A3DC-2A05-40D0-9ACC-F4ABF41931C6");
        internal static Guid BackgroundPlugin = new Guid("8DC4B6F7-BB30-4974-A196-E754E7C24B14");
        internal static string PlugName = "OpenALPR";
        internal static string ProductName = "OpenALPR Plug-in";
        internal static bool IsHostedbySmartClient = true;
        internal static string OpenALPRPluginVersionString = "1.0.0.0";
        internal static float Version = 0;
        internal static FileVersionInfo fileVersion;

        #region Private fields

        private static Image _treeNodeImage;
        private static Image _topTreeNodeImage;
        private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();
        private List<OptionsDialogPlugin> _optionsDialogPlugins = new List<OptionsDialogPlugin>();
        private List<String> _messageIdStrings = new List<string>();
        private List<SecurityAction> _securityActions = new List<SecurityAction>();
        private List<WorkSpacePlugin> _workSpacePlugins = new List<WorkSpacePlugin>();
        private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();
        private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static OpenALPRPluginDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            Logger.Initialize(name);

#if DEBUG
            assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
                fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
#else
            
            string path = @"C:\Program Files\VideoOS\MIPPlugins\OpenALPR\OpenALPRPlugin.dll";

            fileVersion = File.Exists(path) ?
                FileVersionInfo.GetVersionInfo(path) :
                FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName);
#endif

            Stream pluginStream = assembly.GetManifestResourceStream($"{name}.Resources.logo_bluegray.png");//OpenALPRPlugin
            if (pluginStream != null)
                _treeNodeImage = Image.FromStream(pluginStream);

            Stream configStream = assembly.GetManifestResourceStream($"{name}.Server.png");
            if (configStream != null)
                _topTreeNodeImage = Image.FromStream(configStream);
        }

        /// <summary>
        /// Get the icon for the plug-in
        /// </summary>
        internal static Image TreeNodeImage
        {
            get { return _treeNodeImage; }
        }

        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _workSpacePlugins.Add(new OpenALPRWorkSpacePlugin());
                _backgroundPlugins.Add(new OpenALPRBackgroundPlugin());
                _viewItemPlugin.Add(new OpenALPRViewItemPlugin());
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _workSpacePlugins.Clear();
            _sidePanelPlugins.Clear();
            _backgroundPlugins.Clear();
            _viewItemPlugin.Clear();
            Logger.Close();
        }

        /// <summary>
        /// Return any new messages that this plug in can use in SendMessage or PostMessage,
        /// or has a Receiver set up to listen for.
        /// The suggested format is: "YourCompany.Area.MessageId"
        /// </summary>
        public override List<string> PluginDefinedMessageIds
        {
            get { return _messageIdStrings; }
        }

        /// <summary>
        /// If authorization is to be used, add the SecurityActions the entire plug in 
        /// would like to be available.  E.g. Application level authorization.
        /// </summary>
        public override List<SecurityAction> SecurityActions
        {
            get { return _securityActions; }
            set { }
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plug-in component
        /// </summary>
        public override Guid Id
        {
            get { return OpenALPRPluginId; }
        }

        /// <summary>
        /// This Guid can be defined on several different IPluginDefinitions with the same value,
        /// and will result in a combination of this top level ProductNode for several plug-ins.
        /// Set to Guid.Empty if no sharing is enabled.
        /// </summary>
        public override Guid SharedNodeId
        {
            get { return SampleTopNode; }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return PlugName; }
        }

        /// <summary>
        /// Your company name
        /// </summary>
        public override string Manufacturer
        {
            get { return "Milestone Systems A / S"; }
        }

        /// <summary>
        /// Version of this plug-in.
        /// </summary>
        public override string VersionString
        {
            get { return OpenALPRPluginVersionString; }
        }

        internal static string ExtractVersionString
        {
            get { return $"{fileVersion.FileMajorPart.ToString()}.{fileVersion.FileMinorPart.ToString()}.{fileVersion.FileBuildPart.ToString()}.{fileVersion.FilePrivatePart.ToString()}"; }
        }

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override Image Icon
        {
            get { return Util.ImageList.Images[Util.SDK_VSIx]; }
        }

        #endregion

        #region Administration properties

        /// <summary>
        /// A list of server side configuration items in the administrator
        /// </summary>
        public override List<ItemNode> ItemNodes
        {
            //get { return _itemNodes; }
            get { return new List<ItemNode>(); }
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the top TreeNode
        /// </summary>
        public override UserControl GenerateUserControl()
        {
            //return new UserControl();
            return null;
        }

        /// <summary>
        /// This property can be set to true, to be able to display your own help UserControl on the entire panel.
        /// When this is false - a standard top and left side is added by the system.
        /// </summary>
        public override bool UserControlFillEntirePanel
        {
            get { return false; }
        }

        #endregion

        #region Client related methods and properties

        public override List<WorkSpacePlugin> WorkSpacePlugins
        {
            get { return _workSpacePlugins; }
        }

        /// <summary>
        /// A list of Client side definitions for Smart Client
        /// </summary>
        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _viewItemPlugin; }
        }

        /// <summary>
        /// An extension plug-in running in the Smart Client to add more choices on the Options dialog.
        /// </summary>
        public override List<OptionsDialogPlugin> OptionsDialogPlugins
        {
            get { return _optionsDialogPlugins; }
        }

        /// <summary> 
        /// An extention plugin to add to the side panel of the Smart Client.
        /// </summary>
        public override List<SidePanelPlugin> SidePanelPlugins
        {
            get { return _sidePanelPlugins; }
        }

        /// <summary>
        /// Create and returns the background task.
        /// </summary>
        public override List<BackgroundPlugin> BackgroundPlugins
        {
            get { return _backgroundPlugins; }
        }

        #endregion
    }
}
