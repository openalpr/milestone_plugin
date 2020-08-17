using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenALPRPlugin.Client;
namespace test_gui2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OpenALPRViewItemManager view_manager = new OpenALPRViewItemManager();
            WorkSpaceControl wcs = new WorkSpaceControl(view_manager);
            this.Controls.Add(wcs);

        }
    }
}
