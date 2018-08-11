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

        /// <summary>
        /// 是否是冗余字段，只是用来显示的作用
        /// </summary>
        public bool Redundant { get; set; }
    }
}