using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenaricRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        int Delete(T Entity);
        int Update(T Entity);
        int Add(T Entity);
    }
}
