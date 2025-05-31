using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class AuthenticateUserProfile : Profile
    {
        public AuthenticateUserProfile()
        {
            // Map application result to API response DTO
            CreateMap<AuthenticateUserResult, AuthenticateUserResponse>();
        }
    }
}
