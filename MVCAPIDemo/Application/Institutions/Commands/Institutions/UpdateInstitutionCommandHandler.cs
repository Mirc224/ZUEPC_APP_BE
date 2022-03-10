using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class UpdateInstitutionCommandHandler :
	EPCUpdateSimpleModelCommandHandlerBase<InstitutionModel>,
	IRequestHandler<UpdateInstitutionCommand, UpdateInstitutionCommandResponse>
{
	public UpdateInstitutionCommandHandler(IMapper mapper, IInstitutionData repository)
		: base(mapper, repository) { }

	public async Task<UpdateInstitutionCommandResponse> Handle(UpdateInstitutionCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdateInstitutionCommand,
			UpdateInstitutionCommandResponse>(request);
	}
}
