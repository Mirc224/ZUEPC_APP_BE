using AutoMapper;
using DataAccess.Data;
using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;
    public GetUserQueryHandler(IMapper mapper,IUserData repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUser(request.Id);
        return _mapper.Map<User>(user);
    }
}
