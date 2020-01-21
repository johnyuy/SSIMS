using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Database
{
    public class ServerConnection
    {
        public static string ServerName = "T450SS\\SQLEXPRESS";
        public static string DbName = "SSIMS";
        public static string ConnectionString = "Server=" + ServerName + ";Database=" + DbName + ";Integrated Security=True";
    }
}