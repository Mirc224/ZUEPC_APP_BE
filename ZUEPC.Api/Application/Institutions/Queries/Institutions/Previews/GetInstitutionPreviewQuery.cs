using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetInstitutionPreviewQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetInstitutionPreviewQueryResponse>
{
}
