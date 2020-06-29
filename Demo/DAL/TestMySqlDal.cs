using DBUtil;
using Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TestMySqlDal
    {
        #region 获取集合
        /// <summary>
        /// 获取集合
        /// </summary>
        public List<utils_test> GetList()
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from utils_test t
                where 1=1"));
            return DBHelper.FindListBySql<utils_test>(sql.ToString());
        }
        #endregion

        #region 获取集合 (参数化查询)
        /// <summary>
        /// 获取集合
        /// </summary>
        public List<utils_test> GetList2(string name, DateTime startTime, DateTime endTime)
        {
            StringBuilder sql = new StringBuilder(string.Format(@"
                select *
                from utils_test t
                where 1=1 "));

            sql.AppendFormat(" and name like @name");

            sql.AppendFormat(" and (add_time between @startTime and @endTime)");

            DbParameter[] param = new DbParameter[3] {
                new MySqlParameter() { ParameterName="name" , Value="%"+name+"%" },
                new MySqlParameter() { ParameterName="startTime" , Value=startTime.ToString("yyyy-MM-dd HH:mm:ss") },
                new MySqlParameter() { ParameterName="endTime" , Value=endTime.ToString("yyyy-MM-dd HH:mm:ss") }
            };

            List<utils_test> result = DBHelper.FindListBySql<utils_test>(sql.ToString(), param);
            return result;
        }
        #endregion

        #region 获取集合 (参数化查询)
        /// <summary>
        /// 获取集合
        /// </summary>
        public List<utils_test> GetList3(string name, DateTime startTime, DateTime endTime)
        {
            SqlString sql = new SqlString(@"
                select *
                from utils_test t
                where 1=1 ");

            sql.AppendSql(" and name like @name", "%" + name + "%");

            sql.AppendSql(" and (add_time between @startTime and @endTime)", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

            List<utils_test> result = DBHelper.FindListBySql<utils_test>(sql.SQL, sql.Params);
            return result;
        }
        #endregion

        #region 获取集合 (参数化非参数化混合查询)
        /// <summary>
        /// 获取集合
        /// </summary>
        public List<TWO_ORDER> GetList(string name, DateTime startTime, DateTime endTime, int? status)
        {
            SqlString sql = new SqlString(@"
                select *
                from two_order t
                where 1=1 
                and DEL_FLAG=@delFlag", "0");

            sql.AppendSql(@" 
                and (
                    ORDER_TIME>=STR_TO_DATE(@startTime, '%Y-%m-%d %H:%i:%s') 
                    and ORDER_TIME<=STR_TO_DATE(@endTime, '%Y-%m-%d %H:%i:%s')
                )", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql.AppendSql(" and PRISONER_NAME like @name", "%" + name + "%");
            }

            if (status != null)
            {
                sql.AppendSql(" and T_STATUS = @status", status);
            }

            sql.AppendSql(" and 1=1"); //测试没有参数

            sql.AppendSql(" order by ORDER_TIME desc, ID asc"); //测试排序

            List<TWO_ORDER> result = DBHelper.FindListBySql<TWO_ORDER>(sql.SQL, sql.Params);
            return result;
        }
        #endregion

        #region 分页获取集合 (参数化非参数化混合查询)
        /// <summary>
        /// 分页获取集合 
        /// </summary>
        public List<TWO_ORDER> GetListPage(ref PagerModel pager, string name, DateTime startTime, DateTime endTime, int? status)
        {
            SqlString sql = new SqlString(string.Format(@"
                select *
                from two_order t
                where 1=1 
                and DEL_FLAG='{0}'", "0"));

            sql.AppendSql(@" 
                and (
                    ORDER_TIME>=STR_TO_DATE(@startTime, '%Y-%m-%d %H:%i:%s') 
                    and ORDER_TIME<=STR_TO_DATE(@endTime, '%Y-%m-%d %H:%i:%s')
                )", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql.AppendSql(" and PRISONER_NAME like @name", "%" + name + "%");
            }

            if (status != null)
            {
                sql.AppendSql(" and T_STATUS = @status", status);
            }

            if (status != null)
            {
                sql.AppendFormat(" and T_STATUS = '{0}'", status); //测试追加非参数化SQL
            }

            sql.AppendSql(" and 1=1"); //测试没有参数

            string orderby = "order by ORDER_TIME desc, ID asc"; //测试排序
            pager = DBHelper.FindPageBySql<TWO_ORDER>(sql.SQL, orderby, pager.rows, pager.page, sql.Params);
            return pager.result as List<TWO_ORDER>;
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        public void Insert(object obj)
        {
            DBHelper.Insert(obj);
        }
        #endregion

        #region 查询单个记录 (参数化查询)
        /// <summary>
        /// 查询单个记录
        /// </summary>
        public utils_test Get(string name)
        {
            SqlString sql = new SqlString(@"
                select *
                from utils_test t
                where 1=1 
                and name=@name", name);

            utils_test result = DBHelper.FindBySql<utils_test>(sql.SQL, sql.Params);
            return result;
        }
        #endregion

    }
}
