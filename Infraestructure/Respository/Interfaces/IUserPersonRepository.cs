using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Respository.Interfaces
{
    public interface IUserPersonRepository : IRepositoryAsync<UserPerson>
    {
        Task<UserPerson> LoginAsync(UserPerson user);

    }
}
