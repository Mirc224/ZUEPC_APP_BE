using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class CreatePublicationNameCommandHandler :
	EPCCreateSimpleModelCommandHandlerBase<PublicationName, PublicationNameModel>,
	IRequestHandler<CreatePublicationNameCommand, CreatePublicationNameCommandResponse>
{
	public CreatePublicationNameCommandHandler(IMapper mapper, IPublicationNameData repository)
		: base(mapper, repository) {}
	public async Task<CreatePublicationNameCommandResponse> Handle(CreatePublicationNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePublicationNameCommand, CreatePublicationNameCommandResponse>(request);
	}
}
