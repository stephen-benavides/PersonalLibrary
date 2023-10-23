using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;

namespace PersonalLibrary.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        private int? _publicationYear;
        [Display(Name = "Year Of Publication")]
        public int? PublicationYear
        {
            get => _publicationYear;
            set => _publicationYear = value ?? 0; //ternary operator for null values 
        }

        public List<int> GenresId { get; set; }
        
    }



}