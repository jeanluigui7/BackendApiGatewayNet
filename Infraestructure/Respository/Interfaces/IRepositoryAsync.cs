using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Respository.Interfaces
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<bool> DeleteAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<int> InsertAsync(T entity);
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetByIdAsync(Int64 id);
    }
}
