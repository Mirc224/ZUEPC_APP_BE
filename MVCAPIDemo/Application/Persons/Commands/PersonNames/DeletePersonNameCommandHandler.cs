using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNameCommandHandler :
	EPCDeleteSimpleBaseCommandHandler<PersonNameModel>,
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
