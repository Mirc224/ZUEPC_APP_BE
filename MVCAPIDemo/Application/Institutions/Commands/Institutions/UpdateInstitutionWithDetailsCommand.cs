using MediatR;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class UpdateInstitutionWithDetailsCommand : 
	EPCUpdateBaseCommand, 
	IRequest<UpdateInstitutionWithDetailsCommandResponse>
{
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
	public IEnumerable<InstitutionNameCreateDto>? NamesToInsert { get; set; }
	public IEnumerable<InstitutionNameUpdateDto>? NamesToUpdate { get; set; }
	public IEnumerable<long>? NamesToDelete { get; set; }
	public IEnumerable<InstitutionExternDatabaseIdCreateDto>? ExternDatabaseIdsToInsert { get; set; }
	public IEnumerable<InstitutionExternDatabaseIdUpdateDto>? ExternDatabaseIdsToUpdate { get; set; }
	public IEnumerable<long>? ExternDatabaseIdsToDelete { get; set; }
}
