using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetInstitutionQueryHandler : IRequestHandler<GetInstitutionQuery, GetInstitutionQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionData _repository;

	public GetInstitutionQueryHandler(IMapper mapper, IInstitutionData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetInstitutionQueryResponse> Handle(GetInstitutionQuery request, CancellationToken cancellationToken)
	{
		InstitutionModel? queryResult = await _repository.GetInstitutionByIdAsync(request.InstitutionId);
		if (queryResult is null)
		{
			return new() { Success = false };
		}

		Institution mappedModel = _mapper.Map<Institution>(queryResult);
		return new() { Success = true, Institution = mappedModel };
	}
}
