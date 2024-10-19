using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Security.Api
{
    public class AuthorizePolicy : AuthorizationHandler<AuthorizePolicy>, IAuthorizationRequirement
    {

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizePolicy requirement)
        {
            var expires = Convert.ToDateTime(context.User?.FindFirst(c => c.Type == "expires").Value);

            if (!context.User.HasClaim(c => c.Type == "idUser"))
            {
                context.Fail();
                return;
            }
            else if (DateTime.Now > expires)
            {
                context.Fail();
                return;
            }
            else
            {
                var idUser = Convert.ToInt32(context.User.FindFirst(c => c.Type == "idUser").Value);

                if (idUser <= 0)
                {
                    context.Fail();
                    return;
                }


                context.Succeed(requirement);
                return;
            }
        }
    }
}
