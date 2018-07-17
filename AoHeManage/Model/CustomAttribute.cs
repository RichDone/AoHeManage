using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AoHeManage.Model
{
    public class CustomAttribute:Attribute
    {
        /// <summary>
        /// 判断是否存在时的字段
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// 更新有些字段不需要更新
        /// </summary>
        public bool NoUpdate { get; set; }
    }
}