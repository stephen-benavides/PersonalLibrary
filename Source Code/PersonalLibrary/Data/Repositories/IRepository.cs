using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalLibrary.Data.Repositories
{
    public interface IRepository <T> where T : class
    {
        T GetById(int id);
        void Add(T element);
        T Update(T element);
        void DeleteById(int id);
        List<T> GetAll();
    }
}