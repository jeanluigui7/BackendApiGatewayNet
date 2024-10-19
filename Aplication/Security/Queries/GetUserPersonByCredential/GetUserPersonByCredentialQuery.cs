using Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aplication.Security.Queries.GetUserPersonByCredential
{
    public class GetUserPersonByCredentialQuery : IRequest<BaseResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int ModuleID { get; set; }
        public bool Auth { get; set; }
    }
}