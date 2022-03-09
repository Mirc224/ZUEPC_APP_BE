using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class UpdatePersonNameCommandHandler :
	EPCUpdateSimpleModelCommandHandlerBase<PersonNameModel>,
	IRequestHandler<UpdatePersonNameCommand, UpdatePersonNameCommandResponse>
{
	public UpdatePersonNameCommandHandler(IMapper mapper, IPersonNameData repository)
	: base(mapper, repository) { }

	public async Task<UpdatePersonNameCommandResponse> Handle(UpdatePersonNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePersonNameCommand,
			UpdatePersonNameCommandResponse>(request);
	}
}
