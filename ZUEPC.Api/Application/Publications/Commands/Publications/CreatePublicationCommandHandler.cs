using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationCommandHandler :
	EPCCreateSimpleModelCommandHandlerBase<Publication, PublicationModel>,
	IRequestHandler<CreatePublicationCommand, CreatePublicationCommandResponse>
{
	public CreatePublicationCommandHandler(IMapper mapper, IPublicationData repository)
	: base(mapper, repository) { }

	public async Task<CreatePublicationCommandResponse> Handle(CreatePublicationCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePublicationCommand, CreatePublicationCommandResponse>(request);
	}
}
