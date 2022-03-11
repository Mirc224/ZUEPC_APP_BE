using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class CreatePublicationExternDatabaseIdCommandHandler : 
	EPCCreateSimpleModelCommandHandlerBase<PublicationExternDatabaseId, PublicationExternDatabaseIdModel>,
	IRequestHandler<CreatePublicationExternDatabaseIdCommand, CreatePublicationExternDatabaseIdCommandResponse>
{
	public CreatePublicationExternDatabaseIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
		: base(mapper, repository) {}

	public async Task<CreatePublicationExternDatabaseIdCommandResponse> Handle(CreatePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePublicationExternDatabaseIdCommand, CreatePublicationExternDatabaseIdCommandResponse>(request);
	}
}
