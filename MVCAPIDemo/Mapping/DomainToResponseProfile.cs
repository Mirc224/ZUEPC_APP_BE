using AutoMapper;
using DataAccess.Models;
using MVCAPIDemo.Application.Commands.Users;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Mapping;

public class DomainToResponseProfile: Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<UserModel, User>()
			.ReverseMap();
        CreateMap<RegisterUserCommand, UserModel>();
		CreateMap<RoleModel, Role>();
	}
}
