using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AoHeManage.Model
{
    public interface AoHeFactory
    {
        T GetModel_Factory<T>(int ID);

        int Insert_Factory<T>(T t);

        int Update_Factory<T>(T t);

        int Delete_Factory<T>(T t);
    }
}