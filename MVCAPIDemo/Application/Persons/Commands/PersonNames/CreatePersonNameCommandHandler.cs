using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class CreatePersonNameCommandHandler :
	EPCCreateSimpleModelCommandHandlerBase<PersonName, PersonNameModel>,
	IRequestHandler<CreatePersonNameCommand, CreatePersonNameCommandResponse>
{
	public CreatePersonNameCommandHandler(IMapper mapper, IPersonNameData repository)
	: base(mapper, repository) { }

	public async Task<CreatePersonNameCommandResponse> Handle(CreatePersonNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePersonNameCommand, CreatePersonNameCommandResponse>(request);
	}
}
