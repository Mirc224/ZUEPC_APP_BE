using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNameQuery : 
	QueryWithIdBase<long>,
	IRequest<GetPublicationNameQueryResponse>
{
}
