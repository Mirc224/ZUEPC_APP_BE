using AutoMapper;
using Dapper;
using DataAccess.Data.User;
using DataAccess.Enums;
using DataAccess.Models;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Localization;
using Newtonsoft.Json.Linq;
using Users.Base.Domain;

namespace MVCAPIDemo.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
{
	private readonly IUserData _repository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<DataAnnotations> _localizer;

	public UpdateUserCommandHandler(
		IMapper mapper,
		IUserData repository,
		IStringLocalizer<DataAnnotations> localizer)
	{
		_repository = repository;
		_mapper = mapper;
		_localizer = localizer;
	}

	public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{

		var userModel = await _repository.GetUserByIdAsync(request.UserId);
		if (userModel is null)
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer["UserNotFound"].Value },
				Success = false
			};
		}
		
		var user = _mapper.Map<User>(userModel);
		var origUserJSON = JObject.FromObject(user);
		user.Roles = (await _repository.GetUserRolesAsync(userModel.Id)).Select(x => x.Id).ToList();
		
		var actualRoles = user.Roles.ToHashSet();
		request.AppliedPatch.ApplyTo(user);

		var modifUserJSON = JObject.FromObject(user);

		var changedProperties = new List<string>();
		foreach(var x in origUserJSON)
		{
			if(!JToken.DeepEquals(x.Value, modifUserJSON[x.Key]))
			{
				changedProperties.Add(x.Key);
			}
		}


		if(changedProperties.Contains("Roles"))
		{
			changedProperties.Remove("Roles");
			var newUserRoles = user.Roles.ToHashSet();
			newUserRoles.Add(RoleType.USER);

			var addedRoles = newUserRoles.Except(actualRoles);
			var removedRoles = actualRoles.Except(newUserRoles);

			foreach (var role in addedRoles)
			{
				await _repository.InsertUserRoleAsync(user.Id, role);
			}

			foreach (var role in removedRoles)
			{
				await _repository.DeleteUserRoleAsync(user.Id, role);
			}
		}

		if(changedProperties.Count > 0)
		{
			var moddifiedUserModel = _mapper.Map<UserModel>(user);
			await _repository.UpdateUserAsync(moddifiedUserModel);
		}

		return new UpdateUserCommandResponse() { Success = true };
	}
}
