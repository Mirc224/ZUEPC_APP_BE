using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Users.Domain;

namespace ZUEPC.Api.Application.Auth.Commands.RefreshTokens;

public class UpdateRefreshTokenCommandHandler :
	IRequestHandler<UpdateRefreshTokenCommand, UpdateRefreshTokenCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IRefreshTokenData _repository;

	public UpdateRefreshTokenCommandHandler(IMapper mapper, IRefreshTokenData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdateRefreshTokenCommandResponse> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
	{
		RefreshTokenModel? currentModel = await _repository.GetRefreshTokenByTokenAsync(request.Token);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		RefreshTokenModel updatedModel = currentModel;
		_mapper.Map(request, updatedModel);

		int rowsUpdated = await _repository.UpdateModelAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
