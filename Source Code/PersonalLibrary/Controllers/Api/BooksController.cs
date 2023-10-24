using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PersonalLibrary.Data.Repositories;
using PersonalLibrary.Models;
using PersonalLibrary.Services;

namespace PersonalLibrary.Controllers.Api
{
    public class BooksController : ApiController
    {
        private BookServices services;

        public BooksController()
        {
            services = new BookServices(new BookRepository());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


        // GET: api/Books
        [Route("api/books")]
        [HttpGet]
        public IHttpActionResult GetAllBooks()
        {
            return Ok(services.GetAllBooks());
        }

        // GET: api/Books/5
        [Route("api/books/{id}")]
        [HttpGet]
        public IHttpActionResult GetBook(int id)
        {
            var getBook = services.GetBookById(id);

            if (getBook == null)
                return BadRequest("Does Not Exist in DB");

            return Ok(getBook);
        }

        // POST: api/Books
        [HttpPost]
        [Route("api/books/{id}")]
        public IHttpActionResult Post([FromBody]Book book)
        {
            services.AddNewBook(book);
            return Ok("New book has been added");
        }
        //PUT :api/Books
        [HttpPut]
        [Route("api/books/{id}")]
        //This URL IS INVOKED as FOLLOWS: http://localhost:62689/api/books/Put?bookId=13 and the actual book in the body of the JSON request
        public IHttpActionResult Put(int bookId, [FromBody] Book book)
        {
            var getBook = services.GetBookById(bookId);
            if (getBook == null)
                return BadRequest("The book you are trying to update does not exist");

            var updatedBook = services.UpdateBook(book);
            return Ok(updatedBook);
        }

        // DELETE: api/Books/5
        [HttpDelete]
        [Route("api/books/{id}")]
        public IHttpActionResult DeleteBook(int id)
        {
            var bookToDelete = services.GetBookById(id);
            if (bookToDelete == null)
                return NotFound();
            services.DeleteBook(id);
            return Ok(Request.RequestUri+"///BOOK DELETED");
        }
    }
}
