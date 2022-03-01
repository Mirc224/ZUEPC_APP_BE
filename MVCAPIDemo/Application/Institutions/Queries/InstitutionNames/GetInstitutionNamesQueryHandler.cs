using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionNamesQueryHandler : IRequestHandler<GetInstitutionNamesQuery, GetInstitutionNamesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionNameData _repository;

	public GetInstitutionNamesQueryHandler(IMapper mapper, IInstitutionNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetInstitutionNamesQueryResponse> Handle(GetInstitutionNamesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<InstitutionNameModel> queryResult = await _repository.GetInstitutionNamesByInstitutionIdAsync(request.InstitutionId);
		List<InstitutionName> mappedResult = _mapper.Map<List<InstitutionName>>(queryResult);
		return new() { Success = true, InstitutionNames = mappedResult };
	}
}
