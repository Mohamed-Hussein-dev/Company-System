using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly CompanyDbContext _dbContext;

        public GenaricRepository(CompanyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(T Entity)
        {
            _dbContext.Add(Entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(T Entity)
        {
            _dbContext.Remove(Entity);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)_dbContext.Employees.Include(E => E.Department).ToList();
            }
            return _dbContext.Set<T>().ToList();
        }

        public T? GetById(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return _dbContext.Employees.Include(E => E.Department).Where(E => E.Id == id).FirstOrDefault() as T;
            }
            //else{return _dbContext.Departments.Include(D => D.Employees).Where(E => E.Id == id).FirstOrDefault() as T;}
            return _dbContext.Set<T>().Find(id);
        }

        public int Update(T Entity)
        {
            _dbContext.Update(Entity);
            return _dbContext.SaveChanges();
        }
    }
}
