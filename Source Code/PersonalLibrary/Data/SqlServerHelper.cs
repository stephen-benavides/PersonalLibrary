using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc.Html;

namespace PersonalLibrary.Data
{
    public class SqlServerHelper
    {
        private readonly SqlConnection _connection;
        public SqlServerHelper()
        {
            //Set The connection string 
           // _connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
           _connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ClsConstants.DefaultConnection].ConnectionString);
        }

        public void OpenConnection()
        {
                if (_connection.State == ConnectionState.Closed) 
                    _connection.Open();
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            //The connection is reusable, by closing it, we can re open it during the same stream
            //If the connection is dispose, then the state is reset which throws an exception if the same connection string is called within the the same method 
            //_connection.Dispose();

        }


        /// <summary>
        /// Method to execute a stored procedure 
        /// </summary>
        /// <param name="storedProcedure">GenreName of the stored procedure</param>
        /// <param name="parameterList"><k,V> pair, for output parameters begin the key name with "@OUT_"</param>
        /// <returns></returns>
        public DataSet ExecuteSp(string storedProcedure, Hashtable parameterList)
        {
            DataSet resultSet = new DataSet();
            int index = 0;
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, _connection))
                {
                    //_connection.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    foreach (string key in parameterList.Keys)
                    {
                        //Protect Against SQL Injection 
                        sqlCommand.Parameters.AddWithValue(key, parameterList[key]);
                        if (key.StartsWith("@OUT_"))
                            sqlCommand.Parameters[index].Direction = ParameterDirection.Output;

                        sqlCommand.Parameters[index].Direction = ParameterDirection.Input;
                        index++;
                    }

                    new SqlDataAdapter(sqlCommand).Fill(resultSet);
                    sqlCommand.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            
            return resultSet;
        }

        
        public DataSet ExecuteQuery(string cmdText, Hashtable parameterList)
        {
            DataSet result = new DataSet();
            int index = 0;
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(cmdText, _connection))
                {
                    sqlCommand.CommandType = CommandType.Text;

                    foreach (string key in parameterList.Keys)
                    {
                        //Protect Against SQL Injection 
                        sqlCommand.Parameters.AddWithValue(key, parameterList[key]);
                        if (key.StartsWith("@OUT_"))
                            sqlCommand.Parameters[index].Direction = ParameterDirection.Output;

                        sqlCommand.Parameters[index].Direction = ParameterDirection.Input;
                        ++index;

                    }

                    new SqlDataAdapter(sqlCommand).Fill(result);                       
                    sqlCommand.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            return result;
        }

        #region Insert a dictionaty to a table using reflection, does not work for MSSql
        /// <summary>
        /// Inserts into the SP a List of elements
        /// </summary>
        /// <param name="storedProcedure">Stored Procedure to be executed</param>
        /// <param name="key">Name of the key to insert the list of values, always put the @ before inserting the name of the key</param>
        /// <param name="inputList">list of elements to be added directly to the list associated with the key</param>
        /// <returns></returns>
        private int InsertListIntoSP(string storedProcedure, string key, Dictionary<int, List<int>> inputList)
        {
            int rowsAffected = 0;
            DataSet resultSet = new DataSet();
            try
            {
                var dataTable = ConvertListToDataTable(inputList);
                if (dataTable != null)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, _connection))
                    {
                        sqlCommand.Parameters.AddWithValue(key, dataTable);
                        sqlCommand.Parameters[0].Direction = ParameterDirection.Input;
                        rowsAffected = sqlCommand.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return rowsAffected;
        }

        /// <summary>
        /// Convert a list of elements into a data table 
        /// </summary>
        /// <param name="dictionaryInput"></param>
        /// <returns></returns>
        public DataTable ConvertListToDataTable(Dictionary<int, List<int>> dictionaryInput)
        {

            DataTable result = new DataTable();

            if (dictionaryInput == null || dictionaryInput.Count == 0)
                return null;

            result.Columns.Add("BookId", typeof(int));
            result.Columns.Add("GenreId", typeof(int));

            /*foreach (var dictionaryEntry in dictionaryInput)
            {
            //Use this with the columns to set the type of the column at runtime using reflection 
                //Type objectTypeKey = dictionaryEntry.Key.GetType();
                result.Columns.Add(dictionaryEntry.Key.ToString(), typeof(int));
            }*/

            List<int> lstGenreIds = dictionaryInput.SingleOrDefault().Value;
            int bookId = dictionaryInput.SingleOrDefault().Key;

            foreach (var genreId in lstGenreIds)
            {
                result.Rows.Add(bookId, genreId);

            }
            return result;
        }


        #endregion

        private int InsertJsonIntoTable(string json)
        {
            int rowsAffected = 0;
            string storedProcedure = "stepsp_common_InsertBookGenre";

            using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, _connection))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@jsonDataTable", json);
                sqlCommand.Parameters[0].Direction = ParameterDirection.Input;

                _connection.Open();
                rowsAffected = sqlCommand.ExecuteNonQuery();
                _connection.Close();
            }


            return rowsAffected;
        }

        /// <summary>
        /// Add Data from a JSON string to a SQL Table 
        /// </summary>
        /// <param name="json"></param>
        public int AddJsonToTable(string storedProcedure, Hashtable jsonParameters)
        {
            int rowsAffected = 0;
            int index = 0;
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, _connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    
                    foreach (string key in jsonParameters.Keys)
                    {
                        sqlCommand.Parameters.AddWithValue(key, jsonParameters[key]);
                        if(key.StartsWith("@OUT_"))
                            sqlCommand.Parameters[index].Direction = ParameterDirection.Output;
                        sqlCommand.Parameters[index].Direction = ParameterDirection.Input;
                        ++index;
                    }
                    OpenConnection();
                    rowsAffected = sqlCommand.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }

            return rowsAffected;
        }


    }
}