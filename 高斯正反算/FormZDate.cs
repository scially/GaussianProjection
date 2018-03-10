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
    public partial class FormZDate : Form
    {
        SetList szl;
        public FormZDate(SetList sl)
        {
            InitializeComponent();
            this.szl = sl;
        }

        private void btnEntry_Click(object sender, EventArgs e)
        {
            if(txtL.Text!=""&&txtB.Text!="")
            {
                szl(Convert.ToDouble(txtB.Text), Convert.ToDouble(txtL.Text));
                this.Close();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
