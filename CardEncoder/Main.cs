using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CardEncoder
{
    public partial class Main : Form
    {
        private MessageTransport mt = new MessageTransport();
        private string[] passwords = { "FFFFFFFFFFFF", "444C47584854" };

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            string[] ports = mt.getComPortItems();
            if (ports.Length > 0) {
                comboBox1.Items.AddRange(ports);
                comboBox1.SelectedItem = ports[0];
            }

            comboBox2.Items.AddRange(passwords);
            comboBox2.SelectedItem = passwords[0];

            comboBox3.Items.AddRange(passwords);
            comboBox3.SelectedItem = passwords[1];
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length <= 0) {
                MessageBox.Show("请先选择一个串口", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (mt.isOpen()) {
                println("串口己被打开，不用再次打开");
            }

            try
            {
                mt.open(comboBox1.Text);
                println("打开串口成功：" + comboBox1.Text);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                println("打开串口失败：" + comboBox1.Text);
            }

        }

        private void println(string p)
        {
            textBox1.AppendText(DateTime.Now.ToLongTimeString() + " ： " + p);
            textBox1.AppendText("\r\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!mt.isOpen()) {
                println("串口并未被打开，不用关闭");
                return;
            }
            try
            {
                mt.close();
                println("关闭串口成功：" + comboBox1.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                println("关闭串口失败：" + comboBox1.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!mt.isOpen()) {
                println("串口未打开");
                return;
            }
            try
            {
                string card = mt.write(MessageDefine.readCard, 9, 500);
                println("卡片内码：" + card.Substring(8, 12));
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                println("读卡失败");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!mt.isOpen())
            {
                println("串口未打开");
                return;
            }

            string cardId = String.Empty;
            try
            {
                string card = mt.write(MessageDefine.readCard, 9, 500);
                cardId = card.Substring(8, 12);
                println("卡片内码：" + cardId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                println("读卡失败");
                return;
            }

            try
            {
                string isPasswordRight = mt.write(MessageDefine.loadPassword(comboBox2.Text), 6, 500);
                if (!isPasswordRight.Substring(9, 2).Equals("59"))
                {
                    println("验证密码失败：" + isPasswordRight);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                println("加载密码失败");
                return;
            }

            if (checkBox1.Checked) {
                updatePassword(checkBox1.Text, comboBox2.Text, comboBox3.Text, cardId);
            }
            if (checkBox2.Checked)
            {
                updatePassword(checkBox2.Text, comboBox2.Text, comboBox3.Text, cardId);
            }
            if (checkBox3.Checked)
            {
                updatePassword(checkBox3.Text, comboBox2.Text, comboBox3.Text, cardId);
            }
            if (checkBox4.Checked)
            {
                updatePassword(checkBox4.Text, comboBox2.Text, comboBox3.Text, cardId);
            }
        }

        private void updatePassword(string block, string oldPassword, string newPassword,string cardId)
        {
            try
            {
                string card = mt.write(MessageDefine.modifyPassword(block,oldPassword,newPassword), 9, 500);
                if (card.Substring(8, 12).Equals(cardId)) {
                    println("修改扇区" + block + "密码成功");
                    return;
                }
                println("修改扇区" + block + "密码失败：" + card);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                println("修改扇区" + block + "密码失败");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string oldPassword = comboBox2.Text;
            string newPassword = comboBox3.Text;

            comboBox3.Text = oldPassword;
            comboBox2.Text = newPassword;
        }

        
    }
}
