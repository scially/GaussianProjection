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
    public partial class FormSDate : Form
    {
        SetList sl;
        public FormSDate(SetList sl)
        {
            this.sl = sl;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnEntry_Click(object sender, EventArgs e)
        {
            try
            {
                sl(Convert.ToDouble(txtX.Text), Convert.ToDouble(txtY.Text));
                this.Close();
            }catch(Exception eNull)
            {
                throw eNull.InnerException;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
