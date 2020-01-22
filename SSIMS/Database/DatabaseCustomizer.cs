using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using SSIMS.Models;


namespace SSIMS.Database
{
    public static class DatabaseCustomizer
    {
        public static void SetDefaultID<T>(T entityObj, string ID, int DefaultValue, DatabaseContext context)
        {
            string table = (entityObj.GetType().Name) + "s";
            string sql = "INSERT INTO " + table + " (";
            List<string> attibuteList = new List<string>();
            List<string> valueList = new List<string>();

            foreach (var prop in entityObj.GetType().GetProperties())
            {
                if (prop.GetValue(entityObj, null) != null)
                {
                    if (prop.Name == ID)
                    {
                        sql += prop.Name;
                    }
                    else if (prop.GetValue(entityObj, null).ToString() != null)
                    {
                        attibuteList.Add(prop.Name);
                        valueList.Add(prop.GetValue(entityObj, null).ToString());
                    }
                }
            }
            
            for(int i = 0; i < attibuteList.Count; i++)
            {
                if (i < attibuteList.Count)
                    sql += ", ";
                sql += attibuteList[i];
            }
            sql += ") VALUES (" + DefaultValue.ToString();
            for(int i = 0; i < valueList.Count; i++)
            {
                if (i < valueList.Count)
                    sql += ", ";
                sql += "'" + valueList[i]+"'";
            }
            sql += ")";
            InsertCommand(sql, table, context);
        }

        static void InsertCommand(string sql, string table, DatabaseContext context)
        {
            context.Database.Connection.Open();
            try
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo." + table + " ON");
                context.Database.ExecuteSqlCommand(sql);
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo." + table + " OFF");
            }
            finally
            {
                context.Database.Connection.Close();
            }
        }
    }
}