using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class UpdateInstitutionNameCommandHandler : 
	EPCUpdateSimpleModelCommandHandlerBase<InstitutionNameModel>,
	IRequestHandler<UpdateInstitutionNameCommand, UpdateInstitutionNameCommandResponse>
{
	public UpdateInstitutionNameCommandHandler(IMapper mapper, IInstitutionNameData repository)
		: base(mapper, repository) {}

	public async Task<UpdateInstitutionNameCommandResponse> Handle(UpdateInstitutionNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdateInstitutionNameCommand,
			UpdateInstitutionNameCommandResponse>(request);
	}
}
