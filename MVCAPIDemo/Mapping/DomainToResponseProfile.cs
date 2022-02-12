using AutoMapper;
using DataAccess.Models;
using MVCAPIDemo.Auth.Commands;
using MVCAPIDemo.Auth.Domain;
using Users.Base.Application.Domain;
using Users.Base.Domain;

namespace MVCAPIDemo.Application.Mapping;

public class DomainToResponseProfile: Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<UserModel, User>()
			.ReverseMap();
        CreateMap<RegisterUserCommand, UserModel>();
		CreateMap<RoleModel, Role>();
		CreateMap<AuthResult, LoginUserCommandResponse>();
		CreateMap<AuthResult, RefreshTokenCommandResponse>();
		CreateMap<RevokeResult, RevokeTokenCommandResponse>();
		CreateMap<RevokeResult, LogoutUserCommandResponse>();
	}
}
