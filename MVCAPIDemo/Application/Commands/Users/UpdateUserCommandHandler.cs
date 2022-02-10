using AutoMapper;
using DataAccess.Data.User;
using DataAccess.Enums;
using DataAccess.Models;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Localization;
using Newtonsoft.Json.Linq;

namespace MVCAPIDemo.Application.Commands.Users;

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
		var userModel = await _repository.GetUserById(request.UserId);
		if (userModel is null)
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer["UserNotFound"].Value },
				Success = false
			};
		}
		
		User user = _mapper.Map<User>(userModel);
		var origUserJSON = JObject.FromObject(user);

		user.Roles = (await _repository.GetUserRoles(userModel.Id)).Select(x => x.Id).ToList();
		var actualRoles = user.Roles.ToHashSet();
		request.AppliedPatch.ApplyTo(user);

		var modifUserJSON = JObject.FromObject(user);

		List<string> changedProperties = new List<string>();
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
			newUserRoles.Add(RolesType.USER);

			var addedRoles = newUserRoles.Except(actualRoles);
			var removedRoles = actualRoles.Except(newUserRoles);

			foreach (var role in addedRoles)
			{
				await _repository.InsertUserRole(user.Id, role);
			}

			foreach (var role in removedRoles)
			{
				await _repository.DeleteUserRole(user.Id, role);
			}
		}

		if(changedProperties.Count > 0)
		{
			var moddifiedUserModel = _mapper.Map<UserModel>(user);
			await _repository.UpdateUser(moddifiedUserModel);
		}

		return new UpdateUserCommandResponse() { Success = true };
	}
}
