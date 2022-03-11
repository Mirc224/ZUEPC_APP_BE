using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Users.Entities.Details;
using ZUEPC.Api.Application.Users.Queries.Users.Details.BaseHanlders;
using ZUEPC.Application.Users.Queries;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetUserDetailsQueryHandler :
	UserDetailsQueryHandlerBase,
	IRequestHandler<GetUserDetailsQuery, GetUserDetailsQueryResponse>
{
	public GetUserDetailsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }

	public async Task<GetUserDetailsQueryResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
	{
		GetUserQueryResponse response = await _mediator.Send(new GetUserQuery() { Id = request.Id });
		if(!response.Success)
		{
			return new() { Success = false };
		}
		UserDetails userDetails = await ProcessPublicationDetails(response.Data);
		return new() { Success = true, Data = userDetails };
	}
}
