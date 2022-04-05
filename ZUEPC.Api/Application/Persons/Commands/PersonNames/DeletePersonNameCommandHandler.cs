using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNameCommandHandler :
	DeleteModelWithIdCommandHandlerBase<PersonNameModel, long>,
	IRequestHandler<DeletePersonNameCommand, DeletePersonNameCommandResponse>
{
	public DeletePersonNameCommandHandler(IPersonNameData repository)
	:base(repository) { }

	public async Task<DeletePersonNameCommandResponse> Handle(DeletePersonNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeletePersonNameCommand, DeletePersonNameCommandResponse>(request);
	}
}
