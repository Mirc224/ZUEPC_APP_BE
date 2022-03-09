using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class CreateInstitutionNameCommandHandler :
	EPCCreateSimpleModelCommandHandlerBase<InstitutionName, InstitutionNameModel>,
	IRequestHandler<
		CreateInstitutionNameCommand,
		CreateInstitutionNameCommandResponse>
{
	public CreateInstitutionNameCommandHandler(IMapper mapper, IInstitutionNameData repository)
	: base(mapper, repository) { }
	public async Task<CreateInstitutionNameCommandResponse> Handle(CreateInstitutionNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreateInstitutionNameCommand, CreateInstitutionNameCommandResponse>(request);
	}
}
