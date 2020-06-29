using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DBUtil;
using Models;
using System.Threading;
using Utils;

namespace Demo
{
    public partial class Form1 : Form
    {
        private TaskSchedulerEx _task = null;
        private TemplateDal m_TemplateDal = new TemplateDal();
        private TestDal m_TestDal = new TestDal();
        private TestMySqlDal m_TestMySqlDal = new TestMySqlDal();

        #region Form1
        public Form1()
        {
            InitializeComponent();
            ThreadPool.SetMinThreads(128, 128);
            _task = new TaskSchedulerEx(128, 128);
        }
        #endregion

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            BindList();
        }
        #endregion

        #region 绑定列表
        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindList()
        {
            try
            {
                gridView.DataSource = null;
                PagerModel pager = pagerControl1.Pager;
                List<BS_Template> list = m_TemplateDal.GetList(ref pager, null, null, null, Enums.TemplateType.Notice);
                pagerControl1.Pager = pager;
                list.ForEach(a =>
                {

                });
                gridView.ClearSelection();
                gridView.Columns.Clear();
                gridView.AutoGenerateColumns = false;
                DataGridViewTextBoxColumn dc = new DataGridViewTextBoxColumn();
                dc.HeaderText = "ID";
                dc.DataPropertyName = "id";
                //dc.Visible = false;
                gridView.Columns.Add(dc);
                dc = new DataGridViewTextBoxColumn();
                dc.HeaderText = "编码";
                dc.DataPropertyName = "code";
                gridView.Columns.Add(dc);
                dc = new DataGridViewTextBoxColumn();
                dc.HeaderText = "名称";
                dc.DataPropertyName = "name";
                dc.Width = 170;
                gridView.Columns.Add(dc);
                dc = new DataGridViewTextBoxColumn();
                dc.HeaderText = "备注";
                dc.DataPropertyName = "remarks";
                gridView.Columns.Add(dc);
                gridView.ReadOnly = true;
                gridView.DataSource = list;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        #endregion

        #region pagerControl1_PageChanged 翻页事件
        /// <summary>
        /// 翻页事件
        /// </summary>
        private void pagerControl1_PageChanged()
        {
            BindList();
        }
        #endregion

        #region pagerControl1_RefreshData 刷新数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        private void pagerControl1_RefreshData()
        {
            BindList();
        }
        #endregion

        #region button1_Click 测试新增
        private void button1_Click(object sender, EventArgs e)
        {
            int k = 0;
            for (int i = 0; i < 100; i++)
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    try
                    {
                        DBHelper.BeginTransaction();

                        BS_Template model = new BS_Template();
                        model.id = m_TemplateDal.GetMaxId().ToString();
                        model.code = k.ToString("0000");
                        model.name = "测试" + k.ToString();
                        model.remarks = "测试" + k.ToString();
                        model.type = ((int)Enums.TemplateType.Notice).ToString();
                        m_TemplateDal.Insert(model);
                        //throw new Exception("a");

                        BS_Test test = new BS_Test();
                        test.id = m_TestDal.GetMaxId().ToString();
                        test.code = "测试" + k.ToString();
                        test.name = "测试" + k.ToString();
                        test.remarks = "测试" + k.ToString();
                        m_TestDal.Insert(test);

                        DBHelper.CommitTransaction();

                        k++;
                        if (k == 100)
                        {
                            MessageBox.Show("插入数据成功");
                            this.Invoke(new Action(() =>
                            {
                                BindList();
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        DBHelper.RollbackTransaction();
                        MessageBox.Show(ex.Message);
                    }
                }));
            }
        }
        #endregion

        #region button2_Click 测试修改
        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            try
            {
                //DBHelper.BeginTransaction();
                foreach (DataGridViewRow dr in gridView.SelectedRows)
                {
                    BS_Template old = (BS_Template)dr.DataBoundItem;
                    BS_Template model = m_TemplateDal.Get2(old.id, Enums.TemplateType.Notice);
                    model.remarks = rnd.Next(1, 9999).ToString("0000");
                    m_TemplateDal.Update(model);
                }
                //DBHelper.CommitTransaction();
                MessageBox.Show("修改成功");
                BindList();
            }
            catch (Exception ex)
            {
                DBHelper.RollbackTransaction();
                MessageBox.Show("修改失败：" + ex.Message);
            }
        }
        #endregion

        #region button3_Click 测试删除
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DBHelper.BeginTransaction();
                foreach (DataGridViewRow dr in gridView.SelectedRows)
                {
                    BS_Template old = (BS_Template)dr.DataBoundItem;
                    m_TemplateDal.Del(Convert.ToInt32(old.id));
                }
                DBHelper.CommitTransaction();
                MessageBox.Show("删除成功");
                BindList();
            }
            catch (Exception ex)
            {
                DBHelper.RollbackTransaction();
                MessageBox.Show("修改失败：" + ex.Message);
            }
        }
        #endregion

        #region 测试MySQL
        private void btnTestMySQL_Click(object sender, EventArgs e)
        {
            try
            {
                #region 新增数据
                if (false)
                {
                    _task.Run(() =>
                    {
                        int n = 1000;
                        DateTime dt = DateTime.Now;
                        List<Task> tList = new List<Task>();
                        for (int i = 1; i <= n; i++)
                        {
                            Task t = _task.Run((obj) =>
                            {
                                var k = (int)obj;

                                try
                                {
                                    DBHelper.BeginTransaction();

                                    utils_test model = new utils_test();
                                    model.code = k.ToString("0000");
                                    model.name = "测试" + k.ToString();
                                    model.text = "测试" + k.ToString();
                                    model.content = new byte[10];
                                    model.content[1] = (byte)100;
                                    model.content[2] = (byte)99;
                                    model.content[3] = (byte)98;
                                    model.add_time = DateTime.Now;
                                    m_TestMySqlDal.Insert(model);

                                    DBHelper.CommitTransaction();
                                }
                                catch (Exception ex)
                                {
                                    DBHelper.RollbackTransaction();
                                    MessageBox.Show(ex.Message);
                                }
                            }, i);
                            tList.Add(t);
                        }
                        Task.WaitAll(tList.ToArray());
                        double d = DateTime.Now.Subtract(dt).TotalSeconds;
                        MessageBox.Show(n + "条数据插入完成，耗时：" + d.ToString("0.000") + "秒");
                        this.Invoke(new Action(() =>
                        {
                            BindList();
                        }));
                    });
                }
                #endregion

                #region 查询数据
                if (true)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Thread.Sleep(100);
                            DateTime dt = DateTime.Now;
                            utils_test info = m_TestMySqlDal.Get("测试1");
                            List<utils_test> list1 = m_TestMySqlDal.GetList3("测试", DateTime.Now.Date, DateTime.Now.Date.AddDays(1).AddSeconds(-1));
                            double d = DateTime.Now.Subtract(dt).TotalSeconds;
                            MessageBox.Show("成功，list1总数：" + list1.Count + "，耗时：" + d.ToString("0.000") + "秒");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
                }
                #endregion

                #region 查询数据
                if (false)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Thread.Sleep(100);
                            List<TWO_ORDER> list1 = m_TestMySqlDal.GetList("Shao", new DateTime(2020, 5, 1, 0, 0, 0), new DateTime(2020, 5, 15, 0, 0, 0), 1);
                            PagerModel pager = new PagerModel(2, 10);
                            List<TWO_ORDER> list2 = m_TestMySqlDal.GetListPage(ref pager, "Guo", new DateTime(2020, 5, 1, 0, 0, 0), new DateTime(2020, 5, 15, 0, 0, 0), 1);
                            MessageBox.Show("成功，list1总数：" + list1.Count + "，list2总数：" + pager.totalRows + "，当前页：" + pager.page + "，当前页数量：" + list2.Count);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
                }
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    } //end of class Form1

}
