using Common.Models;
using MediatR;

namespace Aplication.Security.Queries.GetUserTokenActive
{
    public class GetUserTokenActiveQuery : IRequest<BaseResponse>
    {
        public string Token { get; set; }
    }
}
