using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalLibrary.Models;

namespace PersonalLibrary.Data.Repositories
{
    public interface IBookRepository 
    {
        Book GetBookById(int id);
        void AddBook(Book book);
        Book UpdateBook(Book book);
        void DeleteBookById(int id);
        List<Book> GetAllBooks();

    }
}
