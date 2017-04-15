using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GroupLinQ
{
    public class DalHelper : IDisposable
    {
        #region Property

        private readonly string _connectionString = "server=TUNV_KLOONPC;database=TestSQL;uid=sa;pwd=123;";

        private DataContext _dataContext;

        private bool _hasTransaction;

        private DataContext DataContext
        {
            get { return _dataContext ?? (_dataContext = new DataContext(_connectionString)); }
        }

        public Exception Exception { get; set; }

        public bool IsSuccess { get; set; }

        #endregion

        #region Constructor

        public DalHelper()
        {
            IsSuccess = true;
        }

        public DalHelper(string connectionString)
        {
            _connectionString = connectionString;
            IsSuccess = true;
        }

        #endregion


        #region Transaction and connection

        public void OpenConnection()
        {
            if (DataContext.Connection.State == ConnectionState.Closed)
                _dataContext.Connection.Open();
        }

        public void CloseConnection()
        {
            if (DataContext.Connection.State == ConnectionState.Open)
                _dataContext.Connection.Close();
        }

        public void BeginTransaction()
        {
            OpenConnection();
            if (_dataContext.Transaction != null) return;
            _dataContext.Transaction = _dataContext.Connection.BeginTransaction();
            _hasTransaction = true;
        }

        private void RollbackTransaction()
        {
            //namnh temp
            //throw Exception;
            //end
            if (_dataContext.Transaction == null)
                return;
            _dataContext.Transaction.Rollback();
            _dataContext.Transaction.Dispose();
            _dataContext.Transaction = null;
            CloseConnection();
        }

        public void CommitTransaction()
        {
            _hasTransaction = false;

            if (_dataContext.Transaction == null)
                return;
            try
            {
                _dataContext.Transaction.Commit();
            }
            catch (Exception ex)
            {
                _dataContext.Transaction.Rollback();
                Exception = ex;
                IsSuccess = false;
            }
            finally
            {
                _dataContext.Transaction.Dispose();
                _dataContext.Transaction = null;
                CloseConnection();
            }
        }

        private bool IsTransactionError()
        {
            return !IsSuccess && _dataContext != null && _hasTransaction && _dataContext.Transaction == null;
        }

        #endregion

        #region Command and Query

        /// <summary>
        /// Execute sql query and return List type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> ExecuteQueryToList<T>(string query, params object[] parameters)
        {
            if (IsTransactionError())
                return null;
            try
            {
                return DataContext.ExecuteQuery<T>(query, parameters).ToList();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return null;
            }
        }

        public T ExecuteQuerySingle<T>(string query, params object[] parameters) where T : new()
        {
            if (IsTransactionError())
                return new T();
            try
            {
                parameters = PatchNullParams(ref query, parameters);
                return DataContext.ExecuteQuery<T>(query, parameters).Single();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return new T();
            }
        }

        public T ExecuteQuerySingleOrDefault<T>(string query, params object[] parameters) where T : new()
        {
            if (IsTransactionError())
                return new T();
            try
            {
                return DataContext.ExecuteQuery<T>(query, parameters).SingleOrDefault();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return new T();
            }
        }

        /// <summary>
        /// Execute sql query contains @ParamName and result is List of type T
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public List<T> ExecuteSqlQueryToList<T>(string query, params SqlParameter[] sqlParameters)
        {
            if (IsTransactionError())
                return null;
            try
            {
                var dbCmd = DataContext.Connection.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = query;
                dbCmd.Parameters.AddRange(sqlParameters);
                OpenConnection();
                var dr = dbCmd.ExecuteReader();
                return _dataContext.Translate<T>(dr).ToList();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return null;
            }
            finally
            {
                if (_dataContext.Transaction == null)
                    CloseConnection();
            }
        }

        public T ExecuteSqlQuerySingle<T>(string query, params SqlParameter[] sqlParameters) where T : new()
        {
            if (IsTransactionError())
                return new T();
            try
            {
                var dbCmd = DataContext.Connection.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = query;
                dbCmd.Parameters.AddRange(sqlParameters);
                OpenConnection();
                var dr = dbCmd.ExecuteReader();
                return _dataContext.Translate<T>(dr).Single();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return new T();
            }
            finally
            {
                if (_dataContext.Transaction == null)
                    CloseConnection();
            }
        }

        public T ExecuteSqlQuerySingleOrDefault<T>(string query, params SqlParameter[] sqlParameters) where T : new()
        {
            if (IsTransactionError())
                return new T();
            try
            {
                var dbCmd = DataContext.Connection.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = query;
                dbCmd.Parameters.AddRange(sqlParameters);
                OpenConnection();

                var dr = dbCmd.ExecuteReader();
                return _dataContext.Translate<T>(dr).SingleOrDefault();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return new T();
            }
            finally
            {
                if (_dataContext.Transaction == null)
                    CloseConnection();
            }
        }

        /// <summary>
        /// Execute command sql
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteCommand(string command, params object[] parameters)
        {
            if (IsTransactionError())
                return -1;
            try
            {
                parameters = PatchNullParams(ref command, parameters);

                return DataContext.ExecuteCommand(command, parameters);
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return -1;
            }
        }

        /// <summary>
        /// Execute command sql
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string query, params SqlParameter[] sqlParameters)
        {
            if (IsTransactionError())
                return -1;
            try
            {
                var dbCmd = _dataContext.Connection.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = query;
                dbCmd.Parameters.AddRange(sqlParameters);
                dbCmd.Transaction = _dataContext.Transaction;
                OpenConnection();
                return dbCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return -1;
            }
            finally
            {
                if (_dataContext.Transaction == null)
                    CloseConnection();
            }
        }

        #endregion

        #region Excute query to DataTable

        public DataTable ExecuteQueryToDataTable<T>(string query, params object[] parameters)
        {
            if (IsTransactionError())
                return null;
            try
            {
                var enumerable = DataContext.ExecuteQuery<T>(query, parameters);
                return enumerable.ToDataTable();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return null;
            }
        }

        /// <summary>
        /// Execute sql query and result is DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryToDataTable(string query, params object[] parameters)
        {
            if (IsTransactionError())
                return null;
            var paramObjs = parameters.Select((t, index) => new { Name = "@p" + index, Value = t }).ToList();
            query = string.Format(query, paramObjs.Select(t => t.Name).ToArray());
            try
            {
                var dbCmd = DataContext.Connection.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = query;
                var sqlParams = paramObjs.Select(t => new SqlParameter(t.Name, t.Value)).ToArray();
                dbCmd.Parameters.AddRange(sqlParams);
                using (var da = new SqlDataAdapter((SqlCommand)dbCmd))
                {
                    using (var dt = new DataTable())
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return null;
            }
        }

        /// <summary>
        /// Execute sql query and result is DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryToDataTable(string query)
        {
            if (IsTransactionError())
                return null;
            try
            {
                var dbCmd = DataContext.Connection.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = query;

                using (var da = new SqlDataAdapter((SqlCommand)dbCmd))
                {
                    using (var dt = new DataTable())
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return null;
            }
        }

        /// <summary>
        /// Execute sql query contains @ParamName and result is DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryToDataTable(string query, Dictionary<string, object> dicParams)
        {
            if (IsTransactionError())
                return null;
            try
            {
                var dbCmd = DataContext.Connection.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = query;

                var sqlParams = dicParams.Select(t => new SqlParameter(t.Key, t.Value)).ToArray();
                dbCmd.Parameters.AddRange(sqlParams);

                using (var da = new SqlDataAdapter((SqlCommand)dbCmd))
                {
                    using (var dt = new DataTable())
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                Exception = ex;
                IsSuccess = false;
                return null;
            }
        }

        #endregion

        private static object[] PatchNullParams(ref string command, params object[] parameters)
        {
            var indexes = parameters.Select((t, index) => new { t, index })
                .Where(t => t.t == null)
                .Select(t => t.index)
                .OrderByDescending(t => t);
            var count = indexes.Count();
            if (count > 0)
            {
                var previousIndex = parameters.Length;
                foreach (var index in indexes)
                {
                    command = command.Replace("{" + index + "}", "NULL");

                    var j = 1;
                    for (var i = previousIndex - 1; i > index; i--)
                    {
                        command = command.Replace("{" + (index + j) + "}", "{." + (index + j - count) + "}");
                        j++;
                    }
                    count--;
                    previousIndex = index;
                }
                command = command.Replace("{.", "{");
                parameters = parameters.Where(t => t != null).ToArray();
                command = command.Replace("{.", "{");
            }
            return parameters;
        }

        public void Dispose()
        {
            _dataContext = null;
        }
    }

    public static class Extensions
    {
        /// <summary>
        /// Convert into DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            var dt = new DataTable();
            var t = typeof(T);
            var pia = t.GetProperties();
            //Create the columns in the DataTable
            foreach (var pi in pia)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }
            //Populate the table
            foreach (var item in collection)
            {
                var dr = dt.NewRow();
                dr.BeginEdit();
                foreach (var pi in pia)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}