using AutoMapper;
using MediatR;
using Users.Base.Domain;
using ZUEPC.Api.Application.Users.Entities.Details;
using ZUEPC.Api.Application.Users.Queries.UserRoles;
using ZUEPC.Users.Base.Domain;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details.BaseHanlders;

public class UserDetailsQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public UserDetailsQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<UserDetails> ProcessUserDetails(User userDomain)
	{
		long userId = userDomain.Id;
		UserDetails result = _mapper.Map<UserDetails>(userDomain);
		IEnumerable<UserRole>? userRoles = (await _mediator.Send(new GetUserRolesByUserIdQuery() { UserId = userId })).Data;
		result.UserRoles = userRoles?.Select(x => x.RoleId);
		return result;
	}
}
