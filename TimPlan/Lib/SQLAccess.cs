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
            { typeof(UserModel), typeof(TeamModel), typeof(TeamRoleModel), typeof(TaskModel), typeof(SystemRoleModel),
            typeof(TaskTypeModel)};

        static public void MapClassAttributes()
        {
            foreach (Type classToMap in ClassesToMapAttributes)
            {
                SqlMapper.SetTypeMap(classToMap,
                new CustomPropertyTypeMap(classToMap,
                (type, columnName) => type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
            }
        }

        #region Select

        static public IEnumerable<T> SelectAll<T>() where T : DbModelBase, new()
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();
                    string tableName = new T().DbTableName;

                    string queryString = $"SELECT * " +
                                    $"FROM {tableName} ";
                    IEnumerable<T> queryResult = connection.Query<T>(queryString, new DynamicParameters());
                    return queryResult;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SQLAccess:SelectAllRecords<{nameof(T)}>():{ex.Message}");
                /*
                var box = MessageBoxManager.GetMessageBoxStandard("Błąd",
                    $"SQLAccess:SelectAllRecords():\n{ex.Message}", MsBox.Avalonia.Enums.ButtonEnum.Ok);
                box.ShowWindowAsync();
                */
                return null;
            }
        }
        static public T SelectSingle<T>(int id) where T : DbModelBase, new()
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();

                    string tableName = new T().DbTableName;

                    string queryString = $"SELECT * " +
                                    $"FROM {tableName} " +
                                    $"WHERE id={id}";
                    T queryOutput = connection.QuerySingleOrDefault<T>(queryString, new DynamicParameters());

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectSingle<{nameof(T)}>():{e.Message}");
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
                                    $"FROM {new UserModel().DbTableName} " +
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
                                    $"FROM {new UserModel().DbTableName} " +
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
                                    $"FROM {new TeamModel().DbTableName} " +
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
        static public List<TaskModel> SelectAllTasksWithoutForeignPrivate(int userId)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {new TaskModel().DbTableName} " +
                                    $"WHERE NOT ({TaskModel.DbCreatorUserIdCol} <> {userId} " +
                                    $"AND {TaskModel.DbPrivateCol} = true)";
                    List<TaskModel> queryOutput = connection.Query<TaskModel>(queryString, new DynamicParameters()).ToList();

                    return queryOutput;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectAllTasksWithoutForeignPrivate(int):{e.Message}");
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
                                    $"FROM {new TeamRoleModel().DbTableName} " +
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
        static public IEnumerable<TaskModel> SelectUserTasks(int userId, bool includePrivate = false)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {new TaskModel().DbTableName} " +
                                    $"WHERE {TaskModel.DbUserIdCol}='{userId}' ";
                    if (!includePrivate)
                        queryString += $"AND {TaskModel.DbPrivateCol}=0 ";

                    return connection.Query<TaskModel>(queryString, new DynamicParameters());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"SelectUserTasks(int):{e.Message}");
                return null;
            }
        }
        static public IEnumerable<UserModel> SelectTeamMembers(int teamId)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    string queryString = $"SELECT * " +
                                    $"FROM {new UserModel().DbTableName} " +
                                    $"WHERE {UserModel.DbTeamId}={teamId}";

                    return connection.Query<UserModel>(queryString, new DynamicParameters());
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
        static public bool InsertSingle<T>(T insertModel) where T : DbModelBase
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();

                    List<PropertyInfo> properties = AnnotationHelper.GetPropertiesWithColumnAnnotation(typeof(T));
                    StringBuilder queryColNames = new StringBuilder();

                    // Get Properties ColumnAnnotation except ID column
                    queryColNames.AppendJoin(',', properties.Where(p => p.Name != nameof(DbModelBase.Id))
                                                            .Select(p => AnnotationHelper.GetColumnAnnotationValue(p)));

                    // Get Properties Values except ID column
                    StringBuilder queryValues = new StringBuilder();
                    queryValues.AppendJoin(',', properties.Where(p => p.Name != nameof(DbModelBase.Id))
                                                            .Select(p => "@" + p.Name));

                    string query = $"INSERT INTO {insertModel.DbTableName} " +
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

        static public bool UpdateSingle<T>(T updateModel) where T : DbModelBase
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();

                    List<PropertyInfo> properties = AnnotationHelper.GetPropertiesWithColumnAnnotation(typeof(T));
                    StringBuilder querySet = new StringBuilder();

                    // Get Properties ColumnAnnotation except ID column
                    querySet.AppendJoin(',', properties.Where(p => p.Name != nameof(DbModelBase.Id))
                                                        .Select(p => $"{AnnotationHelper.GetColumnAnnotationValue(p)}" +
                                                                    $"=@{p.Name}"));
                    string query = $"UPDATE {updateModel.DbTableName} " +
                                    $"SET {querySet} " +
                                    $"WHERE {DbModelBase.DbIdCol}=@{nameof(DbModelBase.Id)}";

                    connection.Execute(query, updateModel);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateSingle<{nameof(T)}>(DbRecordBase):{e.Message}");
                return false;
            }
        }
        static public bool UpdateTaskState(int taskID, TaskModel.TaskState newState)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();

                    var parameters = new { NewState = newState };

                    string query = $"UPDATE {new TaskModel().DbTableName} " +
                                    $"SET {TaskModel.DbStateCol}=@NewState " +
                                    $"WHERE {DbModelBase.DbIdCol}={taskID}";

                    connection.Execute(query, parameters);

                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"UpdateTaskState(int, TaskState):{e.Message}");
                return false;
            }
        }
        #endregion

        #region Delete

        static public bool DeleteSingle<T>(int id) where T : DbModelBase, new()
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(_ConnectionString))
                {
                    connection.Open();

                    string tableName = new T().DbTableName;

                    string queryString = $"DELETE " +
                                    $"FROM {tableName} " +
                                    $"WHERE id = {id}";
                    connection.Execute(queryString);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"DeleteUser<{nameof(T)}>(int):{e.Message}");
                return false;
            }
        }

        #endregion
    }
}
