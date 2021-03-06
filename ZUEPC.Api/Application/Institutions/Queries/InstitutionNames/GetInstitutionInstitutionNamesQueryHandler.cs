using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionInstitutionNamesQueryHandler : IRequestHandler<GetInstitutionInstitutionNamesQuery, GetInstitutionInstitutionNamesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionNameData _repository;

	public GetInstitutionInstitutionNamesQueryHandler(IMapper mapper, IInstitutionNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetInstitutionInstitutionNamesQueryResponse> Handle(GetInstitutionInstitutionNamesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<InstitutionNameModel> queryResult = await _repository.GetInstitutionNamesByInstitutionIdAsync(request.InstitutionId);
		List<InstitutionName> mappedResult = _mapper.Map<List<InstitutionName>>(queryResult);
		return new() { Success = true, Data = mappedResult };
	}
}
