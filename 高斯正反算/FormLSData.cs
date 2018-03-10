using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 高斯正反算
{
    public partial class FormLSData : Form
    {
        SetList sl;
        public FormLSData(SetList sl)
        {
            this.sl = sl;
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string[] strX = txtX.Lines;
            try
            {
                foreach (var item in strX)
                {
                    sl(Convert.ToDouble(item), 0);
                }
            }
            catch (Exception NotDoubleE)
            {
                MessageBox.Show("非数字" + NotDoubleE.Message);
            }
            this.Close();
        }
    }
}
