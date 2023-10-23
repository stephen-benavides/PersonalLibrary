using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using NUnit.Framework;
using PersonalLibrary.Data.Repositories;
using PersonalLibrary.Models;
using PersonalLibrary.Services;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PersonalLibrary.Tests.Services
{
    [TestClass]
    public class BookServicesTest
    {

        private List<Book> _lstMockedBooks;


        [SetUp]
        public void SetUp()
        {
            #region List of Mocked Books

            _lstMockedBooks = new List<Book>()
            {
                new Book()
                {
                    BookId = 0,
                    Title = "Zero"
                },
                new Book()
                {
                    BookId = 1,
                    Title = "One"
                },
                new Book()
                {
                    BookId = 2,
                    Title = "Two"
                },
                new Book()
                {
                    BookId = 3,
                    Title = "Three"
                },


            };

            #endregion

        }

        #region BookServices_GetBookById_GetTheSpecifBookByTheID

        [Test]
        [Order(1)]
        public void BookService_GetAllBooks_GetAllBooksAvailableInTheRepository()
        {
            var mockedBookRepo = new Mock<IRepository<Book>>();

            mockedBookRepo.Setup(r => r.GetAll())
                .Returns(_lstMockedBooks);
            var bookServices = new BookServices(mockedBookRepo.Object);

            Assert.AreEqual(bookServices.GetAllBooks().Count, 4);
        }

        #endregion 

        #region BookServices_GetBookById_GetTheSpecifBookByTheID

        [TestCase(0, "Zero")]
        [Test]
        [Order(2)]
        public void BookServices_GetById_GetABookObjectByTheId(int input, string expected)
        {
            Mock<IRepository<Book>> mockedBookRepo = new Mock<IRepository<Book>>();
            mockedBookRepo.Setup(r => r.GetById(input)).Returns(_lstMockedBooks[input]);

            var bookService = new BookServices(mockedBookRepo.Object);
            var returnedBook = bookService.GetBookById(input);


            Assert.AreEqual(expected, returnedBook.Title);
            Assert.AreNotEqual("TWO", returnedBook.Title);


        }

        #endregion

        #region VOID_BookService_AddNewBook_AddBookObjectToRepository

        [Test]
        [Order(3)]
        public void BookService_AddNewBook_AddBookObjectToRepository()
        {
            //Mock the DB Dependency
            Mock<IRepository<Book>> mockedBookRepo = new Mock<IRepository<Book>>();
            //Create the book to be mocked
            var book = new Book()
            {
                BookId = 4,
                Title = "Mock",
                Author = "Moq",
                PublicationYear = null
            };
            var book2 = new Book()
            {
                BookId = 5,
                Title = "Mock2",
                Author = "Moq",
                PublicationYear = null
            };
            //Pass the mocked dependency into the service
            var bookServices = new BookServices(mockedBookRepo.Object);
            //Run the method
            bookServices.AddNewBook(book);
            bookServices.AddNewBook(book2);

            //Assert that it has been added 
            mockedBookRepo.Verify(repo => repo.Add(It.Is<Book>(b => b.Title == book.Title && b.BookId == 4)), Times.Exactly(1));

            mockedBookRepo.Verify(repo => repo.Add(It.Is<Book>(b => b.Title == book2.Title && b.BookId == 5)), Times.Exactly(1));


            /*
             * 'It' is a class that belong to Moq
             *      is used to assert the values within a method that is mocked 
             */
        }


    #endregion

        #region BookService_AddNewBook_CheckIfInvalidAuthorIsAddedToRepository
        [Test]
        public void BookService_AddNewBook_CheckIfInvalidAuthorIsAddedToRepository()
        {
            var book = new Book()
            {
                Title = "A",
                Author = "INVALIDAUTHOR", //Invalid Author set in the services layer 
                PublicationYear = 1111
            };

            Mock<IRepository<Book>> mockedRepository = new Mock<IRepository<Book>>();

            var bookService = new BookServices(mockedRepository.Object);
            bookService.AddNewBook(book);

            //Verify if the service adds an invalid author
            mockedRepository.Verify(repo => repo.Add(book), Times.Never);
        }


        #endregion

        #region BookService_UpdateBook_UpdateBookInTheRepository

        [Test]
        public void BookService_UpdateBook_UpdateBookInTheRepository()
        {
            //Create a Book to add in the repository
            var bookToAdd = new Book()
            {
                BookId = 5,
                Title = "The Fifth Book"
            };
            //Add the book into the mocked repository 
            Mock<IRepository<Book>> mockedRepository = new Mock<IRepository<Book>>();
            var bookService = new BookServices(mockedRepository.Object);
            bookService.AddNewBook(bookToAdd);
            
            mockedRepository.Verify(r => r.Add(It.Is<Book>(b => b.BookId == 5)), Times.Once);

            //update the book in the service 
            var bookToUpdate = new Book()
            {
                BookId = 5,
                Title = "The Updated Book"
            };
            bookService.UpdateBook(bookToUpdate);
            //Check the new updated book exists in the repository 
            mockedRepository.Verify(r => r.Update(It.Is<Book>(b => b.BookId == 5 && b.Title == bookToUpdate.Title)), Times.Once);

        }


        #endregion

        #region BookService_Delete_DeleteBookByIdInRepository
        [Test]
        public void BookService_Delete_DeleteBookByIdInRepository()
        {
            Mock<IRepository<Book>> mockRepository = new Mock<IRepository<Book>>();

            var bookService = new BookServices(mockRepository.Object);

            var bookToDelete = new Book()
            {
                BookId = 5,
                Title = "The Updated Book"
            };

            bookService.DeleteBook(bookToDelete.BookId);

            //Verify the method to remove the object was called with the right id 
            //mockRepository.Verify(r => r.DeleteById(6),Times.Once);
            //Another way to test, assert through a SetUp to indicate if the right book was deleted from the repository
            mockRepository.Setup(r => r.DeleteById(It.IsAny<int>())).Callback<int>(deletedBook =>
            {
                Assert.AreEqual(5, bookToDelete.BookId);
            });
            //Need to call the service again for the setup to take place
            bookService.DeleteBook(bookToDelete.BookId);
        }

        #endregion


        [Test]
        public void GETALLFBOOKS()
        {
            BookServices services = new BookServices(new BookRepository());
            foreach (var book in services.GetAllBooks())
            {
                Console.WriteLine(book.Title);
            }
            Assert.AreEqual("", "");
        }
    }
}
