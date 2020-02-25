using DBUtil;
using Models;
using System;
using System.Collections.Generic;
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
