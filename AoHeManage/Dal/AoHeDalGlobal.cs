using AoHeManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;
using AoHeManage.Common;
using System.Text;

namespace AoHeManage.Dal
{
    public class AoHeDalGlobal : AoHeFactory
    {
        public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString();

        public DataSet GetRecordPage<T>(int currentPage, int pageSize, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("  select COUNT(1) as totalrow from {0};  ", typeof(T).Name);
            strSql.AppendFormat(" select a.* from {0} a ", typeof(T).Name);
            strSql.AppendFormat(" where 1=1 {0} ", strWhere);
            if (filedOrder.Trim() != "")
            {
                strSql.Append(" order by " + filedOrder);
            }
            strSql.AppendFormat(" LIMIT {0},{1} ", (currentPage - 1) * pageSize, pageSize);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 使用反射获取数据模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T GetModel_Factory<T>(int ID)
        {
            Type type = typeof(T);
            T model = (T)Activator.CreateInstance(type);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string strSql = string.Format(" select * from {0} where ID = '{1}' ", type.Name, ID);
                MySqlCommand cmd = new MySqlCommand(strSql, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.Read())
                {
                    foreach (PropertyInfo item in type.GetProperties())
                    {
                        if (item.PropertyType.Name != reader[item.Name].GetType().Name)
                        {
                            item.SetValue(model, reader[item.Name].ToType(item.PropertyType.Name));
                        }
                        else
                        {
                            item.SetValue(model, reader[item.Name]);
                        }
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 使用反射插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Insert_Factory<T>(T t)
        {
            StringBuilder strSql_Column = new StringBuilder();
            StringBuilder strSql_Value = new StringBuilder();
            Type type = typeof(T);
            foreach (PropertyInfo item in type.GetProperties())
            {
                if (item.Name.ToUpper() == "ID")
                {
                    continue;
                }
                strSql_Column.AppendFormat(",{0} ", item.Name);
                strSql_Value.AppendFormat(",'{0}' ", item.GetValue(t));
            }
            string strSql = string.Format(" insert into {0} ( {1} ) values ( {2} ); ", type.Name, strSql_Column.Remove(0, 1).ToString(), strSql_Value.Remove(0, 1).ToString());
            return DbHelperSQL.ExecuteSql(strSql);
        }

        /// <summary>
        /// 使用反射更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Update_Factory<T>(T t)
        {
            StringBuilder strSql_Column = new StringBuilder();
            StringBuilder strSql_Where = new StringBuilder();
            Type type = typeof(T);
            foreach (PropertyInfo item in type.GetProperties())
            {
                if (item.Name.ToUpper() == "ID")
                {
                    strSql_Where.AppendFormat(" ID = '{0}' ", item.GetValue(t));
                }
                else
                {
                    CustomAttribute getAttribute = item.GetCustomAttribute(typeof(CustomAttribute)) as CustomAttribute;
                    if (getAttribute == null || !getAttribute.NoUpdate)
                    {
                        strSql_Column.AppendFormat(",{0}='{1}' ", item.Name, item.GetValue(t));
                    }
                }
            }
            string strSql = string.Format(" update {0} set {1} where {2} ; ", type.Name, strSql_Column.Remove(0, 1).ToString(), strSql_Where.ToString());
            return DbHelperSQL.ExecuteSql(strSql);
        }

        public bool Exists_Factory<T>(T t)
        {
            StringBuilder strSql_Where = new StringBuilder();
            Type type = typeof(T);
            foreach (PropertyInfo item in type.GetProperties())
            {
                if (item.Name.ToUpper() == "ID")
                {
                    var getValue = item.GetValue(t);
                    if (getValue.ToType<int>() > 0)
                    {
                        strSql_Where.AppendFormat(" and ID<>'{0}' ", getValue);
                    }
                }
                else
                {
                    CustomAttribute getAttribute = item.GetCustomAttribute(typeof(CustomAttribute)) as CustomAttribute;
                    if (getAttribute != null && getAttribute.Exists)
                    {
                        strSql_Where.AppendFormat(" and {0}='{1}' ", item.Name, item.GetValue(t));
                    }
                }
            }
            string strSql = string.Format(" select count(1) from {0} where 1=1 {1} ", type.Name, strSql_Where.ToString());
            return DbHelperSQL.Exists(strSql.ToString());
        }

        public int Delete_Factory<T>(T t)
        {
            throw new NotImplementedException();
        }
    }
}