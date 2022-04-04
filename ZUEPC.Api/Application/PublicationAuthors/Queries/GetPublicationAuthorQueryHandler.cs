using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.EvidencePublication.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQueryHandler : 
	GetSimpleModelQueryHandlerBase<IPublicationAuthorData, PublicationAuthor, PublicationAuthorModel, long>,
	IRequestHandler<GetPublicationAuthorQuery, GetPublicationAuthorQueryResponse>
{
	public GetPublicationAuthorQueryHandler(IMapper mapper, IPublicationAuthorData repository)
	:base(mapper, repository) { }

	public async Task<GetPublicationAuthorQueryResponse> Handle(GetPublicationAuthorQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetPublicationAuthorQuery, GetPublicationAuthorQueryResponse>(request);
	}
}
