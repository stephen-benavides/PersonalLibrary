using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using PersonalLibrary.Data.Repositories;
using PersonalLibrary.Models;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PersonalLibrary.Tests.Data.Repositories
{
    [TestClass]
    public class GenreRepositoryTests
    {
        private IRepository<Genre> _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new GenreRepository();
        }

        [Test]
        public void GetById_ReturnGenreBasedOnId()
        {
            var genre = _repository.GetById(2);
            Assert.AreEqual("Mystery", genre.GenreName);

        }

        //[NUnit.Framework.Ignore("DEBUG: Ignoring to avoid modifying DB")]
        [Test]
        [Order(1)]
        public void AddGenresToBook_AddDataIntoThe_BookAndGenre_Table()
        {
            BookRepository bookRepository = new BookRepository();

            string jsonFile = File.ReadAllText(@"D:\Users\Roshack\Desktop\PublicProjects\PersonalLibrary\Source Code\PersonalLibrary.Tests\Content\Client\BookIdGenreIdInsertionJSON.txt");

            //Desirialize the json string into a Dyamic JSON Object that exists at run time
            var desirializedJson = JsonConvert.DeserializeObject(jsonFile);

            int rowsAffected = bookRepository.AddGenresToBook(jsonFile);


            Assert.AreEqual(4, rowsAffected);
        }


    }
}
