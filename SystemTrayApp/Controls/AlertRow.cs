using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemTrayApp.Controls
{
    public partial class AlertRow : UserControl
    {
        public AlertRow(string plate, string name)
        {
            InitializeComponent();
            txtName.Text = name;
            txtPlate.Text = plate;
        }
    }
}
