using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.EvidencePublication.PublicationAuthors;

namespace ZUEPC.Api.Application.PublicationAuthors.Queries;

public class GetAllPublicationAuthorsByPublicationIdInSetQueryHandler :
	IRequestHandler<GetAllPublicationAuthorsByPublicationIdInSetQuery, GetAllPublicationAuthorsByPublicationIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationAuthorData _repository;

	public GetAllPublicationAuthorsByPublicationIdInSetQueryHandler(IMapper mapper, IPublicationAuthorData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPublicationAuthorsByPublicationIdInSetQueryResponse> Handle(GetAllPublicationAuthorsByPublicationIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationAuthorModel> authors = await _repository.GetAllPublicationAuthorsByPublicationIdInSetAsync(request.PublicationIds.ToHashSet());
		if (authors is null)
		{
			return new() { Success = false };
		}

		IEnumerable<PublicationAuthor> mappedResult = _mapper.Map<List<PublicationAuthor>>(authors);
		return new() { Success = true, Data = mappedResult };
	}
}
