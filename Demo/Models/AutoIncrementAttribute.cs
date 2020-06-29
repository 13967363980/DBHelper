using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    /// 标识该属性是自增字段 或 标识该类的主键是自增字段
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class AutoIncrementAttribute : Attribute
    {
    }
}
