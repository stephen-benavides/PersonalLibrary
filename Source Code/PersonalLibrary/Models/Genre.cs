using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalLibrary.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }

        public bool IsSelected { get; set; }
    }
}