// Copyright OpenALPR Technology, Inc. 2018

using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace OpenALPRPlugin.Client
{
    /// <summary>
    /// The ViewItemManager contains the configuration for the ViewItem. <br/>
    /// When the class is initiated it will automatically recreate relevant ViewItem configuration saved in the properties collection from earlier.
    /// Also, when the viewlayout is saved the ViewItemManager will supply current configuration to the SmartClient to be saved on the server.<br/>
    /// This class is only relevant when executing in the Smart Client.
    /// </summary>
    public class OpenALPRViewItemManager : ViewItemManager
    {
        private Guid _someid;
        private const string selectedGuid = "SelectedGUID";
        public OpenALPRViewItemManager()
            : base(nameof(OpenALPRViewItemManager))
        {
        }

        #region Methods overridden
        /// <summary>
        /// The properties for this ViewItem is now loaded into the base class and can be accessed via 
        /// GetProperty(key) and SetProperty(key,value) methods
        /// </summary>
        public override void PropertiesLoaded()
        {
            string someid = GetProperty(selectedGuid);
            ConfigItems = Configuration.Instance.GetItemConfigurations(OpenALPRPluginDefinition.OpenALPRPluginId, null, OpenALPRPluginDefinition.ExportPluginKind);

            if (someid != null && ConfigItems != null)
                SomeId = new Guid(someid);	// Set as last selected
        }

        /// <summary>
        /// Generate the UserControl containing the Actual ViewItem Content
        /// </summary>
        /// <returns></returns>
        public override ViewItemUserControl GenerateViewItemUserControl()
        {
            //return new SCThemeViewItemUserControl();//Khayralla
            //return new BookmarkInfoControl(false, new List<string>(), new BookmarkViewItemManager());
            return new WorkSpaceControl(new OpenALPRViewItemManager());
        }

        /// <summary>
        /// Generate the UserControl containing the property configuration.
        /// </summary>
        /// <returns></returns>
        //public override PropertiesUserControl GeneratePropertiesUserControl()
        //{
        //    return new BookmarkSamplePropertiesUserControl(this);
        //}

        #endregion

        public List<Item> ConfigItems { get; private set; }

        public Guid SomeId
        {
            get { return _someid; }
            set
            {
                _someid = value;
                SetProperty(selectedGuid, _someid.ToString());
                if (ConfigItems != null)
                {
                    foreach (Item item in ConfigItems)
                    {
                        if (item.FQID.ObjectId == _someid)
                            SomeName = item.Name;
                    }
                }

                SaveProperties();
            }
        }

        public string SomeName { get; set; }
    }
}
