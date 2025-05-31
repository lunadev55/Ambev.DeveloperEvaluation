using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class GetUserByIdRequestProfile : Profile
    {
        public GetUserByIdRequestProfile()
        {
            CreateMap<GetUserRequest, GetUserCommand>();
        }
    }
}
