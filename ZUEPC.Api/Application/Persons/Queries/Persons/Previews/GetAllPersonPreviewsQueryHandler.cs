using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews.BaseHandlers;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Helpers;
using ZUEPC.Base.QueryFilters;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQueryHandler :
	EPCPersonPreviewQueryHandlerBase,
	IRequestHandler<GetAllPersonPreviewsQuery, GetAllPersonPreviewsQueryResponse>
{
	public GetAllPersonPreviewsQueryHandler(IMapper mapper, IMediator mediator)
		:base(mapper, mediator)
	{ }
	
	public async Task<GetAllPersonPreviewsQueryResponse> Handle(GetAllPersonPreviewsQuery request, CancellationToken cancellationToken)
	{
		GetAllPersonsQueryResponse response = await _mediator.Send(new GetAllPersonsQuery() { 
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});
		
		if(!response.Success || response.Data is null) 
		{
			return new() { Success = false };
		}

		IEnumerable<Person> personDomains = response.Data;
		List<PersonPreview> result = new();
		foreach(Person person in personDomains.OrEmptyIfNull())
		{
			PersonPreview personPreview = await ProcessPersonPreview(person);
			if( personPreview != null)
			{
				result.Add(personPreview);
			}
		}
		int totalRecords = response.TotalRecords;
		return PaginationHelper.ProcessResponse<GetAllPersonPreviewsQueryResponse, PersonPreview, PersonFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
