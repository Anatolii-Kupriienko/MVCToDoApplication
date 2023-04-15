using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ToDoList.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ToDoList.DataAccess.SQLDataAccess
{
    public static class SQLDataAccess
    {
        static WebApplicationBuilder builder = WebApplication.CreateBuilder();

        public static string GetConnectionString(string connectionName = "MVCToDoList")
        {
            
            return builder.Configuration.GetConnectionString("MVCToDoList");
        }
        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }
        public static List<T> LoadData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql, data).ToList();
            }
        }
        public static int SaveData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }
        public static int DeleteData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
                
            }
        }
    }
}
