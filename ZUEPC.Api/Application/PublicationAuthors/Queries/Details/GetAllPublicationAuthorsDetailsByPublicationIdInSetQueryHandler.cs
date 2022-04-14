using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Api.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.PublicationAuthors;

namespace ZUEPC.Api.Application.PublicationAuthors.Queries.Details;

public class GetAllPublicationAuthorsDetailsByPublicationIdInSetQueryHandler :
	IRequestHandler<GetAllPublicationAuthorsDetailsByPublicationIdInSetQuery, GetAllPublicationAuthorsDetailsByPublicationIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetAllPublicationAuthorsDetailsByPublicationIdInSetQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<GetAllPublicationAuthorsDetailsByPublicationIdInSetQueryResponse> Handle(GetAllPublicationAuthorsDetailsByPublicationIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationAuthor>? publicationAuthors = (await _mediator
			.Send(new GetAllPublicationAuthorsByPublicationIdInSetQuery() { PublicationIds = request.PublicationIds }))
			.Data;
		if (publicationAuthors is null)
		{
			return new() { Success = false };
		}
		IEnumerable<long> personIds = publicationAuthors.Select(x => x.PersonId);
		IEnumerable<long> institutionIds = publicationAuthors.Select(x => x.InstitutionId);

		IEnumerable<PersonPreview> personPreviews = (await _mediator
			.Send(new GetAllPersonPreviewsForIdsInSetQuery() { PersonIds = personIds }))
			.Data
			.OrEmptyIfNull();
		IEnumerable<InstitutionPreview> institutionPreviews = (await _mediator
			.Send(new GetAllInstitutionsPreviewsForIdsInSetQuery() { InstitutionIds = institutionIds }))
			.Data
			.OrEmptyIfNull();
		List<PublicationAuthorDetails> resultDetails = new();

		foreach (PublicationAuthor pubAuthor in publicationAuthors.OrEmptyIfNull())
		{
			PublicationAuthorDetails authorDetails = _mapper.Map<PublicationAuthorDetails>(pubAuthor);
			authorDetails.PersonPreview = personPreviews.FirstOrDefault(x => x.Id == pubAuthor.PersonId);
			authorDetails.InstitutionPreview = institutionPreviews.FirstOrDefault(x => x.Id == pubAuthor.InstitutionId);
			resultDetails.Add(authorDetails);
		}
		return new() { Success = true, Data = resultDetails };
	}
}
