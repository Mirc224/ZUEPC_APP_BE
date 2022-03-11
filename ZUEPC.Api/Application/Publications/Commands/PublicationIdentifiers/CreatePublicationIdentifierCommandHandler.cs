using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class CreatePublicationIdentifierCommandHandler :
	EPCCreateSimpleModelCommandHandlerBase<PublicationIdentifier, PublicationIdentifierModel>,
	IRequestHandler<CreatePublicationIdentifierCommand, CreatePublicationIdentifierCommandResponse>
{
	public CreatePublicationIdentifierCommandHandler(IMapper mapper, IPublicationIdentifierData repository)
	: base(mapper, repository) { }

	public async Task<CreatePublicationIdentifierCommandResponse> Handle(CreatePublicationIdentifierCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePublicationIdentifierCommand, CreatePublicationIdentifierCommandResponse>(request);
	}
}
