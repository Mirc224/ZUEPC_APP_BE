using AutoMapper;
using DataAccess.Models.User;
using Users.Base.Application.Domain;
using Users.Base.Domain;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Auth.Domain;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPublication;

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
		CreateMap<ImportPublication, PublicationModel>();
		CreateMap<ImportPublicationNameDetails, PublicationNameModel>();
	}
}
