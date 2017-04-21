using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Management;

namespace StudentsControl.Controllers
{
    public class Class_Funciones
    {
       
        public static DataTable GetDataTable(string sqlQuery, StudentsControl.Models.StudentsControlContext _context)
        {
            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(_context.Database.Connection);

                using (var cmd = factory.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = _context.Database.Connection;
                    using (var adapter = factory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;

                        var tb = new DataTable();
                        adapter.Fill(tb);
                        return tb;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SqlExecutionException(string.Format("Error occurred during SQL query execution {0}", sqlQuery), ex);
            }
        }

        
    }
}