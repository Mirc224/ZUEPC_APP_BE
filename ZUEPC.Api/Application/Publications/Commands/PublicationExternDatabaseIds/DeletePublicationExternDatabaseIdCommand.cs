using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class DeletePublicationExternDatabaseIdCommand : 
	EPCDeleteModelCommandBase<long>, 
	IRequest<DeletePublicationExternDatabaseIdCommandResponse>
{
}
