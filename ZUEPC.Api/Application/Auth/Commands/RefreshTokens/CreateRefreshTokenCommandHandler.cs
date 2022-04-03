using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Users.Base.Domain;

namespace ZUEPC.Api.Application.Auth.Commands.RefreshTokens;

public class CreateRefreshTokenCommandHandler :
	IRequestHandler<CreateRefreshTokenCommand, CreateRefreshTokenCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IRefreshTokenData _repository;

	public CreateRefreshTokenCommandHandler(IMapper mapper, IRefreshTokenData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreateRefreshTokenCommandResponse> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
	{
		RefreshTokenModel insertModel = _mapper.Map<RefreshTokenModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		long insertedId = await _repository.InsertModelAsync(insertModel);
		RefreshToken domain = _mapper.Map<RefreshToken>(insertModel);
		return new() { Success = true, Data = domain };
	}
}
