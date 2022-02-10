using AutoMapper;
using DataAccess.Data.User;
using DataAccess.Models;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Commands.Users;

public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, UpdateUserRolesCommandResponse>
{
	private readonly IUserData _repository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<DataAnnotations> _localizer;

	public UpdateUserRolesCommandHandler(
		IMapper mapper,
		IUserData repository,
		IStringLocalizer<DataAnnotations> localizer)
	{
		_repository = repository;
		_mapper = mapper;
		_localizer = localizer;
	}

	public async Task<UpdateUserRolesCommandResponse> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
	{
		var userModel = await _repository.GetUserById(request.UserId);


		if (userModel is null)
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer["UserNotFound"].Value },
				Success = false
			};
		}
		userModel.Roles = new List<RoleModel>();
		var user = _mapper.Map<User>(userModel);
		var actualRoles = user.Roles.ToHashSet();

		request.AppliedPatch.ApplyTo(user);


		



		//var actualUserRoles = (await _repository.GetUserRoles(user.Id)).Select(x => x.Id).ToHashSet();

		//var newUserRoles = request.UserRoles.ToHashSet();
		//newUserRoles.Add(RolesType.USER);

		//var addedRoles = newUserRoles.Except(actualUserRoles);
		//var removedRoles = actualUserRoles.Except(newUserRoles);

		//foreach (var role in addedRoles)
		//{
		//	await _repository.InsertUserRole(user.Id, role);
		//}

		//foreach (var role in removedRoles)
		//{
		//	int tmp = await _repository.DeleteUserRole(user.Id, role);
		//}

		return new UpdateUserRolesCommandResponse() { Success = true };
	}
}
