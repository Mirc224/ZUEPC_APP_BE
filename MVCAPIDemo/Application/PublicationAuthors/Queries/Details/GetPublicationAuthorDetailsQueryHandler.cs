using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Queries.Details;

public class GetPublicationAuthorDetailsQueryHandler : IRequestHandler<GetPublicationAuthorDetailsQuery, GetPublicationAuthorDetailsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPublicationAuthorDetailsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<GetPublicationAuthorDetailsQueryResponse> Handle(GetPublicationAuthorDetailsQuery request, CancellationToken cancellationToken)
	{
		long publicationId = request.PublicationId;
		ICollection<PublicationAuthor> publicationAuthors = (await _mediator.Send(new GetPublicationPublicationAuthorsQuery() { PublicationId = publicationId })).Data;
		List<PublicationAuthorDetails> resultDetails = new();
		foreach(PublicationAuthor pubAuthor in publicationAuthors)
		{
			long personId = pubAuthor.PersonId;
			long institutionId = pubAuthor.InstitutionId;
			PublicationAuthorDetails authorDetails = _mapper.Map<PublicationAuthorDetails>(pubAuthor);
			authorDetails.PersonPreview = (await _mediator.Send(new GetPersonPreviewQuery() 
			{ 
				PersonId = personId 
			})).Data;
			authorDetails.InstitutionPreview = (await _mediator
				.Send(new GetInstitutionPreviewQuery()
				{
					InstitutionId = institutionId
				})).Data;

			resultDetails.Add(authorDetails);
		}

		return new() { Success = true, Data = resultDetails };
	}
}
