using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimPlan.Models;
using System.Diagnostics;

namespace TimPlan.Lib
{
    static public class SQLAccess
    {

        static private string _ConnectionString = "Data Source=.\\TimPlanDB.db;Version=3;";

        #region Select

        static public UserModel? SelectUser(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {UserModel.DbName} " +
                                    $"WHERE {UserModel.DbIdCol}={id}";
                    UserModel queryOutput = connection.QuerySingle<UserModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectUser(uint):{e.Message}");
                return null;
            }
        }

        static public UserModel SelectUser(string login, string password)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {UserModel.DbName} " +
                                    $"WHERE {UserModel.DbLoginCol}='{login}' " +
                                    $"AND {UserModel.DbPasswordCol}='{password}'";

                    UserModel queryOutput = connection.QuerySingle<UserModel>(queryString, new DynamicParameters());
                    Debug.WriteLine(queryOutput);

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectUser(string, string):{e.Message}");
                return null;
            }
        }

        #endregion


        #region Insert

        public static void InsertUser(UserModel user)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"INSERT " +
                                    $"INTO Users (Name) " +
                                    $"VALUES (@Name)";
                    connection.Execute(queryString, user);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertUser():{e.Message}");
            }
        }

        public static void InsertTeam(TeamModel team)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"INSERT " +
                                    $"INTO Teams (Name) " +
                                    $"VALUES (@Name)";
                    connection.Execute(queryString, team);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertUser():{e.Message}");
            }
        }

        #endregion


    }
}
