using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Respository.Interfaces
{
    public interface IUserTokenRepository : IRepositoryAsync<UserToken>
    {
        Task<int> CreateUserTokenAsync(UserToken userToken);
        Task<int> UpdateUserTokenAsync(UserToken userToken);
        Task<UserToken> GetByTokenAsync(string token);
    }
}
