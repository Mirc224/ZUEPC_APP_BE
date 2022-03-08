using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQueryHandler : IRequestHandler<GetPublicationAuthorQuery, GetPublicationAuthorQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationAuthorData _repository;

	public GetPublicationAuthorQueryHandler(IMapper mapper, IPublicationAuthorData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetPublicationAuthorQueryResponse> Handle(GetPublicationAuthorQuery request, CancellationToken cancellationToken)
	{
		PublicationAuthorModel? result = await _repository.GetPublicationAuthorByIdAsync(request.PublicationAuthorRecordId);
		if (result is null)
		{
			return new() { Success = false };
		}
		PublicationAuthor mappedResult = _mapper.Map<PublicationAuthor>(result);
		return new() { Success = true, PublicationAuthor = mappedResult };
	}
}
