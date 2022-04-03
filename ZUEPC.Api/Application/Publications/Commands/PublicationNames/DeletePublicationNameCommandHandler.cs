using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class DeletePublicationNameCommandHandler :
	DeleteSimpleModelBaseCommandHandler<PublicationNameModel, long>,
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
