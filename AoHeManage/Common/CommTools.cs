﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;

namespace AoHeManage.Common
{
    public static class CommTools
    {
        /// <summary>
        /// 获取导航链接
        /// </summary>
        /// <returns></returns>
        public static StreamWriter streamWriter;
        public static string getNav(int currentpage, int totalpage, int totalrow, int pagesize, bool model)
        {
            string href = (model == false ? "javascript:void(0);" : "#");
            StringBuilder nav = new StringBuilder();
            nav.Append("<div id='div_nav' style='height: 25px;'><table><tr><td id='page_td' style='text-align:center'>");
            nav.Append("每页条数&nbsp;<select id='sel_pagesize' style='width:96px;' onchange='getPage(1)' class='pagenav'><option value='15'>15</option><option value='20'>20</option><option value='30'>30</option><option value='50'>50</option><option value='100'>100</option><option value='200'>200</option></select>&nbsp;&nbsp;&nbsp;");
            if (currentpage > 1)
            {
                nav.Append("[<a href='" + href + "' onclick='getPage(1)'>首页</a>] ");
            }
            else
            {
                nav.Append("[首页] ");
            }
            int prev = currentpage - 1;
            if (prev > 0)
            {
                nav.Append("&nbsp;&nbsp;[<a href='" + href + "' onclick='getPage(" + prev + ")'>上一页</a>] ");
            }
            else
            {
                nav.Append("&nbsp;&nbsp;[上一页] ");
            }
            int next = currentpage + 1;
            if (next <= totalpage)
            {
                nav.Append("&nbsp;&nbsp;[<a href='" + href + "' onclick='getPage(" + next + ")'>下一页</a>] ");
            }
            else
            {
                nav.Append("&nbsp;&nbsp;[下一页] ");
            }
            if (totalpage > 1)
            {
                if (currentpage != totalpage)
                {
                    nav.Append("&nbsp;&nbsp;[<a href='" + href + "' onclick='getPage(" + totalpage + ")'>末页</a>] ");
                }
                else
                {
                    nav.Append("&nbsp;&nbsp;[末页] ");
                }
            }
            else
            {
                nav.Append("&nbsp;&nbsp;[末页] ");
            }
            if (currentpage > totalpage)
            {
                if (totalpage == 1) { currentpage = 1; }
                if (totalpage > 1) { currentpage = totalpage - 1; }
                if (totalpage == 0) { currentpage = 0; }
            }
            nav.Append("&nbsp;&nbsp;第 <strong><font color=red>" + currentpage.ToString() + "</font></strong> 页/共 <strong><font color=red>" + totalpage.ToString() + "</font></strong> 页 &nbsp;&nbsp;共 <strong><font color=red>" + totalrow.ToString() + "</font></strong> 条记录");
            nav.Append("&nbsp;&nbsp;&nbsp;跳转到第&nbsp;<input type='text' id='txt_topages' title='输入数字后回车'  value=" + currentpage + " style='width:36px;ime-mode:disabled;height:17px;' onkeydown='pagespress()' />&nbsp;页&nbsp;&nbsp;&nbsp;<a href='javascript:void(0);' onclick='pagespressForGo()' style='padding:2px 6px 3px 6px;background-color:#157EFB;color:white'>确定</a>");
            nav.Append("<input type='hidden' id='hid_currentpage' class='pagenav' value=" + currentpage + " style='width:0px;' /><input type='hidden' id='hid_totalpage' value=" + totalpage + " style='width:0px;' /></td></tr></table></div>");
            //nav.Append("<script type='text/javascript'>$(\"#sel_pagesize\").attr(\"value\", '" + pagesize.ToString() + "');
            //           + " function pagespress() { var k=window.event.keyCode; "
            //           + " if ((k==46)||(k==8)||(k>=48 && k<=57)||(k>=96 && k<=105)||(k>=37 && k<=40)) {} "
            //           + " else if(k==13) { var topage=$(\"#txt_topages\").val(); if (topage!='0') { if (topage>$(\"#hid_totalpage\").val()) { topage=$(\"#hid_totalpage\").val(); $(\"#txt_topages\").val(topage); } getPage(topage); } } "
            //           + " else { window.event.returnValue = false;} "
            //           + " } </script>");
            nav.Append("<script type='text/javascript'>$(\"#sel_pagesize\").val('" + pagesize.ToString() + "');</script>");
            return nav.ToString();
        }

