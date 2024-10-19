
using Common.Encryptation;
using Common.JwtToken;
using Common.MapperDto;
using Common.Models;
using Common.Modules;
using Domain.Models;
using Infraestructure.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Aplication.Security.Queries.GetUserPersonByCredential
{
    public class GetUserPersonByCredentialQueryHandler : IRequestHandler<GetUserPersonByCredentialQuery, BaseResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        public GetUserPersonByCredentialQueryHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _configuration = configuration;
        }
        public async Task<BaseResponse> Handle(GetUserPersonByCredentialQuery request, CancellationToken cancellationToken)
        {
            try
            {
                BaseResponse response;

                var userPerson = await _unitOfWork.UserPersonRepository.LoginAsync(new UserPerson
                {
                    Email = request.Email,
                    Password = request.Password,
                    ModuleID = request.ModuleID
                });

                if (userPerson == null)
                {

                    response = new BaseResponse(HttpStatusCode.BadRequest, new EntityResponse<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Payload = null,
                        Message = "Correo y/o contraseña incorrectos. Inténtelo de nuevo"
                    });

                    return response;
                }
                bool valid = false;

                if (request.Auth)
                {
                    valid = true;
                }
                else
                {
                    valid = userPerson.Password.Equals(EncryptationAES.Encriptar(request.Password, _configuration["TokenPassword"]));//userPerson.Password.Equals(Encrypt.GetMD5(request.Password));
                }
                var getUserPersonDto = new GetUserPersonQueryDto
                {
                    userPerson = userPerson.MapTo<UserPersonDto>()
                };

                if (!valid)
                {
                    response = new BaseResponse(HttpStatusCode.BadRequest, new EntityResponse<object>
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Payload = null,
                        Message = "Usuario y/o contraseña son incorrectas."
                    });

                    return response;
                }
                //Create Token
                var tokenDto = await _tokenService.GetTokenNew(GetClaimUser(userPerson), _configuration["Jwt:RsaPrivateKey"], ModuleName.getModuleName(request.ModuleID), _configuration["Jwt:ExpirationMinutes"]);
                getUserPersonDto.tokenData = tokenDto;

                // Save Token Active
                await _unitOfWork.UserTokenRepository.CreateUserTokenAsync(new UserToken
                {
                    Token = tokenDto.token,
                    UserID = userPerson.ID,
                    StatusID = 1,
                    CreatedBy = userPerson.ID
                });

                //Response
                response = new BaseResponse(HttpStatusCode.OK, new EntityResponse<GetUserPersonQueryDto>
                {
                    Code = (int)HttpStatusCode.OK,
                    Payload = getUserPersonDto
                });
                return response;


            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<Claim> GetClaimUser(UserPerson userPerson)
        {
            return new List<Claim>
            {
                new Claim("ID", userPerson.ID.ToString()),
                new Claim("Email", userPerson.Email),
                new Claim("IsShopper", userPerson.IsShopper ? "1" : "0"),
            };
        }
    }
}
