using Infraestructure.Respository.Implementation;
using Infraestructure.Respository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(string connectionString)
        {
            UserPersonRepository = new UserPersonRepository(connectionString);
            UserTokenRepository = new UserTokenRepository(connectionString);
        }
        public IUserPersonRepository UserPersonRepository { get; private set; }
        public IUserTokenRepository UserTokenRepository { get; }
    }
}
