using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse>
{
    private readonly IUserData _repository;
	private readonly IStringLocalizer<DataAnnotations> _localizer;
	private readonly IMapper _mapper;
    
	public GetUserQueryHandler(IMapper mapper,IUserData repository, IStringLocalizer<DataAnnotations> localizer)
    {
        _repository = repository;
		_localizer = localizer;
		_mapper = mapper;
    }

    public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userModel = await _repository.GetUserById(request.Id);
		if (userModel is null)
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer["UserNotFound"].Value },
				Success = false
			};
		}
		
		var user = _mapper.Map<User>(userModel);
		var roles = await _repository.GetUserRoles(user.Id);
		user.Roles = roles.Select(x => x.Id).ToList();
		
		var response = new GetUserQueryResponse() { User = user, Success = true };
        return response;
    }
}
