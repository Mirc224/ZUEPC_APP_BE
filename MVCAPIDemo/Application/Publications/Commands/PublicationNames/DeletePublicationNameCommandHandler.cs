using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class DeletePublicationNameCommandHandler :
	EPCDeleteSimpleBaseCommandHandler<PublicationNameModel>,
	IRequestHandler<DeletePublicationNameCommand, DeletePublicationNameCommandResponse>
{
	public DeletePublicationNameCommandHandler(IPublicationNameData repository)
	: base(repository) { }

	public async Task<DeletePublicationNameCommandResponse> Handle(DeletePublicationNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeletePublicationNameCommand, DeletePublicationNameCommandResponse>(request);
	}
}
