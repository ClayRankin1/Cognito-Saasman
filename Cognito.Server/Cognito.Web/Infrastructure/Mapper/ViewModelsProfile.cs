using AutoMapper;
using Cognito.DataAccess.Entities;
using Cognito.Web.Services.Security.Models;
using Cognito.Web.ViewModels;
using Cognito.Web.ViewModels.Authentication;
using System.Linq;

namespace Cognito.Web.Infrastructure.Mapper
{
    public class ViewModelsProfile : Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.DomainId, opt => opt.MapFrom(src => src.UserDomains.Select(du => du.DomainId).FirstOrDefault()));

            CreateMap<Tokens, TokensViewModels>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken.Token));
        }
    }
}
