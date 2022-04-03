using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetInstitutionPreviewQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetInstitutionPreviewQueryResponse>
{
}
