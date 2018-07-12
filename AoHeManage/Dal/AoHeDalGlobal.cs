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
                if (strSql_Column.Length == 0)
                {
                    strSql_Column.AppendFormat(" {0} ", item.Name);
                    strSql_Value.AppendFormat(" '{0}' ", item.GetValue(t));
                }
                else
                {
                    strSql_Column.AppendFormat(" ,{0} ", item.Name);
                    strSql_Value.AppendFormat(" ,'{0}' ", item.GetValue(t));
                }
            }
            string strSql = string.Format(" insert into {0} ( {1} ) values ( {2} ); ", type.Name, strSql_Column.ToString(), strSql_Value.ToString());
            return DbHelperSQL.ExecuteSql(strSql);
        }

        public int Update_Factory<T>(T t)
        {
            throw new NotImplementedException();
        }

        public int Delete_Factory<T>(T t)
        {
            throw new NotImplementedException();
        }
    }
}