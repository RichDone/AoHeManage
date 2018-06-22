using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AoHeManage.Model
{
    public interface AoHeFactory<T>
    {
        T GetModel(DataSet ds);
    }
}