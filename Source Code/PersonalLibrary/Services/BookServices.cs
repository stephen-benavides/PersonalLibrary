using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLibrary.Data.Repositories;
using PersonalLibrary.Models;

namespace PersonalLibrary.Services
{
    public class BookServices
    {
        #region Dependency

        private readonly IRepository<Book> _repository;

        #endregion

        #region ctor

        /// <summary>
        /// Initializes the repository Data Access Layer 
        /// </summary>
        /// <param name="repositorytory"></param>
        public BookServices(IRepository<Book> repository)
        {
            _repository = repository;
        }


        #endregion

        #region GetById

        /// <summary>
        /// Get A Specific book by its id
        /// </summary>
        /// <param name="id">id of the book</param>
        /// <returns></returns>
        public Book GetBookById(int id)
        {
            return _repository.GetById(id);
        }

        #endregion

        #region AddNewBook

        /// <summary>
        /// Add new book
        /// </summary>
        /// <param name="book">new book to be added</param>
        public void AddNewBook(Book book)
        {
            if (book.Author != "INVALIDAUTHOR")
                _repository.Add(book);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update an existing book 
        /// </summary>
        /// <param name="book">book to be updated</param>
        /// <returns></returns>
        public Book UpdateBook(Book book)
        {
            return _repository.Update(book);
        }

        #endregion

        #region DeleteBook

        /// <summary>
        /// Delete a book if it exists based on its id
        /// </summary>
        /// <param name="bookId">id of the book to be deleted</param>
        public void DeleteBook(int bookId)
        {
            _repository.DeleteById(bookId);
        }

        #endregion

        #region GetAll

        /// <summary>
        /// Get all the available books in the db 
        /// </summary>
        /// <returns></returns>
        public List<Book> GetAllBooks()
        {
            return _repository.GetAll();
        }

        #endregion
    }
}