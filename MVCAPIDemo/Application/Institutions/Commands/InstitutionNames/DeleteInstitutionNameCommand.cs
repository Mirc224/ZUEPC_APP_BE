using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class DeleteInstitutionNameCommand : EPCDeleteBaseCommand, IRequest<DeleteInstitutionNameCommandResponse>
{
}
