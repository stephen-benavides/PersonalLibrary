using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PersonalLibrary.Models;

namespace PersonalLibrary.Data.Repositories
{
    public class GenreRepository : IRepository<Genre>
    {
        private SqlServerHelper _sqlServerHelper;
        public GenreRepository()
        {
            _sqlServerHelper = new SqlServerHelper();
        }
        public Genre GetById(int id)
        {
            Genre genre = new Genre();
            Hashtable inpputParameters = new Hashtable();
            inpputParameters["GenreId"] = id;
            string cmdText = "SELECT * FROM Genre WHERE GenreId = @GenreId";
            try
            {
                _sqlServerHelper.OpenConnection();
                var resultSet = _sqlServerHelper.ExecuteQuery(cmdText, inpputParameters);

                genre.GenreId = (int) resultSet.Tables[0].Rows[0]["GenreId"];
                genre.GenreName = (string) resultSet.Tables[0].Rows[0]["GenreName"];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                _sqlServerHelper.CloseConnection();
            }

            return genre;
        }

        public void Add(Genre element)
        {
            throw new NotImplementedException();
        }

        public Genre Update(Genre element)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Genre> GetAll()
        {
            List<Genre> lstGenre = new List<Genre>();
            Hashtable inpputParameters = new Hashtable();
            string cmdText = "SELECT * FROM Genre";
            try
            {
                _sqlServerHelper.OpenConnection();
                var resultSet = _sqlServerHelper.ExecuteQuery(cmdText, inpputParameters);

                foreach (DataRow dataRow in resultSet.Tables[0].Rows)
                {
                    lstGenre.Add(new Genre()
                    {
                        GenreId = (int) dataRow["GenreId"],
                        GenreName = (string) dataRow["GenreName"]

                    });   
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                _sqlServerHelper.CloseConnection();
            }

            return lstGenre;
        }
        /// <summary>
        /// Add Genres In JSON format that have been mapped to a Book
        /// </summary>
        /// <param name="json">JSON of BookId + GenresId Mapped</param>
        /// <returns></returns>
        public int AddGenresToBook(string json)
        {
            string storedProc = "stepsp_common_InsertBookGenre";
            Hashtable parameters = new Hashtable();
            parameters["@jsonDataTable"] = json;

            return _sqlServerHelper.AddJsonToTable(storedProc, parameters);
        }
    }
}