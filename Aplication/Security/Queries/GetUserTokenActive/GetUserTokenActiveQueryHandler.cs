using Common.Base;
using Domain.Models;
using Infraestructure.UnitOfWork;
using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Security.Queries.GetUserTokenActive
{
    public class GetUserTokenActiveQueryHandler : IRequestHandler<GetUserTokenActiveQuery, Common.Models.BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserTokenActiveQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Common.Models.BaseResponse> Handle(GetUserTokenActiveQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Common.Models.BaseResponse response;

                var userPerson = await _unitOfWork.UserTokenRepository.GetByTokenAsync(request.Token);

                if (userPerson == null)
                {

                    response = new Common.Models.BaseResponse(HttpStatusCode.BadRequest, new BaseResponse<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Payload = null,
                        Message = "El token de usuario no está activo"
                    });

                    return response;
                }

                //Response
                response = new Common.Models.BaseResponse(HttpStatusCode.OK, new BaseResponse<UserToken>
                {
                    Code = (int)HttpStatusCode.OK,
                    Payload = userPerson
                });
                return response;


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}