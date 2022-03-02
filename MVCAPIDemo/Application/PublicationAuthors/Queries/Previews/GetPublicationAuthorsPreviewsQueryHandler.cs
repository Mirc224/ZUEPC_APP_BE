using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Queries.Previews;

public class GetPublicationAuthorsPreviewsQueryHandler : IRequestHandler<GetPublicationAuthorsPreviewsQuery, GetPublicationAuthorsPreviewsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPublicationAuthorsPreviewsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<GetPublicationAuthorsPreviewsQueryResponse> Handle(GetPublicationAuthorsPreviewsQuery request, CancellationToken cancellationToken)
	{
		long publicationId = request.PublicationId;
		ICollection<PublicationAuthor> pubAuthorDomain = (await _mediator.Send(new GetPublicationAuthorsQuery() 
															{ PublicationId = publicationId })).PublicationAuthors;
		List<PublicationAuthorDetails> resultAuthorsList = new List<PublicationAuthorDetails>();
		foreach(PublicationAuthor author in pubAuthorDomain)
		{
			long personId = author.PersonId;
			long institutionId = author.InstitutionId;
			PublicationAuthorDetails publicationAuthorDetails = _mapper.Map<PublicationAuthorDetails>(author);
			publicationAuthorDetails.PersonPreview = (await _mediator.Send(new GetPersonPreviewQuery() { PersonId = personId })).PersonPreview;
			publicationAuthorDetails.InstitutionPreview = (await _mediator.Send(new GetInstitutionPreviewQuery() 
			{ 
				InstitutionId = institutionId
			})).InstitutionPreview;
			resultAuthorsList.Add(publicationAuthorDetails);
		}
		return new() { Success = true, PublicationAuthorPreviews = resultAuthorsList };
	}
}
