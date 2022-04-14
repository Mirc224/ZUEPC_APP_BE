using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.Persons;

public class GetAllPersonsWithIdInSetQueryHandler :
	IRequestHandler<GetAllPersonsWithIdInSetQuery, GetAllPersonsWithIdInSetQueryResponse>
{
	private readonly IPersonData _repository;
	private readonly IMapper _mapper;
	
	public GetAllPersonsWithIdInSetQueryHandler(IMapper mapper, IPersonData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetAllPersonsWithIdInSetQueryResponse> Handle(GetAllPersonsWithIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PersonModel> response = await _repository.GetModelsWhereIdInSetAsync(request.PersonIds);
		IEnumerable<Person> mappedResult = _mapper.Map<List<Person>>(response);
		return new() { Success=true,  Data = mappedResult };
	}
}
