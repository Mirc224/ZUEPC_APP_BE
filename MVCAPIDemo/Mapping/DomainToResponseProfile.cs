using AutoMapper;
using DataAccess.Models;
using Users.Base.Application.Domain;
using Users.Base.Domain;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Auth.Domain;

namespace ZUEPC.Application.Mapping;

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
