using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathMethod;
using Geodesy;

namespace 高斯正反算
{
    //委托，将新的窗体中的值添加到ListView
    public delegate void SetList(double x, double y = 0);
    public partial class FormMain : Form
    {
        #region 窗体初始化
        public FormMain()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //改变窗体名称
            this.Text = "子午线弧长正算";
            //增加常见椭球参数类型
            //设置可选椭球选项,并且默认选项为克洛索夫斯基椭球
            string[] OvalType = { "克洛索夫斯基椭球", "1975国际椭球", "WGS84椭球", "自定义椭球" };
            this.cboxOval.Items.AddRange(OvalType);
            this.cboxOval.SelectedIndex = 0;
            txtA.Text = "6378245";
            txtB.Text = "298.3";
            txtA.Enabled = false;
            txtB.Enabled = false;

            string[] ZMath = { "经典算法", "复化梯形算法", "复化辛普森算法", "龙贝格算法" };
            cboxZMethod.Items.AddRange(ZMath);
            cboxZMethod.SelectedIndex = 0;

            string[] SMath = { "经典算法", "单点迭代法", "牛顿迭代法", "割线法" };
            cboxSMethod.Items.AddRange(SMath);
            cboxSMethod.SelectedIndex = 0;
        }

        //设置预选椭球参数
        private void cboxOval_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据不同的选项，显示不同的a，b值
            switch (this.cboxOval.SelectedIndex)
            {
                case 0:
                    txtA.Text = "6378245";
                    txtB.Text = "298.3";
                    txtA.Enabled = false;
                    txtB.Enabled = false;
                    break;
                case 1:
                    txtA.Text = "6378140";
                    txtB.Text = "298.257";
                    txtA.Enabled = false;
                    txtB.Enabled = false;
                    break;
                case 2:
                    txtA.Text = "6378137";
                    txtB.Text = "298.257223563";
                    txtA.Enabled = false;
                    txtB.Enabled = false;
                    break;
                case 3:
                    txtA.Enabled = true;
                    txtB.Enabled = true;
                    txtA.Clear();
                    txtB.Clear();
                    txtA.Focus();
                    break;
            }
        }

        #endregion

        #region Tab标签初始化
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (this.GaussType.SelectedIndex == 0)
            {
                this.Text = "子午线弧长正算";
            }
            else if (this.GaussType.SelectedIndex == 1)
            {
                this.Text = "子午线弧长反算";
            }
            else if (this.GaussType.SelectedIndex == 2)
            {
                this.Text = "高斯正算";
            }
            else if (this.GaussType.SelectedIndex == 3)
            {
                this.Text = "高斯反算";
            }

        }

        #endregion

        #region 子午线正算

        private void btnLZAdd_Click(object sender, EventArgs e)
        {
            FormLZData flzd = new FormLZData((x, y) =>
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = x.ToString("0.0000");
                this.listLZ.Items.Add(lvi);
            });
            flzd.Show();
        }
        private void btnLZDelete_Click(object sender, EventArgs e)
        {
            Data.DeleteListSelected(this.listLZ);
        }

        //计算子午线弧长
        private void btnLZCal_Click(object sender, EventArgs e)
        {
            //获取所选椭球参数值
            double a = Convert.ToDouble(txtA.Text);
            //由于double精度有限，所以选择通过扁率的倒数β来计算e的平方
            double e2 = (2 * Convert.ToDouble(txtB.Text) - 1) /
                        Convert.ToDouble(txtB.Text) / Convert.ToDouble(txtB.Text);

            //获取步长或者精度
            double n = 0;
            if (txtZE.Enabled)
            {
                n = Convert.ToDouble(txtZE.Text);
            }

            //存储从ListView中获取到的“纬度B”
            List<string> list = new List<string>();
            Data.FromListView(this.listLZ, list);
            List<string> listStr = new List<string>();
            foreach (var item in list)
            {
                listStr.Add((Convert.ToDouble(item)).ToString("0.0000"));
            }

            //将list中的“纬度”转换为弧度制
            //根据所选算法计算弧长，将结果存放在listRes集合中。
            List<string> listRes = new List<string>();
            ArcLength arc = new ArcLength(a, e2);

            //经典算法结果
            List<string> list_c = new List<string>();
            foreach (var item in list)
            {
                list_c.Add(arc.X_standard(DmsRad.dms2rad(item)).ToString("0.000"));
            }

            //根据数值积分计算弧长
            switch (this.cboxZMethod.SelectedIndex)
            {
                case 1:  //复化梯形算法
                    listRes.Clear();
                    foreach (var item in list)
                    {
                        double B_rad = DmsRad.dms2rad(item.ToString());
                        listRes.Add(arc.X_ft((int)(B_rad / (n / DmsRad.p)), B_rad).ToString("0.000"));
                    }
                    break;
                case 2:  //复化辛普森算法
                    listRes.Clear();
                    foreach (var item in list)
                    {
                        double B_rad = DmsRad.dms2rad(item.ToString());
                        listRes.Add(arc.X_fs((int)(B_rad / (n / DmsRad.p)), B_rad).ToString("0.000"));
                    }
                    break;
                case 3:  //龙贝格算法
                    listRes.Clear();
                    foreach (var item in list)
                    {
                        double B_rad = DmsRad.dms2rad(item.ToString());
                        listRes.Add(arc.X_r(n, B_rad).ToString("0.000"));
                    }
                    break;
                default: break;
            }

            //在ListView中显示与结果
            if (cboxZMethod.SelectedIndex == 0)
            {
                Data.AddDataList(this.listLZ, listStr, list_c);
            }
            else
            {
                List<string> listDif = XZ_dif(listRes, list_c);
                Data.AddDataList(this.listLZ, listStr, listRes, listDif);
            }

        }

        private void cboxZMethod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cboxZMethod.SelectedIndex)
            {
                case 0:
                    txtZE.Enabled = false;
                    break;
                case 1:
                case 2:
                case 3:
                    txtZE.Enabled = true;
                    break;
            }
        }

        #endregion

        #region 子午线反算

        private void cboxSMethod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (cboxSMethod.SelectedIndex)
            {
                case 0:
                    txtSE.Enabled = false;
                    break;
                case 1:
                case 2:
                case 3:
                    txtSE.Enabled = true;
                    break;
            }
        }

        private void btnLSAdd_Click(object sender, EventArgs e)
        {
            FormLSData fls = new FormLSData((x, y) =>
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = x.ToString("0.000");
                this.listLS.Items.Add(lvi);
            });
            fls.Show();
        }

        private void btnLSDelete_Click(object sender, EventArgs e)
        {
            Data.DeleteListSelected(this.listLS);
        }

        private void btnSCal_Click(object sender, EventArgs e)
        {
            //获取所选椭球参数值
            double a = Convert.ToDouble(txtA.Text);
            //由于double精度有限，所以选择通过扁率的倒数β来计算e的平方
            double e2 = (2 * Convert.ToDouble(txtB.Text) - 1) /
                        Convert.ToDouble(txtB.Text) / Convert.ToDouble(txtB.Text);

            int n = 0;   //迭代次数
            if (txtSE.Enabled)
            {
                n = Convert.ToInt32(txtSE.Text);
            }

            //存储从ListView中获取到弧长
            List<string> list = new List<string>();
            Data.FromListView(this.listLS, list);

            ArcLength arc = new ArcLength(a, e2);
            List<string> listRes = new List<string>();
            //经典迭代算法结果
            List<string> list_c = new List<string>();

            foreach (var item in list)
            {
                double rad = arc.B_standard(Convert.ToDouble(item), 1e-10);
                list_c.Add(Convert.ToDouble(DmsRad.rad2dms(rad)).ToString("0.0000"));
            }

            //根据数值积分计算弧长
            switch (this.cboxSMethod.SelectedIndex)
            {
                case 1:  //单点迭代法
                    listRes.Clear();
                    foreach (var item in list)
                    {
                        double rad = arc.B_single(Convert.ToDouble(item), n);
                        listRes.Add(Convert.ToDouble(DmsRad.rad2dms(rad)).ToString("0.0000"));
                    }
                    break;
                case 2:  //牛顿迭代法
                    listRes.Clear();
                    foreach (var item in list)
                    {
                        double rad = arc.B_newton(Convert.ToDouble(item), n);
                        listRes.Add(Convert.ToDouble(DmsRad.rad2dms(rad)).ToString("0.0000"));
                    }
                    break;
                case 3: //割线法
                    listRes.Clear();
                    foreach (var item in list)
                    {
                        double rad = arc.B_sec(Convert.ToDouble(item), n);
                        listRes.Add(Convert.ToDouble(DmsRad.rad2dms(rad)).ToString("0.0000"));
                    }
                    break;
                default: break;
            }

            //在ListView中显示与结果
            if (cboxSMethod.SelectedIndex == 0)
            {
                Data.AddDataList(this.listLS, list, list_c);
            }
            else
            {
                List<string> listDif = XZ_dif(listRes, list_c);
                Data.AddDataList(this.listLS, list, listRes, listDif);
            }

        }

        #endregion

        #region 高斯正算
        //将新的窗体中的值添加到listveiw中，重写FormZdate的构造函数，传递委托参数
        private void btnZAdd_Click(object sender, EventArgs e)
        {
            //将新弹出的窗体中的BL值保存到LIstVIew中
            FormZDate fzd = new FormZDate((x, y) =>
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = x.ToString("0.0000");
                lvi.SubItems.Add(y.ToString("0.0000"));
                this.listZSource.Items.Add(lvi);
            });
            fzd.Show();
        }

        //两个LIstView控件的删除功能
        private void btnZDelet_Click(object sender, EventArgs e)
        {
            Data.DeleteListSelected(this.listZSource);
        }

        //打开数据文件
        private void btnZOpen_Click(object sender, EventArgs e)
        {
            Data.AddDataList(this.listZSource);
        }

        #endregion

        #region 高斯反算

        private void btnSDelete_Click(object sender, EventArgs e)
        {
            Data.DeleteListSelected(this.listSSource);
        }

        private void btnSOpen_Click(object sender, EventArgs e)
        {
            Data.AddDataList(this.listSSource);
        }

        private void btnSAdd_Click(object sender, EventArgs e)
        {
            FormSDate fsd = new FormSDate(delegate(double x, double y)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = x.ToString("0.000");
                lvi.SubItems.Add(y.ToString("0.000"));
                this.listSSource.Items.Add(lvi);
            });
            fsd.Show();
        }

        //高斯批量正算
        private void btnZConvert_Click(object sender, EventArgs e)
        {

        }
        #endregion

        //计算数值积分与经典算法得出的结果的差值的绝对值
        private List<string> XZ_dif(List<string> listRes, List<string> list_c)
        {
            List<string> listDiff = new List<string>();
            for (int i = 0; i < list_c.Count; i++)
            {
                double dif = Math.Abs(Convert.ToDouble(listRes[i]) - Convert.ToDouble(list_c[i]));
                listDiff.Add(dif.ToString("0.0000"));
            }
            return listDiff;
        }

        




    }
}
