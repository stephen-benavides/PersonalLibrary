using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalLibrary.Data.Repositories;
using PersonalLibrary.Models;

namespace PersonalLibrary.Services
{
    public class GenreServices
    {
        GenreRepository genreRepository;

        public GenreServices(GenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        public Genre GetGenreById(int id)
        {
            return genreRepository.GetById(id);
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return genreRepository.GetAll();
        }

        public int AddGenresToBook(string json)
        {
            return genreRepository.AddGenresToBook(json);
        }
    }
}