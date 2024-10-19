
using Common.Models;
using Models.Security.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxies.SecurityProxy
{
    public interface ISecurityProxy
    {
        Task<BaseResponse> GetUserPersonByCredential(GetUserPersonByCredentialQuery request);
        Task<bool> GetTokenActive(LogoutUserPersonQuery req);
    }
}
