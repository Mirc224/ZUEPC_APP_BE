using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorsQueryHandler : IRequestHandler<GetPublicationAuthorsQuery, GetPublicationAuthorsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationAuthorData _repository;

	public GetPublicationAuthorsQueryHandler(IMapper mapper, IPublicationAuthorData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationAuthorsQueryResponse> Handle(GetPublicationAuthorsQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationAuthorModel> queryResult = await _repository.GetPublicationAuthorByPublicationIdAsync(request.PublicationId);
		List<PublicationAuthor> mappedResult = _mapper.Map<List<PublicationAuthor>>(queryResult);
		return new() { Success = true, PublicationAuthors = mappedResult };
	}
}
