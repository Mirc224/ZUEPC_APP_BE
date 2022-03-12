using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Auth.Commands.RefreshTokens;

public class UpdateRefreshTokenCommandHandler :
	UpdateSimpleModelCommandHandlerBase<RefreshTokenModel>,
	IRequestHandler<UpdateRefreshTokenCommand, UpdateRefreshTokenCommandResponse>
{

	public UpdateRefreshTokenCommandHandler(IMapper mapper, IRefreshTokenData repository)
		: base(mapper, repository) { }
	public async Task<UpdateRefreshTokenCommandResponse> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdateRefreshTokenCommand,
			UpdateRefreshTokenCommandResponse>(request);
	}
}
