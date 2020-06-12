using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTrayApp.Controls;

namespace SystemTrayApp
{
    public partial class AlertList : Form
    {
        internal List<string> lines = null;
        public AlertList()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void LoadGrid()
        {
            if (lines == null)
            {
                lines = File.ReadAllLines(FilePath()).ToList();
                lines.RemoveRange(0, 6);
            }

            int rowNum = 0;

            foreach (string row in lines)
            {
                this.pnlAlertList.Controls.Add(new AlertRow(row.Split(',').FirstOrDefault(), row.Split(',').LastOrDefault())
                {
                    Top = (rowNum++ * 25) + 5,
                    Left = 5,
                    Name = "Line"
                });
            }
        }

        private string FilePath()
        {
            const string PlugName = "OpenALPR";
            string mappingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PlugName, "Mapping");            
            return Path.Combine(mappingPath, "AlertList.txt");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.pnlAlertList.Controls.Clear();
            lines.Add("");
            LoadGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<Control> controls = this.Controls.Find("Line", true).ToList();
            string result = $"# Edit this file to add alerts.{Environment.NewLine}# Each line represents one alert and a description separated by a comma.{Environment.NewLine}# For example (Do not use a \"#\" symbol for your alerts):{Environment.NewLine}# ABC123,Walter Smith's Truck{Environment.NewLine}Plate Number, Description{Environment.NewLine}{Environment.NewLine}";

            foreach (Control control in controls)
            {
                TextBox txtPlate = (control.Controls.Find("txtPlate", true).FirstOrDefault() as TextBox);
                TextBox txtName = (control.Controls.Find("txtName", true).FirstOrDefault() as TextBox);

                if ((!string.IsNullOrEmpty(txtPlate.Text) || !string.IsNullOrWhiteSpace(txtPlate.Text)) &&
                    (!string.IsNullOrEmpty(txtName.Text) || !string.IsNullOrWhiteSpace(txtName.Text)))
                {
                    result += $"{txtPlate.Text},{txtName.Text}{Environment.NewLine}";
                }
            }

            File.WriteAllText(FilePath(), result);
            this.Close();
        }
    }
}
