using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App1
{

    public partial class 计算器 : Form
    {
        public 计算器()
        {
            InitializeComponent();
        }

        int tab = 0;
        //记录指针
        int Precord = 0;
        //记录数
        int RecordNum = 0;
        public String text = "";
        //存储记录
        public String[] record = new String[10];
        //存储表达式
        public String[] texts = new String[10000];      


        //处理表达式
        public void addComments(String s)
        {
            this.text += s;
            this.texts[tab] = s;
            this.richTextBox1.Text = this.text;
            tab++;
        }

        //0按钮的触发事件
        private void button10_Click(object sender, EventArgs e)
        {
            this.addComments("0");
        }
       
        //1按钮的触发事件
        private void button1_Click(object sender, EventArgs e)
        {
            this.addComments("1");
        }

        //2按钮的触发事件
        private void button2_Click(object sender, EventArgs e)
        {
            this.addComments("2");
        }

        //3按钮的触发事件
        private void button3_Click(object sender, EventArgs e)
        {
            this.addComments("3"); ;
        }

        //4按钮的触发事件
        private void button4_Click(object sender, EventArgs e)
        {
            this.addComments("4");
        }

        //5按钮的触发事件
        private void button5_Click(object sender, EventArgs e)
        {
            this.addComments("5");
        }

        //6按钮的触发事件
        private void button6_Click(object sender, EventArgs e)
        {
            this.addComments("6");
        }

        //7按钮的触发事件
        private void button7_Click(object sender, EventArgs e)
        {
            this.addComments("7");
        }

        //8按钮的触发事件
        private void button8_Click(object sender, EventArgs e)
        {
            this.addComments("8");
        }

        //9按钮的触发事件
        private void button9_Click(object sender, EventArgs e)
        {
            this.addComments("9");
        }
        //小数点.按钮的触发事件
        private void button12_Click(object sender, EventArgs e)
        {
            this.addComments(".");
        }

        //（按钮的触发事件
        private void button18_Click(object sender, EventArgs e)
        {
            this.addComments("(");
        }

        //）按钮的触发事件
        private void button17_Click(object sender, EventArgs e)
        {
            this.addComments(")");
        }

        //+按钮的触发事件
        private void button13_Click(object sender, EventArgs e)
        {
            this.addComments("+");
        }

        //-按钮的触发事件
        private void button14_Click(object sender, EventArgs e)
        {
            this.addComments("-");
        }

        //*按钮的触发事件
        private void button15_Click(object sender, EventArgs e)
        {
            this.addComments("*");
        }

        //按钮/的触发事件
        private void button16_Click(object sender, EventArgs e)
        {
            this.addComments("/");
        }

        /// <summary>
        /// =按钮的触发事件,最终计算结果并显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                
                string formula = this.text;
                DataTable dt = new DataTable();
                string result = dt.Compute(formula, "false").ToString();
        

                this.richTextBox1.Text = result;
                this.record[RecordNum] = this.text;
                this.text = result;

                this.RecordNum++;

                this.Precord = this.RecordNum;

            }
            catch (Exception)
            {
                this.richTextBox1.Text = "输入错误！";
                this.text = "";
                tab = 0;
            }

        }

        //清屏按钮的触发事件
        private void button20_Click(object sender, EventArgs e)
        {
            this.text = "";
            this.richTextBox1.Text = this.text;
        }

        //退格按钮的触发事件
        private void button19_Click(object sender, EventArgs e)
        {
            if (tab > 0)
            {
                tab -= 1;
            }
            this.text = "";
            for (int i = 0; i < tab; i++)
            {
                this.text += this.texts[i];
            }

            this.richTextBox1.Text = this.text;
        }

        //<-按钮的触发事件
        private void button21_Click(object sender, EventArgs e)
        {
            Precord--;
            if (Precord < 0)
            {
                Precord = RecordNum;
            }
            // this.text = this.record[Precord];
            this.richTextBox1.Text = this.record[Precord];
        }

        //->按钮的触发事件
        private void button22_Click(object sender, EventArgs e)
        {
            Precord++;
            if (Precord > RecordNum)
            {
                Precord = 0;
            }
            // this.text = this.record[Precord];

            this.richTextBox1.Text = this.record[Precord];

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void 计算器_Load(object sender, EventArgs e)
        {

        }
    }
}
