using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews.BaseHandlers;

public abstract class EPCPersonPreviewQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCPersonPreviewQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<PersonPreview> ProcessPersonPreview(Person personDomain)
	{
		long personId = personDomain.Id;
		PersonPreview resultPreview = _mapper.Map<PersonPreview>(personDomain);
		resultPreview.Names = (await _mediator.Send(new GetPersonPersonNamesQuery()
		{
			PersonId = personId
		})).Data;
		resultPreview.ExternDatabaseIds = (await _mediator.Send(new GetPersonPersonExternDatabaseIdsQuery()
		{
			PersonId = personId
		})).Data;
		return resultPreview;
	}
}
