using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class UpdatePublicationExternDatabaseIdCommandHandler :
	UpdateModelWithIdCommandHandlerBase<IPublicationExternDatabaseIdData, PublicationExternDatabaseIdModel, long>,
	IRequestHandler<UpdatePublicationExternDatabaseIdCommand, UpdatePublicationExternDatabaseIdCommandResponse>
{

	public UpdatePublicationExternDatabaseIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	: base(mapper, repository) { }
	public async Task<UpdatePublicationExternDatabaseIdCommandResponse> Handle(UpdatePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePublicationExternDatabaseIdCommand,
			UpdatePublicationExternDatabaseIdCommandResponse>(request);
	}

}
