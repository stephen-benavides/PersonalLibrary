using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalLibrary.Models.ViewModels
{
    public class NewBookFormViewModel
    {
        public Book Book{ get; set; }
        public List<Genre> GenreTypes { get; set; }
    }
}