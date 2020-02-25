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
        public List<utils_test> GetList(string name, DateTime startTime, DateTime endTime)
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

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        public void Insert(object obj)
        {
            DBHelper.Insert(obj);
        }
        #endregion

    }
}
