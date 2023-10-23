using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace PersonalLibrary.Models
{
    public class GenreEnum
    {
        private int? _genreId;

        public int? GenreId
        {
            get => _genreId;
            set => SetGenreId(value);
        }
        private string _genreName;
        public string GenreName
        {
            get => _genreName;
        }

        /// <summary>
        /// Set Genre ID based on the correct Genre to be tested 
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetGenreId(int? value)
        {
            bool genreIsDefined = Enum.IsDefined(typeof(GenreTypes), value);
            if (genreIsDefined)
            {
                _genreId = value;
                _genreName = Enum.GetName(typeof(GenreTypes), value);
            }
            else
            {
                throw new ArgumentException("The Genre Is Invalid");
            }

        }
       
    }

    //You can use enums to add the id taht you want to test without having to call to the DB, then add the ID to the Customer Model as the Genre Table is constant
    //You would need to mantain the type of genre in 2 places, but you can have less calls to the db 
    //I did it this way to showcase the power of enums, and if NO MORE genres are added to the DB
    public enum GenreTypes :int 
    {
        Sience_Fiction = 1,
        Mystery = 2,
        Fantasy = 3,
        Romance = 4

    }
}