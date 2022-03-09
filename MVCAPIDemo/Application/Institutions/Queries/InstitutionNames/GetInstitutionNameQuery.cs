using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionNameQuery :
	EPCSimpleQueryBase,
	IRequest<GetInstitutionNameQueryResponse>
{
}
