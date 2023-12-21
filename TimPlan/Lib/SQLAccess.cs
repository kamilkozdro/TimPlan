using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;

namespace TimPlan.Lib
{
    static public class SQLAccess
    {

        static private string _ConnectionString = "name=\"Default\" connectionString=\"Data Source=.\\SharedPlannerDB.db;Version=3;\" providerName=\"System.Data.SqlClient\"";

        public static UserModel? SelectUser(uint userId)
        {
            using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
            {
                string queryString = $"SELECT * " +
                                $"FROM users " +
                                $"WHERE id='{userId}'";
                var queryOutput = connection.Query<UserModel>(queryString, new DynamicParameters());
                queryOutput.ToList();

                if (queryOutput.Count() != 1)
                {
                    // THROW ERROR - Database Error: Not unique user name!
                    return null;
                }

                return queryOutput.ElementAt(0);
            }
        }

        public static void InsertUser(UserModel user)
        {
            using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
            {
                string queryString = $"INSERT " +
                                $"INTO Users (Name) " +
                                $"VALUES (@Name)";
                connection.Execute(queryString, user);
            }
        }
    }
}
