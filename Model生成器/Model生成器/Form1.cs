using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DBUtil;
using Utils;
using System.Text.RegularExpressions;

namespace Model生成器
{
    public partial class Form1 : Form
    {
        #region Form1
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #endregion

        //生成
        private void btnCreate_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    IDal dal = DalFactory.CreateDal(ConfigurationManager.AppSettings["DBType"]);
                    List<Dictionary<string, string>> tableList = dal.GetAllTables();
                    string strNamespace = ConfigurationManager.AppSettings["Namespace"];
                    string strClassTemplate = string.Empty;
                    string strClassExtTemplate = string.Empty;
                    string strFieldTemplate = string.Empty;
                    Regex regField = new Regex(@"[ \t]*#field start([\s\S]*)#field end", RegexOptions.IgnoreCase);

                    #region 操作控件
                    InvokeDelegate invokeDelegate = delegate ()
                    {
                        btnCreate.Enabled = false;
                        progressBar1.Visible = true;
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = tableList.Count;
                        progressBar1.Value = 0;
                    };
                    InvokeUtil.Invoke(this, invokeDelegate);
                    #endregion

                    #region 读取模板
                    strClassTemplate = FileHelper.ReadFile(Application.StartupPath + "\\Template\\class.txt");
                    strClassExtTemplate = FileHelper.ReadFile(Application.StartupPath + "\\Template\\class_ext.txt");
                    Match matchField = regField.Match(strClassTemplate);
                    if (matchField.Success)
                    {
                        strFieldTemplate = matchField.Groups[1].Value.TrimEnd(' ');
                    }
                    #endregion

                    int i = 0;
                    foreach (Dictionary<string, string> table in tableList) //遍历表
                    {
                        string tableName = table["table_name"].ToUpper();
                        StringBuilder sbFields = new StringBuilder();
                        List<Dictionary<string, string>> columnList = dal.GetAllColumns(tableName);

                        #region 原始Model
                        string strClass = strClassTemplate.Replace("#table_comments", table["comments"]);
                        strClass = strClass.Replace("#table_name", tableName);

                        foreach (Dictionary<string, string> column in columnList) //遍历字段
                        {
                            string data_type = dal.ConvertDataType(column);

                            string strField = strFieldTemplate.Replace("#field_comments", column["comments"]);

                            if (column["constraint_type"] != "P")
                            {
                                strField = strField.Replace("        [IsId]\r\n", string.Empty);
                            }

                            strField = strField.Replace("#data_type", data_type);
                            strField = strField.Replace("#field_name", column["columns_name"].ToUpper());

                            sbFields.Append(strField);
                        }

                        strClass = regField.Replace(strClass, sbFields.ToString());

                        FileHelper.WriteFile(Application.StartupPath + "\\Models", strClass, tableName);
                        #endregion

                        #region 扩展Model
                        string strClassExt = strClassExtTemplate.Replace("#table_comments", table["comments"]);
                        strClassExt = strClassExt.Replace("#table_name", tableName);

                        FileHelper.WriteFile(Application.StartupPath + "\\ExtModels", strClassExt.ToString(), tableName);
                        #endregion

                        #region 操作控件
                        invokeDelegate = delegate ()
                        {
                            progressBar1.Value = ++i;
                        };
                        InvokeUtil.Invoke(this, invokeDelegate);
                        #endregion
                    }

                    #region 操作控件
                    invokeDelegate = delegate ()
                    {
                        btnCreate.Enabled = true;
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                    };
                    InvokeUtil.Invoke(this, invokeDelegate);
                    #endregion

                    MessageBox.Show("完成");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                }
            })).Start();
        }
    }
}
