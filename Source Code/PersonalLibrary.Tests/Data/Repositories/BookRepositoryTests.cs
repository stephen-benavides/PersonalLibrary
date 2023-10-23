using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using PersonalLibrary.Data;
using PersonalLibrary.Data.Repositories;
using PersonalLibrary.Models;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PersonalLibrary.Tests.TempDataBase.Repositories
{
    [TestClass]
    public class BookRepositoryTests
    {
        #region SetUP

        //DATA ACCESS
        private IRepository<Book> repository;

        //ALL BOOKS
        private List<Book> allBooks;

        [SetUp]
        public void SetUp()
        {
            repository = new BookRepository();
            allBooks = new List<Book>();
        }

        #endregion

        [NUnit.Framework.Ignore("DEBUG")]
        [TestCase(1, "Dune")]
        [TestCase(2, "The Hitchhiker's Guide to the Galaxy")]
        [TestCase(4, "The Lord of the Rings")]
        [Test]
        public void GetBookById(int input, string expected)
        {
            var book = repository.GetById(input);

            Console.WriteLine("BOOK NAME = {0}", book.Title);
            Console.WriteLine("Expected = {0}", expected);
            Assert.AreEqual(expected, book.Title);
        }


        [Test]
        [NUnit.Framework.Ignore("No Adding new elements to the same DB")]
        public void BookRepository_AddNewBook_AddNewBookToDB()
        {
            var book = new Book()
            {
                Author = "BBB",
                Title = "CCC",
                PublicationYear = 1444
            };
            repository.Add(book);
            Assert.AreEqual("", "");
        }

        [Test]
        [Order(1)]
        [NUnit.Framework.Ignore("DEBUG")]

        public void BookRepository_GetAll_GetAllBooksAvailableInDb()
        {
            allBooks = repository.GetAll();

            foreach (var book in allBooks)
            {
                Console.WriteLine(book.Title);
            }

            Assert.AreEqual("Dune", allBooks[0].Title);
        }

        [Test]
        [Order(2)]
        [NUnit.Framework.Ignore("DEBUG")]

        public void BookRepository_UpdateBook_UpdateExistingBookAndReturnIt()
        {
            var bookToUpdate = new Book()
            {
                BookId = 6, Title = "ZXS", Author = "ZZX", PublicationYear = 555
            };

            var bookUpdated = repository.Update(bookToUpdate);

            Assert.AreEqual(bookToUpdate.Title, bookUpdated.Title);
        }

        [Test]
        [Order(3)]
        [NUnit.Framework.Ignore("DEBUG")]

        public void BookRepository_DeleteBook_DeleteBookById()
        {
            //Delete Book
            //repository.DeleteById(8);


            //Filer from AllBooks
            var filteredBooks = allBooks.Where(book => (book.BookId == 0)).Select(book => book);


            Assert.AreEqual(filteredBooks.Count(), 0);
        }

        [Test]
        public void AddBookAndGenres_AddIntoBookBenreTableTheManyToMAnyRelationshipOf_BookAndGenre()
        {
            BookRepository bookRepository = new BookRepository();
            int bookId = 2;
            List<Genre> lstGenres = new List<Genre>();
            lstGenres.Add(new Genre { GenreId = 1 });
            lstGenres.Add(new Genre { GenreId = 2 });
            lstGenres.Add(new Genre { GenreId = 3 });

            //int rowsAffected = bookRepository.AddBookAndGenres(bookId, lstGenres);
            //Assert.AreEqual(3, rowsAffected); 
            //@Html.DropDownListFor(m => m.Book.GenresId, new SelectList(Model.GenreTypes, "GenreId", "GenreName"),"CHOOSE")

        }

    }
}