using MediatR;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.DataAccess.Data.Persons;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, DeletePersonCommandResponse>
{
	private readonly IMediator _mediator;
	private readonly IPersonData _repository;

	public DeletePersonCommandHandler(IMediator mediator,IPersonData repository)
	{
		_mediator = mediator;
		_repository = repository;
	}
	public async Task<DeletePersonCommandResponse> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
	{
		long personId = request.Id;

		GetPersonQueryResponse model = await _mediator.Send(new GetPersonQuery() { PersonId = personId });

		if(!model.Success)
		{
			return new() { Success = false };
		}
		await _mediator.Send(new DeletePersonNamesByPersonIdCommand() { PersonId = personId });
		await _mediator.Send(new DeletePersonExternDatabaseIdsByPersonIdCommand() { PersonId = personId });
		await _mediator.Send(new DeletePublicationAuthorsByPersonIdCommand() { PersonId = personId });
		await _repository.DeleteModelByIdAsync(personId);
		return new() { Success = true };
	}
}
