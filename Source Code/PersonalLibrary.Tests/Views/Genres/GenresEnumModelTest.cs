using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NUnit.Framework;
using PersonalLibrary.Models;

namespace PersonalLibrary.Tests.Views.Genres
{
    [TestClass]
    public class GenresEnumModelTest
    {
        [Test]
        public void SetGenreId_GetTheGenreBasedOnTheId()
        {
            GenreEnum genre = new GenreEnum();
            genre.SetGenreId(4);

            Console.WriteLine("GENRE ID: {0}",genre.GenreId);
            Console.WriteLine("GENRE NAME: {0}",genre.GenreName);

        }
    }
}
