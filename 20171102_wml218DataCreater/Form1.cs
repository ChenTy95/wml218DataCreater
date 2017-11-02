using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _20171102_wml218DataCreater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bCheck_Click(object sender, EventArgs e)
        {
            string[] strLine = tPaste.Lines;
            int totalLines = strLine.Length;
            string[] userid = new string[totalLines];
            string[] username = new string[totalLines];
            int js = 0;
            try
            { 
                for (int i = 0; i < totalLines; i++)
                {
                    if (strLine[i] != "")
                    {
                        js++;
                        string tmpStr = strLine[i].Replace("\t","`");
                        userid[js] = tmpStr.Substring(0, tmpStr.IndexOf('`'));
                        username[js] = tmpStr.Substring(tmpStr.IndexOf('`')).Replace("`", "");
                    }
                }
                totalLines = js;
                tTotalNum.Text = totalLines.ToString();

                string identityStr = "" ;
                switch (cBoxIdentity.SelectedIndex)
                {
                    case 0:
                        identityStr = "bk";
                        break;
                    case 1:
                        identityStr = "szy";
                        break;
                    case 2:
                        identityStr = "by";
                        break;
                    case 3:
                        identityStr = "fdy";
                        break;
                    case 4:
                        identityStr = "jg";
                        break;
                }

                tPaste.Text = "";
                tPaste.Text += "[Name]" + tName.Text.Trim() + Environment.NewLine;
                tPaste.Text += "[TotalRecord]" + totalLines.ToString() + Environment.NewLine;
                tPaste.Text += "[Identity]" + identityStr + Environment.NewLine;
                for (int i = 1; i <= totalLines; i++)
                    tPaste.Text += userid[i] + "," + username[i] + Environment.NewLine;
                tPaste.Text += "[EOF.]";

                tPaste.ReadOnly = true;
                bConfirm.Enabled = false;
                bReset.Enabled = true;
                bSave.Enabled = true;
                bCheck.Enabled = false;

                bSave.Focus();
            }
            catch
            {
                MessageBox.Show("粘贴结果不符合格式要求！\r\n请从Excel表中复制，A列为证件号码，B列为姓名！");
                ResetInput();
                bReset.Enabled = false;
            }
            
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "请选择数据文件保存位置";
            saveFileDialog.Filter = "文本文件(*.txt)|";
            saveFileDialog.FileName = tName.Text + ".txt";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var utf8WithoutBom = new UTF8Encoding(false);
                using (var sink = new StreamWriter(saveFileDialog.FileName.ToString(), false, utf8WithoutBom))
                {
                    sink.Write(tPaste.Text);
                }
            }
        }

        private void tName_Click(object sender, EventArgs e)
        {
            tName.SelectAll();
        }

        private void tInfo_Click(object sender, EventArgs e)
        {
            tInfo.SelectAll();
        }

        private void bConfirm_Click(object sender, EventArgs e)
        {
            tName.Text = tName.Text.Trim();
            tInfo.Text = tInfo.Text.Trim();
            if (tName.Text != "" && tInfo.Text != "" && cBoxIdentity.SelectedIndex != -1)
            {
                gBox1.Enabled = false;
                tPaste.Enabled = true;
                bCheck.Enabled = true;
                bConfirm.Enabled = false;
            }
            else
                MessageBox.Show("数据文件设置未完成！");
        }

        private void tInfo_TextChanged(object sender, EventArgs e)
        {
            if (tInfo.Text.IndexOf("本") >= 0)
                cBoxIdentity.SelectedIndex = 0;
            else if (tInfo.Text.IndexOf("硕") >= 0)
                cBoxIdentity.SelectedIndex = 1;
            else if (tInfo.Text.IndexOf("博") >= 0)
                cBoxIdentity.SelectedIndex = 2;
            else if (tInfo.Text.IndexOf("辅") >= 0)
                cBoxIdentity.SelectedIndex = 3;
            else if (tInfo.Text.IndexOf("教") >= 0)
                cBoxIdentity.SelectedIndex = 4;
            else
                cBoxIdentity.SelectedIndex = -1;
        }

        private void ResetInput()
        {
            gBox1.Enabled = true;
            tPaste.ReadOnly = false;
            tPaste.Text = "";
            tPaste.Enabled = false;
            bCheck.Enabled = false;
            bSave.Enabled = false;
            bConfirm.Enabled = true;
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            ResetInput();
        }
    }
}
