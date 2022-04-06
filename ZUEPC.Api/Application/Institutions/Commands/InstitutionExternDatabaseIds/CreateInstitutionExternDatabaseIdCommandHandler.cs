using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class CreateInstitutionExternDatabaseIdCommandHandler :
	EPCCreateSimpleModelCommandHandlerBase<InstitutionExternDatabaseId, InstitutionExternDatabaseIdModel>,
	IRequestHandler<
		CreateInstitutionExternDatabaseIdCommand,
		CreateInstitutionExternDatabaseIdCommandResponse>
{
	public CreateInstitutionExternDatabaseIdCommandHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	: base(mapper, repository) { }

	public async Task<CreateInstitutionExternDatabaseIdCommandResponse> Handle(CreateInstitutionExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreateInstitutionExternDatabaseIdCommand, CreateInstitutionExternDatabaseIdCommandResponse>(request);
	}
}
