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
using OpenALPRPlugin.Controls;

namespace OpenALPRPlugin.Forms
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
            this.pnlAlertList.Controls.Clear();

            if (lines == null)
            {
                lines = File.ReadAllLines(FilePath()).ToList();
                //lines.RemoveRange(0, 6);

                int nSkippedLines = 0;
                foreach (string row in lines)
                {
                    if (row.IndexOf("#") == 0)
                    {
                        ++nSkippedLines;
                    }
                    else if (row.IndexOf("Plate Number, Description") == 0)
                    {
                        ++nSkippedLines;
                    }
                    else if(string.IsNullOrWhiteSpace(row))
                    {
                        ++nSkippedLines;
                    }
                }

                if(nSkippedLines > 0)
                {
                    lines.RemoveRange(0, nSkippedLines);
                }
            }
                

            this.reoGridControl1.CurrentWorksheet.SetCols(2);
            this.reoGridControl1.CurrentWorksheet.SetColumnsWidth(1, 1, 248);
            // set text of column header
            this.reoGridControl1.CurrentWorksheet.ColumnHeaders[0].Text = "Plate";
            //this.reoGridControl1.CurrentWorksheet.ColumnHeaders[0].TextColor = Color.White;
            //this.reoGridControl1.CurrentWorksheet.ColumnHeaders[0].Style.TextColor = Color.White;
            //this.reoGridControl1.CurrentWorksheet.ColumnHeaders[0].Style.BackColor = Color.DarkGray;

            this.reoGridControl1.CurrentWorksheet.ColumnHeaders[1].Text = "Description";
            //this.reoGridControl1.CurrentWorksheet.ColumnHeaders[1].TextColor = Color.White;
            //this.reoGridControl1.CurrentWorksheet.ColumnHeaders[1].Style.TextColor = Color.White;
            //this.reoGridControl1.CurrentWorksheet.ColumnHeaders[1].Style.BackColor = Color.DarkGray;

            this.reoGridControl1.SetSettings(unvell.ReoGrid.WorkbookSettings.View_ShowSheetTabControl, false);

            int rowNum = 0;

            foreach (string row in lines)
            {           
                //var sheet = this.reoGridControl1.CurrentWorksheet;

                string sPlate = row.Split(',').FirstOrDefault();
                string sDescription = row.Split(',').LastOrDefault();

                this.reoGridControl1.CurrentWorksheet[rowNum, 0] = sPlate;
                this.reoGridControl1.CurrentWorksheet[rowNum, 1] = sDescription;        

                this.pnlAlertList.Controls.Add(new AlertRow(row.Split(',').FirstOrDefault(), row.Split(',').LastOrDefault(), rowNum)
                {
                    Top = (rowNum++ * 25) + 5,
                    Left = 5,
                    Name = "Line"
                });

            }

            List<Control> controls = this.Controls.Find("Line", true).ToList();

            foreach (Control control in controls)
            {
                Button btnDelete = (control.Controls.Find("btnDelete", true).FirstOrDefault() as Button);
                btnDelete.Click += new EventHandler(btnDelete_Click);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ((sender as Button).Parent as AlertRow).Dispose();
            lines.RemoveRange(Convert.ToInt32((sender as Button).Tag), 1);

            ParseControls();

            LoadGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ParseControls();
            this.Close();
        }

        private void ParseControls()
        {
            //List<Control> controls = this.Controls.Find("Line", true).ToList();
            string result = $"# Edit this file to add alerts.{Environment.NewLine}# Each line represents one alert and a description separated by a comma.{Environment.NewLine}# For example (Do not use a \"#\" symbol for your alerts):{Environment.NewLine}# ABC123,Walter Smith's Truck{Environment.NewLine}Plate Number, Description{Environment.NewLine}{Environment.NewLine}";

            // save the alert list here

            /*
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
            */


            for (int rowNum = 0; rowNum < this.reoGridControl1.CurrentWorksheet.MaxContentRow; ++rowNum)
            {
                if (this.reoGridControl1.CurrentWorksheet.GetCell(rowNum, 0) == null)
                    continue;

                if (this.reoGridControl1.CurrentWorksheet.GetCell(rowNum, 1) == null)
                    continue;

                if (this.reoGridControl1.CurrentWorksheet[rowNum, 0] == null)
                    continue;

                string sPlate = this.reoGridControl1.CurrentWorksheet[rowNum, 0].ToString();
                string sDescription = this.reoGridControl1.CurrentWorksheet[rowNum, 1].ToString();

                if ((!string.IsNullOrEmpty(sPlate) || !string.IsNullOrWhiteSpace(sPlate)) &&
                        (!string.IsNullOrEmpty(sDescription) || !string.IsNullOrWhiteSpace(sDescription)))
                {
                    result += $"{sPlate},{sDescription}{Environment.NewLine}";
                }
            }

            File.WriteAllText(FilePath(), result);
        }
    }
}
