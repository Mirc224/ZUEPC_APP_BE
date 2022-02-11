using AutoMapper;
using Dapper;
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
		var builder = new SqlBuilder();
		builder.Select("*");
		builder.Where("Id = @Id");
		var userModel = (await _repository.GetUsersAsync(new { Id = request.UserId}, builder)).FirstOrDefault();
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
		builder = new SqlBuilder();
		builder.Where("UserId = @UserId");
		user.Roles = (await _repository.GetUserRolesAsync(new { UserId = userModel.Id }, builder)).Select(x => x.Id).ToList();
		
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
				await _repository.InsertUserRoleAsync(user.Id, role);
			}

			foreach (var role in removedRoles)
			{
				builder = new SqlBuilder();
				builder.Where("UserId = @UserId");
				builder.Where("RoleId = @RoleId");
				await _repository.DeleteUserRoleAsync(new { UserId = user.Id, RoleId = role }, builder);
			}
		}

		if(changedProperties.Count > 0)
		{
			var moddifiedUserModel = _mapper.Map<UserModel>(user);
			await _repository.UpdateUserAsync(moddifiedUserModel, new());
		}

		return new UpdateUserCommandResponse() { Success = true };
	}
}
