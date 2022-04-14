using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstititutionNamesByInstititutionIdInSetQueryHandler :
	IRequestHandler<GetAllInstititutionNamesByInstititutionIdInSetQuery, GetAllInstititutionNamesByInstititutionIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionNameData _repository;

	public GetAllInstititutionNamesByInstititutionIdInSetQueryHandler(IMapper mapper, IInstitutionNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllInstititutionNamesByInstititutionIdInSetQueryResponse> Handle(GetAllInstititutionNamesByInstititutionIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<InstitutionNameModel> institutionNames = await _repository.GetAllInstitutionNamesByInstitutionIdInSetAsync(request.InstitutionIds);
		if (institutionNames is null)
		{
			return new() { Success = false };
		}

		IEnumerable<InstitutionName> mapedResult = _mapper.Map<List<InstitutionName>>(institutionNames);
		return new() { Success = true, Data = mapedResult };
	}
}
