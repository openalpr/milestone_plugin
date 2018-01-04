using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.Client;

namespace OpenALPRPlugin.Client
{
    public class OpenALPRWorkSpacePlugin : WorkSpacePlugin
	{
        public static Guid SCWorkSpacePluginId = new Guid("29021339-67EE-4161-ACFD-96E50D02458D");

        public override Guid Id
        {
            get { return SCWorkSpacePluginId; }
        }

        public override string Name
        {
            get { return OpenALPRPluginDefinition.PlugName; }
        }

        public override bool IsSetupStateSupported
        {
            get { return false; }
        }

        public override void Init()
        {
            //int rec1_3Width = 249;
            //var rect = new Rectangle[3];

            //rect[0] = new Rectangle(000, 000, rec1_3Width, 999);
            //rect[1] = new Rectangle(rec1_3Width, 000, 999 - 2 * rec1_3Width, 999);
            //rect[2] = new Rectangle(999 - rec1_3Width, 000, rec1_3Width, 999);

            //ViewAndLayoutItem.Layout = rect; //980, 845
            ViewAndLayoutItem.Layout = new Rectangle[] { new Rectangle(000, 000, 2000, 2000) };
            
            ViewAndLayoutItem.Name = Name;
            ViewAndLayoutItem.InsertViewItemPlugin(0, new OpenALPRViewItemPlugin(), new Dictionary<string, string>());
        }

        public override void Close()
        {
        }


    }
}
