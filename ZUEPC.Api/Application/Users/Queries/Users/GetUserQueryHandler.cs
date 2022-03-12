using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Microsoft.Extensions.Localization;
using Users.Base.Domain;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Localization;

namespace ZUEPC.Application.Users.Queries.Users;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse>
{
	private readonly IUserData _repository;
	private readonly IStringLocalizer<DataAnnotations> _localizer;
	private readonly IMapper _mapper;

	public GetUserQueryHandler(IMapper mapper, IUserData repository, IStringLocalizer<DataAnnotations> localizer)
	{
		_repository = repository;
		_localizer = localizer;
		_mapper = mapper;
	}

	public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
	{
		UserModel? userModel = await _repository.GetModelByIdAsync(request.Id);
		if (userModel is null)
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.USER_NOT_EXIST].Value },
				Success = false
			};
		}

		User user = _mapper.Map<User>(userModel);
		return new() { Data = user, Success = true };
	}
}
