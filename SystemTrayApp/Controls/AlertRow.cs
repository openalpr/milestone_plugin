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
        public AlertRow(string plate, string name, int row)
        {
            InitializeComponent();
            txtName.Text = name;
            txtPlate.Text = plate;
            btnDelete.Tag = row;
        }

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    this.Dispose();
        //}
    }
}
