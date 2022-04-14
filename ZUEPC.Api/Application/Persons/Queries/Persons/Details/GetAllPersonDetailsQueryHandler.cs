using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Api.Application.Persons.Queries.PersonNames;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Queries.Persons.Details.BaseHandler;
using ZUEPC.Base.Extensions;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetAllPersonDetailsQueryHandler :
	EPCPersonDetailsQueryHandlerBase,
	IRequestHandler<GetAllPersonDetailsQuery, GetAllPersonDetailsQueryResponse>
{
	public GetAllPersonDetailsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }

	public async Task<GetAllPersonDetailsQueryResponse> Handle(GetAllPersonDetailsQuery request, CancellationToken cancellationToken)
	{
		GetAllPersonsQueryResponse response = await _mediator.Send(new GetAllPersonsQuery()
		{
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}
		int totalRecords = response.TotalRecords;

		if (!response.Data.Any())
		{
			return PaginationHelper.ProcessResponse<GetAllPersonDetailsQueryResponse, PersonDetails, PersonFilter>(
			new List<PersonDetails>(),
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
		}

		IEnumerable<PersonDetails> result = await ProcessPersonDetails(response.Data);
		return PaginationHelper.ProcessResponse<GetAllPersonDetailsQueryResponse, PersonDetails, PersonFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
