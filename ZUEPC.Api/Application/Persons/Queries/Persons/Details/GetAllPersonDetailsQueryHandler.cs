using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Queries.Persons.Details.BaseHandler;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Helpers;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQueryHandler :
	EPCPersonDetailsQueryHandlerBase,
	IRequestHandler<GetAllPersonDetailsQuery, GetAllPersonDetailsQueryResponse>
{
	public GetAllPersonDetailsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }

	public async Task<GetAllPersonDetailsQueryResponse> Handle(GetAllPersonDetailsQuery request, CancellationToken cancellationToken)
	{
		GetAllPersonsQueryResponse response = await _mediator.Send(new GetAllPersonsQuery() { PaginationFilter = request.PaginationFilter });

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}

		IEnumerable<Person> PersonDomains = response.Data;
		List<PersonDetails> result = new();
		foreach (Person Person in PersonDomains.OrEmptyIfNull())
		{
			PersonDetails PersonDetails = await ProcessPersonDetails(Person);
			if (PersonDetails != null)
			{
				result.Add(PersonDetails);
			}
		}
		int totalRecords = response.TotalRecords;
		return PaginationHelper.ProcessResponse<GetAllPersonDetailsQueryResponse, GetAllPersonDetailsQuery, PersonDetails>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route);
	}
}
