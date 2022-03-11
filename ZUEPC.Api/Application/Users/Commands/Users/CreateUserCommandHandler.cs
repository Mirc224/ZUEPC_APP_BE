using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using System.Security.Cryptography;
using Users.Base.Domain;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Users.Commands.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IUserData _repository;

	public CreateUserCommandHandler(IMapper mapper, IUserData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	
	public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		byte[]? passwordHash = null;
		byte[]? passwordSalt = null;

		using (HMACSHA512 hmac = new())
		{
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
		}

		UserModel newUserModel = _mapper.Map<UserModel>(request);
		newUserModel.CreatedAt = DateTime.UtcNow;
		newUserModel.PasswordHash = passwordHash;
		newUserModel.PasswordSalt = passwordSalt;

		long newUserId = await _repository.InsertModelAsync(newUserModel);
		newUserModel.Id = newUserId;
		User mappedResult = _mapper.Map<User>(newUserModel);
		return new() { Success = true, Data = mappedResult };
	}
}
