using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PersonalLibrary.Data.Repositories;
using PersonalLibrary.Models;
using PersonalLibrary.Models.ViewModels;
using PersonalLibrary.Services;

namespace PersonalLibrary.Controllers
{
    public class BooksController : Controller
    {

        private BookServices services;
        private GenreServices genreServices;
        // GET: Book
        public BooksController()
        {
            services = new BookServices(new BookRepository());
            genreServices = new GenreServices(new GenreRepository());
        }
        public ActionResult Index()
        {
            var lstBook = services.GetAllBooks();
            return View(lstBook);
        }

        // GET: Book/Details/5
        public ActionResult Details(int bookId)
        {
            //Console.WriteLine(book);
            var bookReturned = services.GetBookById(bookId);
            return View(bookReturned);
        }

        /// <summary>
        /// Access a brand new form to set up the attributes for the form creation
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            //Add the model that we are going to be using for the form 
            //Passes a new objects that is going to be interacted by the view 
            NewBookFormViewModel viewModel = new NewBookFormViewModel();
            viewModel.GenreTypes = genreServices.GetAllGenres().ToList();
            
            return View("New", viewModel);
        }
        // POST: Book/Create
        //Populated from the New Action in the Book Controller 
        [HttpPost]
        public ActionResult Save(NewBookFormViewModel newBookFormView)
        {
            /*
            //Selecting both the GenreId and GenreName for testing purposes, can only be done by using an anonymous object which passes through reflection 
            var selectedGenres = newBookFormView.GenreTypes.Where(s => s.IsSelected)
                .Select(genreItem => new {GenreId = genreItem.GenreId, GenreName = genreItem.GenreName}).ToList();
            */
            //If the model state is invalid based on the attributes set on the model, then this return the same view again with the same values that were passed. This will trigger the Html.Validation Messages For to indicate the user the issues with the model
            if (!ModelState.IsValid)
                return View("New", newBookFormView);


            var selectedGenres = newBookFormView.GenreTypes.Where(s => s.IsSelected)
                .Select(genreItem => genreItem.GenreId).ToList();
            var newBook = new Book()
            {
                BookId = newBookFormView.Book.BookId, 
                Title = newBookFormView.Book.Title,
                Author = newBookFormView.Book.Author,
                PublicationYear = newBookFormView.Book.PublicationYear,
                GenresId = selectedGenres
            };
            
            return Json(newBook, JsonRequestBehavior.AllowGet);
        }

        // GET: Book/Edit/5
        
        [HttpGet]
        [Route("api2/edit")]
        public IEnumerable<string> Edit(int id)
        {
            //return View();
            return new List<string>() {"a,b,c,d", "1,2,3,4"};
        }

        // POST: Book/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
