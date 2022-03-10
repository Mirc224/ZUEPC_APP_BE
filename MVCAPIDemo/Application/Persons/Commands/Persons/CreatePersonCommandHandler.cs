using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonCommandHandler : 
	EPCCreateSimpleModelCommandHandlerBase<Person, PersonModel>,
	IRequestHandler<CreatePersonCommand, CreatePersonCommandResponse>
{
	public CreatePersonCommandHandler(IMapper mapper, IPersonData repository)
	: base(mapper, repository) { }

	public async Task<CreatePersonCommandResponse> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePersonCommand, CreatePersonCommandResponse>(request);
	}
}
