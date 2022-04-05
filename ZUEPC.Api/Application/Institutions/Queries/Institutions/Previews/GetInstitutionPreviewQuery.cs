using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetInstitutionPreviewQuery : 
	QueryWithIdBase<long>,
	IRequest<GetInstitutionPreviewQueryResponse>
{
}
