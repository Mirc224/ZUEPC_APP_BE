using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetPersonPreviewQueryHandler : IRequestHandler<GetPersonPreviewQuery, GetPersonPreviewQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPersonPreviewQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<GetPersonPreviewQueryResponse> Handle(GetPersonPreviewQuery request, CancellationToken cancellationToken)
	{
		long personId = request.Id;
		Person? personDomain = (await _mediator.Send(new GetPersonQuery() { Id = personId})).Data;
		if(personDomain is null)
		{
			return new() { Success = false };
		}
		PersonPreview resultPreview = _mapper.Map<PersonPreview>(personDomain);
		resultPreview.Names = (await _mediator.Send(new GetPersonPersonNamesQuery()
		{ 
			PersonId = personId 
		})).Data;
		resultPreview.ExternDatabaseIds = (await _mediator.Send(new GetPersonPersonExternDatabaseIdsQuery()
		{
			PersonId = personId
		})).Data;
		return new() { Success = true, Data = resultPreview };
	}
}
