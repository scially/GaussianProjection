using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace 高斯正反算
{
    public class Data
    {
        //数据文件，格式为num1,num2
        //              num1,num2
        //              ... ,...
        public static void getData2(string FileName, MyList<double> mylist)
        {
            using (StreamReader sr = new StreamReader(FileName, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    //读取到的每一行不能为空
                    if (str == "")
                    {
                        continue;
                    }
                    string[] content = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    mylist.Add(Convert.ToDouble(content[0]), Convert.ToDouble(content[1]));
                }
            }
        }

        //创建打开数据文件对话框
        //读取数据将其添加到ListView中
        public static void AddDataList(ListView lv)
        {
            MyList<double> mylist = new MyList<double>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "打开数据";
            ofd.Filter = "文本文件(*.txt)|*.txt|数据文件(*.dat)|*.dat";
            if (DialogResult.OK == ofd.ShowDialog())
            {
                Data.getData2(ofd.FileName, mylist);
                lv.BeginUpdate();
                for (int i = 0; i < mylist.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = mylist.ListX[i].ToString();
                    lvi.SubItems.Add(mylist.ListY[i].ToString());
                    lv.Items.Add(lvi);
                }
                lv.EndUpdate();
            }
        }

        //创建保存数据文件对话框
        //将ListView数据保存到文件中
        public static void ListViewtoFile(ListView lv)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存数据";
            sfd.Filter = "文本文件(*.txt)|*.txt|数据文件(*.dat)|*.dat";

            if (DialogResult.OK == sfd.ShowDialog())
            {
                using(StreamWriter sw=new StreamWriter(sfd.FileName))
                {
                    for (int i = 0; i < lv.Items.Count; i++)
                    {
                        sw.Write(lv.Items[i].SubItems[0].Text);
                        sw.Write(",");
                        sw.WriteLine(lv.Items[i].SubItems[1].Text);
                    }
                } 
            }
        }

        //将不定个list集合按照数组顺序添加到ListView中
        public static void AddDataList(ListView lv,params List<string>[] list)
        {
            lv.Items.Clear();
            for (int i = 0; i <list[0].Count; i++)
            {
                ListViewItem lvi = new ListViewItem(list[0][i]);
                for (int j = 1; j < list.Length; j++)
                {
                    lvi.SubItems.Add(list[j][i]);
                }
                lv.Items.Add(lvi);
            }
        }

        //从ListView中读取所有数据
        public static void FromListView(ListView lv,  params List<string>[] list)
        {
            for(int i=0;i<list.Length;i++)
            {
                for(int j=0;j<lv.Items.Count;j++)
                {
                    list[i].Add(lv.Items[j].SubItems[i].Text);
                }
            }
        }

        //删除ListView选中行
        public static void DeleteListSelected(ListView lv)
        {
            foreach (ListViewItem lvi in lv.SelectedItems)
            {
                lv.Items.Remove(lvi);
            }
        }
    }
}
