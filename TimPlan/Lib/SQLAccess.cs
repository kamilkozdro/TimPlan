using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using TimPlan.Models;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimPlan.Lib
{
    static public class SQLAccess
    {

        static private string _ConnectionString = "Data Source=.\\TimPlanDB.db;Version=3;";

        static public void MapClassAttributes()
        {
            SqlMapper.SetTypeMap(typeof(UserModel),
                new CustomPropertyTypeMap(typeof(UserModel),
                (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            
            SqlMapper.SetTypeMap(typeof(TeamModel),
                new CustomPropertyTypeMap(typeof(TeamModel),
                (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            
            SqlMapper.SetTypeMap(typeof(SystemRoleModel),
                new CustomPropertyTypeMap(typeof(SystemRoleModel),
                (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            
            SqlMapper.SetTypeMap(typeof(TaskModel),
                new CustomPropertyTypeMap(typeof(TaskModel),
                (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));

        }

        #region Select

        static public List<UserModel> SelectAllUsers()
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {UserModel.DbTableName} ";
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
                                    $"FROM {UserModel.DbTableName} " +
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
                                    $"FROM {UserModel.DbTableName} " +
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
                                    $"FROM {UserModel.DbTableName} " +
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
        static public List<TeamModel> SelectAllTeams()
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {TeamModel.DbTableName} ";
                    List<TeamModel> queryOutput = connection.Query<TeamModel>(queryString, new DynamicParameters()).ToList();

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectAllTeams():{e.Message}");
                return null;
            }
        }
        static public TeamModel SelectTeam(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {TeamModel.DbTableName} " +
                                    $"WHERE {TeamModel.DbIdCol}={id}";
                    TeamModel queryOutput = connection.QuerySingleOrDefault<TeamModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectTeam(uint):{e.Message}");
                return null;
            }
        }
        static public TeamModel SelectTeamByName(string name)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {TeamModel.DbTableName} " +
                                    $"WHERE {TeamModel.DbNameCol}='{name}'";

                    TeamModel queryOutput = connection.QuerySingleOrDefault<TeamModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectTeamByName(string):{e.Message}");
                return null;
            }
        }
        static public List<TaskModel> SelectAllTask()
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {TaskModel.DbTableName} ";
                    List<TaskModel> queryOutput = connection.Query<TaskModel>(queryString, new DynamicParameters()).ToList();

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectAllTask():{e.Message}");
                return null;
            }
        }
        static public TaskModel SelectTask(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {TaskModel.DbTableName} " +
                                    $"WHERE {TaskModel.DbIdCol}={id}";
                    TaskModel queryOutput = connection.QuerySingleOrDefault<TaskModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectTask(uint):{e.Message}");
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

        static public void InsertUser(UserModel user)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"INSERT " +
                                    $"INTO {UserModel.DbTableName} " +
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
        static public void InsertTeam(TeamModel team)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"INSERT " +
                                    $"INTO {TeamModel.DbTableName} " +
                                    $"({TeamModel.DbNameCol}) " +
                                    $"VALUES (@{nameof(TeamModel.Name)})";
                    connection.Execute(queryString, team);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertTeam(TeamModel):{e.Message}");
            }
        }
        static public void InsertTask(TaskModel user)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"INSERT " +
                                    $"INTO {TaskModel.DbTableName} " +
                                    $"({TaskModel.DbNameCol}," +
                                    $"{TaskModel.DbParentTaskIdCol}," +
                                    $"{TaskModel.DbDateCreatedCol}," +
                                    $"{TaskModel.DbDateStartCol}," +
                                    $"{TaskModel.DbDateEndCol}," +
                                    $"{TaskModel.DbDescriptionCol}," +
                                    $"{TaskModel.DbIsCompletedCol}," +
                                    $"{TaskModel.DbPrivateCol}," +
                                    $"{TaskModel.DbCreatorUserIdCol}) " +
                                    $"VALUES (@{nameof(TaskModel.Name)}," +
                                    $"@{nameof(TaskModel.ParentTaskID)}," +
                                    $"@{nameof(TaskModel.DateCreated)}," +
                                    $"@{nameof(TaskModel.DateStart)}," +
                                    $"@{nameof(TaskModel.DateEnd)}," +
                                    $"@{nameof(TaskModel.Description)}," +
                                    $"@{nameof(TaskModel.IsCompleted)}," +
                                    $"@{nameof(TaskModel.Private)}," +
                                    $"@{nameof(TaskModel.CreatorUserId)})";
                    connection.Execute(queryString, user);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertUser(UserModel):{e.Message}");
            }
        }

        #endregion

        #region Update

        static public void UpdateUser(UserModel user)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"UPDATE {UserModel.DbTableName} " +
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
        static public void UpdateTeam(TeamModel team)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"UPDATE {TeamModel.DbTableName} " +
                                    $"SET {TeamModel.DbNameCol} = @{nameof(TeamModel.Name)}, "+
                                    $"WHERE {TeamModel.DbIdCol} = @{nameof(TeamModel.Id)}";
                    connection.Execute(queryString, team);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateTeam(TeamModel):{e.Message}");
            }
        }
        static public void UpdateTask(TaskModel task)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"UPDATE {TaskModel.DbTableName} " +
                                    $"SET {TaskModel.DbNameCol} = @{nameof(TaskModel.Name)}," +
                                    $"{TaskModel.DbParentTaskIdCol} = @{nameof(TaskModel.ParentTaskID)}," +
                                    $"{TaskModel.DbDateCreatedCol} = @{nameof(TaskModel.DateCreated)}," +
                                    $"{TaskModel.DbDateStartCol} = @{nameof(TaskModel.DateStart)}," +
                                    $"{TaskModel.DbDateEndCol} = @{nameof(TaskModel.DateEnd)}," +
                                    $"{TaskModel.DbDescriptionCol} = @{nameof(TaskModel.Description)}," +
                                    $"{TaskModel.DbIsCompletedCol} = @{nameof(TaskModel.IsCompleted)}," +
                                    $"{TaskModel.DbPrivateCol} = @{nameof(TaskModel.Private)}," +
                                    $"{TaskModel.DbCreatorUserIdCol} = @{nameof(TaskModel.CreatorUserId)}" +
                                    $"WHERE {TaskModel.DbIdCol} = @{nameof(TaskModel.Id)}";
                    connection.Execute(queryString, task);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateTask(TaskModel):{e.Message}");
            }
        }
        #endregion

        #region Delete

        static public void DeleteUser(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"DELETE " +
                                    $"FROM {UserModel.DbTableName} " +
                                    $"WHERE {UserModel.DbIdCol} = {id}";
                    connection.Execute(queryString);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"DeleteUser(uint):{e.Message}");
            }
        }
        static public void DeleteTeam(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"DELETE " +
                                    $"FROM {TeamModel.DbTableName} " +
                                    $"WHERE {TeamModel.DbIdCol} = {id}";
                    connection.Execute(queryString);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"DeleteTeam(uint):{e.Message}");
            }
        }
        static public void DeleteTask(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"DELETE " +
                                    $"FROM {TaskModel.DbTableName} " +
                                    $"WHERE {TaskModel.DbIdCol} = {id}";
                    connection.Execute(queryString);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"DeleteTask(uint):{e.Message}");
            }
        }
        #endregion
    }
}
