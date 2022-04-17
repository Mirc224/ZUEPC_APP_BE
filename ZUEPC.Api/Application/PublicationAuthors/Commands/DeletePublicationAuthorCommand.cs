using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorCommand : 
	EPCDeleteCommandBase<long>, 
	IRequest<DeletePublicationAuthorCommandResponse>
{
}
