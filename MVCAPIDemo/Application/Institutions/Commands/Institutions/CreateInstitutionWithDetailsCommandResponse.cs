using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionWithDetailsCommandResponse : ResponseBase
{
	public InstitutionDetails CreatedInstitutionDetails { get; set; }
}