        //将一个对象转换为Json字符串
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
        //将一个Json字符串转换为对象
        public static object JsonToObject(string jsonString, Type type)
        {
            DataContractJsonSerializer serilizer = new DataContractJsonSerializer(type);
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serilizer.ReadObject(stream);
        }

        //记录文本日志
        public static void WriteErrorLog(string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    return;
                }
                string path = @"C:\AoHeErrorLog";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += string.Format(@"\{0}.log", DateTime.Now.ToString("yyyy-MM-dd"));
                if (streamWriter == null)
                {
                    streamWriter = !File.Exists(path) ? File.CreateText(path) : File.AppendText(path);
                }
                streamWriter.WriteLine("*******************************************************");
                streamWriter.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                streamWriter.WriteLine("异常信息：");
                streamWriter.WriteLine(message);

            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
            }
        }

        public static string GetSqlInStr(List<string> list)
        {
            string str = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    str += "'" + list[i] + "'";
                }
                else
                {
                    str += ",'" + list[i] + "'";
                }
            }
            return str;
        }

        public static T ToType<T>(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }
            try
            {
                Type type = typeof(T);
                return (T)GetValue(value, type);
            }
            catch
            {
                return default(T);
            }
        }

        public static object ToType(this object value, string typeName)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }
            try
            {
                switch (typeName)
                {
                    case "String":
                        return Convert.ToString(value);
                    case "Int16":
                        return Convert.ToInt16(value);
                    case "Int32":
                        return Convert.ToInt32(value);
                    case "Int64":
                        return Convert.ToInt64(value);
                    case "Boolean":
                        try
                        {
                            //字符串的"true"和"false"都可转换(无论大小写)，字符串的"1"无法转换为true，字符串的"0"无法转换为false
                            if (Convert.ToString(value) == "1")
                            {
                                return true;
                            }
                            if (Convert.ToString(value) == "0")
                            {
                                return false;
                            }
                            return Convert.ToBoolean(value);
                        }
                        catch
                        {
                            return false;
                        }
                    case "Byte":
                        return Convert.ToByte(value);
                    case "Decimal":
                        return Convert.ToDecimal(value);
                    case "Double":
                        return Convert.ToDouble(value);
                    case "DateTime":
                        return Convert.ToDateTime(value);
                    default:
                        return value;
                }
            }
            catch
            {
                return null;
            }
        }

        public static object GetValue(object value, Type type)
        {
            switch (type.Name)
            {
                case "String":
                    return Convert.ToString(value);
                case "Int16":
                    return Convert.ToInt16(value);
                case "Int32":
                    return Convert.ToInt32(value);
                case "Int64":
                    return Convert.ToInt64(value);
                case "Boolean":
                    try
                    {
                        //字符串的"true"和"false"都可转换(无论大小写)，字符串的"1"无法转换为true，字符串的"0"无法转换为false
                        if (Convert.ToString(value) == "1")
                        {
                            return true;
                        }
                        if (Convert.ToString(value) == "0")
                        {
                            return false;
                        }
                        return Convert.ToBoolean(value);
                    }
                    catch
                    {
                        return false;
                    }
                case "Byte":
                    return Convert.ToByte(value);
                case "Decimal":
                    return Convert.ToDecimal(value);
                case "Double":
                    return Convert.ToDouble(value);
                case "DateTime":
                    return Convert.ToDateTime(value);
                default:
                    return value;
            }
        }
    }
}