using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using TimPlan.Models;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;

namespace TimPlan.Lib
{
    static public class SQLAccess
    {

        static private string _ConnectionString = "Data Source=.\\TimPlanDB.db;Version=3;";
        private static Type[] ClassesToMapAttributes =
            { typeof(UserModel), typeof(TeamModel), typeof(TeamRoleModel), typeof(TaskModel), typeof(SystemRoleModel) };

        static public void MapClassAttributes()
        {
            foreach (Type classToMap in ClassesToMapAttributes)
            {
                SqlMapper.SetTypeMap(classToMap,
                new CustomPropertyTypeMap(classToMap,
                (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            }
            /*

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
            */
        }

        #region Select

        static public List<T> SelectAll<T>(string dbTableName) where T : DbRecordBase<T>
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();
                    string queryString = $"SELECT * " +
                                    $"FROM {dbTableName} ";
                    List<T> queryResult = connection.Query<T>(queryString, new DynamicParameters()).ToList();
                    return queryResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SQLAccess:SelectAllRecords<{nameof(T)}>(string):{ex.Message}");
                /*
                var box = MessageBoxManager.GetMessageBoxStandard("Błąd",
                    $"SQLAccess:SelectAllRecords():\n{ex.Message}", MsBox.Avalonia.Enums.ButtonEnum.Ok);
                box.ShowWindowAsync();
                */
                return null;
            }
        }
        static public T SelectSingle<T>(string dbTableName, uint id) where T : DbRecordBase<T>
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();
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
        static public bool InsertSingle<T>(DbRecordBase<T> insertModel, string dbTableName)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();

                    List<PropertyInfo> properties = AnnotationHelper.GetPropertiesWithColumnAnnotation<T>();
                    StringBuilder queryColNames = new StringBuilder();

                    // TODO: Hardcoded id property name "Id" in LINQ Where statements
                    // Find better solution

                    queryColNames.AppendJoin(',', properties.Where(p => p.Name != "Id")
                                                            .Select(p => AnnotationHelper.GetColumnAnnotationValue(p)));

                    StringBuilder queryValues = new StringBuilder();
                    queryValues.AppendJoin(',', properties.Where(p => p.Name != "Id")
                                                            .Select(p => "@" + p.Name));

                    string query = $"INSERT INTO {dbTableName} " +
                                    $"({queryColNames}) " +
                                    $"VALUES ({queryValues})";

                    connection.Execute(query, insertModel);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"InsertSingle<{nameof(T)}>(DbRecordBase, string):{e.Message}");
                return false;
            }
        }
        #endregion

        #region Update

        static public bool UpdateSingle<T>(DbRecordBase<T> updateModel, string dbTableName)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();

                    List<PropertyInfo> properties = AnnotationHelper.GetPropertiesWithColumnAnnotation<T>();
                    StringBuilder querySet = new StringBuilder();

                    // TODO: Hardcoded id property name "Id" in LINQ Where statements
                    // Find better solution

                    querySet.AppendJoin(',', properties.Where(p => p.Name != "Id")
                                                        .Select(p => $"{AnnotationHelper.GetColumnAnnotationValue(p)}" +
                                                                    $"=@{p.Name}"));
                    string query = $"UPDATE {dbTableName} " +
                                    $"SET {querySet} " +
                                    $"WHERE {properties.Where(p => p.Name == "Id")
                                                        .Select(p => AnnotationHelper.GetColumnAnnotationValue(p) +
                                                                    "=@" + p.Name)
                                                        .FirstOrDefault()}";

                    connection.Execute(query, updateModel);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateSingle<{nameof(T)}>(DbRecordBase, string):{e.Message}");
                return false;
            }
        }

        #endregion

        #region Delete

        static public void DeleteSingle(string dbTableName, uint id)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"DELETE " +
                                    $"FROM {UserModel.DbTableName} " +
                                    $"WHERE id = {id}";
                    connection.Execute(queryString);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"DeleteUser(string, uint):{e.Message}");
            }
        }

        #endregion
    }
}
