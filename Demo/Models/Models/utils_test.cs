using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class utils_test
    {
        /// <summary>
        /// 主键
        /// </summary>
        [IsId]
        [IsDBField]
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [IsDBField]
        public string name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [IsDBField]
        public string code { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [IsDBField]
        public DateTime? add_time { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [IsDBField]
        public long? count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [IsDBField]
        public byte[] content { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        [IsDBField]
        public string text { get; set; }
    }
}
