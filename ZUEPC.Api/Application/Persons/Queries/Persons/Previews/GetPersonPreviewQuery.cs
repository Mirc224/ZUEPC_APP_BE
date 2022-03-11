using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetPersonPreviewQuery : 
	EPCSimpleQueryBase,
	IRequest<GetPersonPreviewQueryResponse>
{
}
