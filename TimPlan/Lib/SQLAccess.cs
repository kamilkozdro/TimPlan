using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using TimPlan.Models;
using System.Diagnostics;

namespace TimPlan.Lib
{
    static public class SQLAccess
    {

        static private string _ConnectionString = "Data Source=.\\TimPlanDB.db;Version=3;";

        #region Select

        static public List<UserModel> SelectAllUsers()
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {UserModel.DbName} ";
                    List<UserModel> queryOutput = connection.Query<UserModel>(queryString, new DynamicParameters()).ToList();

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectAllUsers():{e.Message}");
                return null;
            }
        }
        static public UserModel SelectUser(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {UserModel.DbName} " +
                                    $"WHERE {UserModel.DbIdCol}={id}";
                    UserModel queryOutput = connection.QuerySingleOrDefault<UserModel>(queryString, new DynamicParameters());

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

                    UserModel queryOutput = connection.QuerySingleOrDefault<UserModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectUser(string, string):{e.Message}");
                return null;
            }
        }
        static public UserModel SelectUserByLogin(string login)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {UserModel.DbName} " +
                                    $"WHERE {UserModel.DbLoginCol}='{login}'";

                    UserModel queryOutput = connection.QuerySingleOrDefault<UserModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectUserByLogin(string):{e.Message}");
                return null;
            }
        }
        static public SystemRoleModel SelectSystemRole(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {SystemRoleModel.DbTableName} " +
                                    $"WHERE {SystemRoleModel.DbIdCol}={id}";
                    SystemRoleModel queryOutput = connection.QuerySingleOrDefault<SystemRoleModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectSystemRole(uint):{e.Message}");
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
                                    $"INTO {UserModel.DbName} " +
                                    $"({UserModel.DbNameCol},{UserModel.DbLoginCol},{UserModel.DbPasswordCol}) " +
                                    $"VALUES (@{nameof(UserModel.Name)}," +
                                    $"@{nameof(UserModel.Login)}," +
                                    $"@{nameof(UserModel.Password)})";
                    connection.Execute(queryString, user);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertUser(UserModel):{e.Message}");
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

        #region Update

        public static void UpdateUser(UserModel user)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"UPDATE {UserModel.DbName} " +
                                    $"SET {UserModel.DbNameCol} = @{nameof(UserModel.Name)}, " +
                                    $"{UserModel.DbLoginCol} = @{nameof(UserModel.Login)}, " +
                                    $"{UserModel.DbPasswordCol} = @{nameof(UserModel.Password)} " +
                                    $"WHERE {UserModel.DbIdCol} = @{nameof(UserModel.Id)}";
                    connection.Execute(queryString, user);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateUser(UserModel):{e.Message}");
            }
        }

        #endregion

        #region Delete

        public static void DeleteUser(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"DELETE " +
                                    $"FROM {UserModel.DbName} " +
                                    $"WHERE {UserModel.DbIdCol} = {id}";
                    connection.Execute(queryString);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"DeleteUser(uint):{e.Message}");
            }
        }

        #endregion
    }
}
