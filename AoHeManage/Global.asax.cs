using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AoHeManage
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // 获取到HttpUnhandledException异常，这个异常包含一个实际出现的异常
            Exception ex = Server.GetLastError();
            //实际发生的异常
            Exception innerException = ex.InnerException;
            if (innerException != null) ex = innerException;

            using (StreamWriter sw = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "Exception.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("Global捕获到未处理异常：" + ex.GetType().ToString());
                sw.WriteLine("异常信息：" + ex.Message);
                sw.WriteLine("异常堆栈：" + ex.StackTrace);
                sw.WriteLine();
            }

            HttpContext.Current.Response.Write(string.Format("捕捉到未处理的异常：{0}<br/>", ex.GetType().ToString()));
            HttpContext.Current.Response.Write("请刷新页面重新操作。");
            Server.ClearError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}