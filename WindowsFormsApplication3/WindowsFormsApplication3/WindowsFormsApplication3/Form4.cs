using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        int i;
        private void Form4_Load(object sender, EventArgs e)
        {
            while (i < 8)
            {
                this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

                ListViewItem lvi = new ListViewItem();

                lvi.ImageIndex = i;     //通过与imageList绑定，显示imageList中第i项图标 
                lvi.Text = Form1.Setup_SW_Name[i];
                lvi.SubItems.Add(Form1.Setup_SW_Path[i]);

                //lvi.SubItems.Add("第3列,第" + i + "行");
                this.listView1.Items.Add(lvi);
                //}
                this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                //this.listView1.Refresh();
                i++;
            }
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
