﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using TimPlan.Models;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;
using TimPlan.Interfaces;

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

            SqlMapper.SetTypeMap(typeof(TeamRoleModel),
                new CustomPropertyTypeMap(typeof(TeamRoleModel),
                (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));

        }

        #region Select

        static public List<T> SelectAll<T>(string dbTableName)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    if (!typeof(IDbRecord).IsAssignableFrom(typeof(T)))
                    {
                        throw new ArgumentException($"{nameof(T)} must implement IDbRecord interface.");
                    }

                    string queryString = $"SELECT * " +
                                    $"FROM {dbTableName} ";
                    List<T> queryOutput = connection.Query<T>(queryString, new DynamicParameters()).ToList();

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectAll<T>(string):{e.Message}");
                return null;
            }
        }
        static public T SelectSingle<T>(string dbTableName, uint id)
        {
            

            try
            {
                if (!typeof(IDbRecord).IsAssignableFrom(typeof(T)))
                {
                    throw new ArgumentException($"{nameof(T)} must implement IDbRecord interface.");
                }

                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {dbTableName} " +
                                    $"WHERE id={id}";
                    T queryOutput = connection.QuerySingleOrDefault<T>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectSingle<{nameof(T)}>(string, uint):{e.Message}");
                return default(T);
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
        static public List<TaskModel> SelectAllTasksWithoutForeignPrivate(uint userId)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {TaskModel.DbTableName} " +
                                    $"WHERE NOT ({TaskModel.DbCreatorUserIdCol} <> {userId} " +
                                    $"AND {TaskModel.DbPrivateCol} = true)";
                    List<TaskModel> queryOutput = connection.Query<TaskModel>(queryString, new DynamicParameters()).ToList();

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectAllTasksWithoutForeignPrivate(uint):{e.Message}");
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
        static public TeamRoleModel SelectTeamRoleByName(string roleName)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {TeamRoleModel.DbTableName} " +
                                    $"WHERE {TeamRoleModel.DbNameCol}='{roleName}'";

                    TeamRoleModel queryOutput = connection.QuerySingleOrDefault<TeamRoleModel>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectTeamRoleByName(string):{e.Message}");
                return null;
            }
        }


        #endregion

        #region Insert

        static public bool InsertUser(UserModel user)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"INSERT " +
                                    $"INTO {UserModel.DbTableName} " +
                                    $"({UserModel.DbNameCol}," +
                                    $"{UserModel.DbLoginCol}," +
                                    $"{UserModel.DbPasswordCol}," +
                                    $"{UserModel.DbSystemRoleIdCol}," +
                                    $"{UserModel.DbTeamId}," +
                                    $"{UserModel.DbTeamRoleId}) " +
                                    $"VALUES (@{nameof(UserModel.Name)}," +
                                    $"@{nameof(UserModel.Login)}," +
                                    $"@{nameof(UserModel.Password)}," +
                                    $"@{nameof(UserModel.SystemRoleId)}," +
                                    $"@{nameof(UserModel.TeamId)}," +
                                    $"@{nameof(UserModel.TeamRoleId)})";
                    connection.Execute(queryString, user);

                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertUser(UserModel):{e.Message}");
                return false;
            }
        }
        static public bool InsertTeam(TeamModel team)
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

                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertTeam(TeamModel):{e.Message}");
                return false;
            }
        }
        static public bool InsertTask(TaskModel user)
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
                                    $"{TaskModel.DbCreatorUserIdCol}," +
                                    $"{TaskModel.DbUserIdCol}," +
                                    $"{TaskModel.DbTeamIdCol}) " +
                                    $"VALUES (@{nameof(TaskModel.Name)}," +
                                    $"@{nameof(TaskModel.ParentTaskID)}," +
                                    $"@{nameof(TaskModel.DateCreated)}," +
                                    $"@{nameof(TaskModel.DateStart)}," +
                                    $"@{nameof(TaskModel.DateEnd)}," +
                                    $"@{nameof(TaskModel.Description)}," +
                                    $"@{nameof(TaskModel.IsCompleted)}," +
                                    $"@{nameof(TaskModel.Private)}," +
                                    $"@{nameof(TaskModel.CreatorUserId)}," +
                                    $"@{nameof(TaskModel.UserId)}," +
                                    $"@{nameof(TaskModel.TeamId)})";
                    connection.Execute(queryString, user);

                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertUser(UserModel):{e.Message}");
                return false;
            }
        }
        static public bool InsertTeamRole(TeamRoleModel teamRole)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"INSERT " +
                                    $"INTO {TeamRoleModel.DbTableName} " +
                                    $"({TeamRoleModel.DbNameCol}), " +
                                    $"({TeamRoleModel.DbCanEditAllTasks}), " +
                                    $"({TeamRoleModel.DbCanEditCreatedTasks}), " +
                                    $"({TeamRoleModel.DbCanAssignTasks}), " +
                                    $"({TeamRoleModel.DbCanViewForeignTasks}), " +
                                    $"({TeamRoleModel.DbCanViewAllTasks})" +
                                    $"VALUES (@{nameof(TeamRoleModel.Name)}, " +
                                    $"@{nameof(TeamRoleModel.CanEditAllTasks)}, " +
                                    $"@{nameof(TeamRoleModel.CanEditCreatedTasks)}, " +
                                    $"@{nameof(TeamRoleModel.CanAssignTasks)}, " +
                                    $"@{nameof(TeamRoleModel.CanViewForeignTasks)}, " +
                                    $"@{nameof(TeamRoleModel.CanViewAllTasks)})";
                    connection.Execute(queryString, teamRole);

                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"TeamRoleModel(TeamRoleModel):{e.Message}");
                return false;
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
                                    $"{UserModel.DbPasswordCol} = @{nameof(UserModel.Password)}, " +
                                    $"{UserModel.DbSystemRoleIdCol} = @{nameof(UserModel.SystemRoleId)}, " +
                                    $"{UserModel.DbTeamId} = @{nameof(UserModel.TeamId)}, " +
                                    $"{UserModel.DbTeamRoleId} = @{nameof(UserModel.TeamRoleId)} " +
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
                                    $"SET {TeamModel.DbNameCol} = @{nameof(TeamModel.Name)} "+
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
                                    $"SET {TaskModel.DbNameCol} = @{nameof(TaskModel.Name)}, " +
                                    $"{TaskModel.DbParentTaskIdCol} = @{nameof(TaskModel.ParentTaskID)}, " +
                                    $"{TaskModel.DbDateCreatedCol} = @{nameof(TaskModel.DateCreated)}, " +
                                    $"{TaskModel.DbDateStartCol} = @{nameof(TaskModel.DateStart)}, " +
                                    $"{TaskModel.DbDateEndCol} = @{nameof(TaskModel.DateEnd)}, " +
                                    $"{TaskModel.DbDescriptionCol} = @{nameof(TaskModel.Description)}, " +
                                    $"{TaskModel.DbIsCompletedCol} = @{nameof(TaskModel.IsCompleted)}, " +
                                    $"{TaskModel.DbPrivateCol} = @{nameof(TaskModel.Private)}, " +
                                    $"{TaskModel.DbCreatorUserIdCol} = @{nameof(TaskModel.CreatorUserId)}, " +
                                    $"{TaskModel.DbUserIdCol} = @{nameof(TaskModel.UserId)}, " +
                                    $"{TaskModel.DbTeamIdCol} = @{nameof(TaskModel.TeamId)} " +
                                    $"WHERE {TaskModel.DbIdCol} = @{nameof(TaskModel.Id)}";
                    connection.Execute(queryString, task);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateTask(TaskModel):{e.Message}");
            }
        }
        static public void UpdateTeamRole(TeamRoleModel teamRole)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"UPDATE {TeamRoleModel.DbTableName} " +
                                    $"SET {TeamRoleModel.DbNameCol} = @{nameof(TeamRoleModel.Name)}, " +
                                    $"{TeamRoleModel.DbCanEditAllTasks} = @{nameof(TeamRoleModel.CanEditAllTasks)}, " +
                                    $"{TeamRoleModel.DbCanEditCreatedTasks} = @{nameof(TeamRoleModel.CanEditCreatedTasks)}, " +
                                    $"{TeamRoleModel.DbCanAssignTasks} = @{nameof(TeamRoleModel.CanAssignTasks)}, " +
                                    $"{TeamRoleModel.DbCanViewForeignTasks} = @{nameof(TeamRoleModel.CanViewForeignTasks)}, " +
                                    $"{TeamRoleModel.DbCanViewAllTasks} = @{nameof(TeamRoleModel.CanViewAllTasks)} " +
                                    $"WHERE {TeamRoleModel.DbIdCol} = @{nameof(TeamRoleModel.Id)}";
                    connection.Execute(queryString, teamRole);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateTeamRole(TeamRoleModel):{e.Message}");
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
        static public void DeleteTeamRole(uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"DELETE " +
                                    $"FROM {TeamRoleModel.DbTableName} " +
                                    $"WHERE {TeamRoleModel.DbIdCol} = {id}";
                    connection.Execute(queryString);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"DeleteTeamRole(uint):{e.Message}");
            }
        }
        #endregion
    }
}
