using AutoMapper;
using MediatR;
using Users.Base.Domain;
using ZUEPC.Api.Application.Users.Entities.Details;
using ZUEPC.Api.Application.Users.Queries.Users.Details.BaseHanlders;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Helpers;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetAllUsersDetailsQueryHandler :
	UserDetailsQueryHandlerBase,
	IRequestHandler<GetAllUsersDetailsQuery, GetAllUsersDetailsQueryResponse>
{
	public GetAllUsersDetailsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }

	public async Task<GetAllUsersDetailsQueryResponse> Handle(GetAllUsersDetailsQuery request, CancellationToken cancellationToken)
	{
		GetAllUsersQueryResponse response = await _mediator.Send(new GetAllUsersQuery() { 
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}

		IEnumerable<User> userDomains = response.Data;
		List<UserDetails> result = new();
		foreach (User user in userDomains.OrEmptyIfNull())
		{
			UserDetails UserDetails = await ProcessUserDetails(user);
			if (UserDetails != null)
			{
				result.Add(UserDetails);
			}
		}
		int totalRecords = response.TotalRecords;
		return PaginationHelper.ProcessResponse<GetAllUsersDetailsQueryResponse, UserDetails, UserFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
