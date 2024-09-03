using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenaricRepository<Employee> , IEmployeeRepository
    {
        private readonly CompanyDbContext dbContext;

        public EmployeeRepository(CompanyDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Employee> GetByAdress(string adress)
        {
            return dbContext.Employees.Where(E => E.Address ==  adress);
        }

        public IQueryable<Employee> GetByName(string name)
        {
            return dbContext.Employees.Where(E => E.Name.Contains(name));
        }
    }
}
