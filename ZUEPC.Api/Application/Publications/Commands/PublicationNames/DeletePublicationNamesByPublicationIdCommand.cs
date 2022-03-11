using MediatR;

namespace ZUEPC.Api.Application.Publications.Commands.PublicationNames;

public class DeletePublicationNamesByPublicationIdCommand :
	IRequest<DeletePublicationNamesByPublicationIdCommandResponse>
{
	public long PublicationId { get; set; }	
}
