using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.PersonNames;

public class GetAllPersonNamesByPersonIdInSetQueryHandler : 
	IRequestHandler<GetAllPersonNamesByPersonIdInSetQuery, GetAllPersonNamesByPersonIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonNameData _repository;

	public GetAllPersonNamesByPersonIdInSetQueryHandler(IMapper mapper, IPersonNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPersonNamesByPersonIdInSetQueryResponse> Handle(GetAllPersonNamesByPersonIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonNameModel> personNames = await _repository.GetAllPersonNamesByPersonIdInSetAsync(request.PersonIds);
		if(personNames is null)
		{
			return new() { Success = false };
		}

		IEnumerable<PersonName> mappedResult = _mapper.Map<List<PersonName>>(personNames);
		return new() { Success = true, Data = mappedResult };
	}
}
