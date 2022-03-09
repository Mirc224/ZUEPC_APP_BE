using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class UpdatePublicationNameCommandHandler :
	EPCUpdateSimpleModelCommandHandlerBase<PublicationNameModel>,
	IRequestHandler<UpdatePublicationNameCommand, UpdatePublicationNameCommandResponse>
{
	public UpdatePublicationNameCommandHandler(IMapper mapper, IPublicationNameData repository)
	: base(mapper, repository) { }
	
	public async Task<UpdatePublicationNameCommandResponse> Handle(UpdatePublicationNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePublicationNameCommand,
			UpdatePublicationNameCommandResponse>(request);
	}
}
