using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PersonalLibrary.Models;

namespace PersonalLibrary.Data.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        #region sqlServer

        private readonly SqlServerHelper _sqlServerHelper = new SqlServerHelper();

        #endregion

        #region GetById

        public Book GetById(int id)
        {
            string steps_selectBookById = "SELECT * FROM Book WHERE BookId = @BookId";

            var book = new Book();

            var tableParams = new Hashtable();
            tableParams["@BookId"] = id;
            try
            {
                _sqlServerHelper.OpenConnection();
                var dsBook = _sqlServerHelper.ExecuteQuery(steps_selectBookById, tableParams);

                if (dsBook.Tables[0].Rows.Count == 0)
                    return null;

                foreach (DataRow dataRow in dsBook.Tables[0].Rows)
                {
                    book.BookId = (int) dataRow["BookId"];
                    book.Title = (string) dataRow["Title"];
                    book.Author = (string) dataRow["Author"];
                    book.PublicationYear = (int) dataRow["PublicationYear"];
                }

                dsBook.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            finally
            {
                _sqlServerHelper.CloseConnection();
            }

            return book;
        }

        #endregion

        #region Add

        public void Add(Book book)
        {
            //Command
            string stepsp_common_AddNewBook = "stepsp_common_AddNewBook";

            var parameters = new Hashtable();
            parameters.Add("@Title", book.Title);
            parameters.Add("@Author", book.Author);
            parameters.Add("@PublicationYear", book.PublicationYear);


            try
            {
                _sqlServerHelper.OpenConnection();
                var resultSet = _sqlServerHelper.ExecuteSp(stepsp_common_AddNewBook, parameters);
                if (resultSet.Tables.Count != 0)
                    Console.WriteLine("Issues When Adding a New Book: {0}", resultSet.Tables[0].Rows[0][0]);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            finally
            {
                _sqlServerHelper.CloseConnection();
            }
        }

        #endregion

        #region Update

        public Book Update(Book book)
        {
            var updatedBook = new Book();

            string stepsp_common_UpdateExistingBook = "stepsp_common_UpdateExistingBook";
            Hashtable parameters = new Hashtable();
            parameters.Add("@BookId", book.BookId);
            parameters.Add("@Title", book.Title);
            parameters.Add("@Author", book.Author);
            parameters.Add("@PublicationYear", book.PublicationYear);


            try
            {
                _sqlServerHelper.OpenConnection();
                var resultSet = _sqlServerHelper.ExecuteSp(stepsp_common_UpdateExistingBook, parameters);

                if (resultSet == null)
                    Console.WriteLine("No Book Was Updated In The DB");

                foreach (DataRow dataRow in resultSet.Tables[0].Rows)
                {
                    updatedBook.BookId = (int)dataRow["BookId"];
                    updatedBook.Title = (string) dataRow["Title"];
                    updatedBook.Author = (string)dataRow["Author"];
                    updatedBook.PublicationYear = (int) dataRow["PublicationYear"];
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

            return updatedBook;
        }

        #endregion

        #region DeleteById

        public void DeleteById(int id)
        {
            try
            {
                _sqlServerHelper.OpenConnection();

                Hashtable parameters = new Hashtable();
                parameters.Add("@bookId", id);

                _sqlServerHelper.ExecuteQuery("DELETE FROM Book WHERE BookId = @bookId", parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            finally
            {
                _sqlServerHelper.CloseConnection();
            }
        }

        #endregion

        #region GetAll

        public List<Book> GetAll()
        {
            List<Book> lstAllBooks = new List<Book>();


            try
            {
                _sqlServerHelper.OpenConnection();
                var resultSet = _sqlServerHelper.ExecuteQuery("SELECT * FROM Book", new Hashtable());

                if (resultSet == null)
                    Console.WriteLine("No Books Available");

                foreach (DataRow dataRow in resultSet.Tables[0].Rows)
                {
                    lstAllBooks.Add(new Book()
                    {
                        BookId = (int) dataRow["BookId"],
                        Title = (string) dataRow["Title"],
                        Author = (string) dataRow["Author"],
                        PublicationYear = (int) dataRow["PublicationYear"],
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            finally
            {
                _sqlServerHelper.CloseConnection();
            }

            return lstAllBooks;
        }

        #endregion

        #region AddIntoBookGenreRelationshipTable a List of elements

        /// <summary>
        /// Add into the Book and Genre table, that has a many to many relationship 
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="lstGenres"></param>
        /// <returns></returns>
        private int AddBookAndGenres(int bookId, List<Genre>lstGenres)
        {
            int rowsAffected = 0;
            try
            {
                _sqlServerHelper.OpenConnection();

                string storedProc = "stepsp_common_InsertBookGenre";
                string parameterKey = "@BookGenreList";

                // BookId: 2 , List<GenreIds>: 1,2,3,4,5
                Dictionary<int, List<int>> dataElements = new Dictionary<int, List<int>>();
                List<int> lstGenreIds = new List<int>();
                foreach (var genre in lstGenres)
                {
                    lstGenreIds.Add(genre.GenreId);
                }

                dataElements.Add(bookId, lstGenreIds);
                //DEBUG: Set the status of the InsertListIntoSPmethod in the server helper to public 
                //rowsAffected = _sqlServerHelper.InsertListIntoSP(storedProc, parameterKey, dataElements);
                return rowsAffected;
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
        }

        #endregion

        public int AddGenresToBook(string json)
        {
            string storedProc = "stepsp_common_InsertBookGenre";
            Hashtable parameters = new Hashtable();
            parameters["@jsonDataTable"] = json;

            return _sqlServerHelper.AddJsonToTable(storedProc, parameters);
        }
    }
}