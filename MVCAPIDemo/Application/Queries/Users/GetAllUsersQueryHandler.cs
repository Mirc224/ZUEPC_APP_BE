using AutoMapper;
using DataAccess.Data;
using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;
    public GetAllUsersQueryHandler(IMapper mapper,IUserData repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetUsers();
        var usersResponses = _mapper.Map<List<User>>(users);
        return usersResponses;
    }
}
