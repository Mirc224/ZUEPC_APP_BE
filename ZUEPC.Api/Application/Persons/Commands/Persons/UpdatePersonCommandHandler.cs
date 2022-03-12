using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class UpdatePersonCommandHandler :
	UpdateSimpleModelCommandHandlerBase<PersonModel>,
	IRequestHandler<UpdatePersonCommand, UpdatePersonCommandResponse>
{

	public UpdatePersonCommandHandler(IMapper mapper, IPersonData repository)
		: base(mapper, repository) { }
	
	public async Task<UpdatePersonCommandResponse> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePersonCommand,
			UpdatePersonCommandResponse>(request);
	}
}
