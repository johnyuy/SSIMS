using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Database
{
    public class Data
    {
        public static string serverName = "T450SS\\SQLEXPRESS";
        public static string dbName = "SSIMS";
        public static string connectionString = "Server=" + serverName + ";Database=" + dbName + ";Integrated Security=True";
    }
}