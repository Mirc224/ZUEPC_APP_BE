using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Api.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsWithIdInSetQueryHandler :
	IRequestHandler<GetAllInstitutionsWithIdInSetQuery, GetAllInstitutionsWithIdInSetQueryResponse>
{
	private readonly IInstitutionData _repository;
	private readonly IMapper _mapper;

	public GetAllInstitutionsWithIdInSetQueryHandler(IMapper mapper, IInstitutionData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllInstitutionsWithIdInSetQueryResponse> Handle(GetAllInstitutionsWithIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<InstitutionModel> response = await _repository.GetModelsWhereIdInSetAsync(request.InstitutionIds);
		IEnumerable<Institution> mappedResult = _mapper.Map<List<Institution>>(response);
		return new() { Success = true, Data = mappedResult };
	}
}
