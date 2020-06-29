using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    /// <summary>
    /// 物品代购
    /// </summary>
    [Serializable]
    public partial class TWO_ORDER
    {

        /// <summary>
        /// 主键
        /// </summary>
        [IsId]
        [IsDBField]
        public string ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [IsDBField]
        public string ORDER_NO { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        [IsDBField]
        public DateTime ORDER_TIME { get; set; }

        /// <summary>
        /// 在押人员ID
        /// </summary>
        [IsDBField]
        public string PRISONER_ID { get; set; }

        /// <summary>
        /// 在押人员姓名
        /// </summary>
        [IsDBField]
        public string PRISONER_NAME { get; set; }

        /// <summary>
        /// 监室号
        /// </summary>
        [IsDBField]
        public string ROOM_NO { get; set; }

        /// <summary>
        /// 代购物品总金额
        /// </summary>
        [IsDBField]
        public decimal TOTAL { get; set; }

        /// <summary>
        /// 实际消费金额
        /// </summary>
        [IsDBField]
        public decimal? T_REAL { get; set; }

        /// <summary>
        /// 订单状态(0草稿 1已下单 2采购中 3部分已退回 4已完成)
        /// </summary>
        [IsDBField]
        public int T_STATUS { get; set; }

        /// <summary>
        /// 删除标志(0正常 1已删除)
        /// </summary>
        [IsDBField]
        public string DEL_FLAG { get; set; }

    }
}
