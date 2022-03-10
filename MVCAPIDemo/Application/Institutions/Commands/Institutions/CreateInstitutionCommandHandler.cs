using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionCommandHandler : 
	EPCCreateSimpleModelCommandHandlerBase<Institution, InstitutionModel>,
	IRequestHandler<CreateInstitutionCommand, CreateInstitutionCommandResponse>
{
	public CreateInstitutionCommandHandler(IMapper mapper, IInstitutionData repository)
	: base(mapper, repository) { }

	public async Task<CreateInstitutionCommandResponse> Handle(CreateInstitutionCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreateInstitutionCommand, CreateInstitutionCommandResponse>(request);
	}
}
