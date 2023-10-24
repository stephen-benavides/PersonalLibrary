using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PersonalLibrary.Models.ViewModels
{
    public class NewBookFormViewModel
    {
        public Book Book{ get; set; }
        [DisplayName("Genres")]
        public List<Genre> GenreTypes { get; set; }

        //Personal Notes from the user to write about a specific book 
        //Every user can have their own description for a book they have in their personal library 
        //New version, to be included when implementing view for different users DEBUG
        //public string Description { get; set; }
    }
}