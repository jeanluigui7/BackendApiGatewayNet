using Infraestructure.Respository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserPersonRepository UserPersonRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }

    }
}
