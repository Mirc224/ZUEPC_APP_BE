﻿using AutoMapper;
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
		ICollection<PublicationAuthor> pubAuthorDomain = (await _mediator.Send(new GetPublicationPublicationAuthorsQuery() 
															{ PublicationId = publicationId })).Data;
		List<PublicationAuthorDetails> resultAuthorsList = new List<PublicationAuthorDetails>();
		foreach(PublicationAuthor author in pubAuthorDomain)
		{
			long personId = author.PersonId;
			long institutionId = author.InstitutionId;
			PublicationAuthorDetails publicationAuthorDetails = _mapper.Map<PublicationAuthorDetails>(author);
			publicationAuthorDetails.PersonPreview = (await _mediator.Send(new GetPersonPreviewQuery() { Id = personId })).Data;
			publicationAuthorDetails.InstitutionPreview = (await _mediator.Send(new GetInstitutionPreviewQuery() 
			{ 
				Id = institutionId
			})).Data;
			resultAuthorsList.Add(publicationAuthorDetails);
		}
		return new() { Success = true, Data = resultAuthorsList };
	}
}
