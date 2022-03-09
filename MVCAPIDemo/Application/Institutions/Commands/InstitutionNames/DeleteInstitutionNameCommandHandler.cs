using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class DeleteInstitutionNameCommandHandler :
	EPCDeleteSimpleModelBaseCommandHandler<InstitutionNameModel>,
	IRequestHandler<
		DeleteInstitutionNameCommand,
		DeleteInstitutionNameCommandResponse>
{
	public DeleteInstitutionNameCommandHandler(IInstitutionNameData repository)
		: base(repository) { }
	
	public async Task<DeleteInstitutionNameCommandResponse> Handle(DeleteInstitutionNameCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeleteInstitutionNameCommand, DeleteInstitutionNameCommandResponse>(request);
	}
}
