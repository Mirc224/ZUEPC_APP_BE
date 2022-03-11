using AutoMapper;
using DataAccess.Data.User;
using ZUEPC.DataAccess.Enums;
using MediatR;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using Users.Base.Domain;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Localization;

namespace ZUEPC.Application.Users.Commands;

public class PatchUserCommandHandler : IRequestHandler<PatchUserCommand, PatchUserCommandResponse>
{
	private readonly IUserData _repository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<DataAnnotations> _localizer;

	public PatchUserCommandHandler(
		IMapper mapper,
		IUserData repository,
		IStringLocalizer<DataAnnotations> localizer)
	{
		_repository = repository;
		_mapper = mapper;
		_localizer = localizer;
	}

	public async Task<PatchUserCommandResponse> Handle(PatchUserCommand request, CancellationToken cancellationToken)
	{

		//UserModel? userModel = await _repository.GetUserByIdAsync(request.UserId);
		//if (userModel is null)
		//{
		//	return new()
		//	{
		//		ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.USER_NOT_EXIST].Value },
		//		Success = false
		//	};
		//}

		//User user = _mapper.Map<User>(userModel);
		//JObject origUserJSON = JObject.FromObject(user);
		//user.Roles = (await _repository.GetUserRolesAsync(userModel.Id)).Select(x => x.Id).ToList();

		//HashSet<RoleType> actualRoles = user.Roles.ToHashSet();
		//request.AppliedPatch.ApplyTo(user);

		//JObject modifUserJSON = JObject.FromObject(user);

		//List<string> changedProperties = new();
		//foreach (KeyValuePair<string, JToken> x in origUserJSON)
		//{
		//	if (!JToken.DeepEquals(x.Value, modifUserJSON[x.Key]))
		//	{
		//		changedProperties.Add(x.Key);
		//	}
		//}


		//if (changedProperties.Contains("Roles"))
		//{
		//	changedProperties.Remove("Roles");
		//	HashSet<RoleType> newUserRoles = user.Roles.ToHashSet();
		//	newUserRoles.Add(RoleType.USER);

		//	IEnumerable<RoleType> addedRoles = newUserRoles.Except(actualRoles);
		//	IEnumerable<RoleType> removedRoles = actualRoles.Except(newUserRoles);

		//	foreach (RoleType role in addedRoles)
		//	{
		//		await _repository.InsertUserRoleAsync(user.Id, role);
		//	}

		//	foreach (RoleType role in removedRoles)
		//	{
		//		await _repository.DeleteUserRoleAsync(user.Id, role);
		//	}
		//}

		//if (changedProperties.Count > 0)
		//{
		//	UserModel moddifiedUserModel = _mapper.Map<UserModel>(user);
		//	await _repository.UpdateUserAsync(moddifiedUserModel);
		//}

		//return new PatchUserCommandResponse() { Success = true };
		return null;
	}
}
