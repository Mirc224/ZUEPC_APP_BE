using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Queries.Institutions.Details.BaseHandlers;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetInstitutionDetailsQueryHandler :
	EPCInstitutionDetailsQueryHandlerBase,
	IRequestHandler<GetInstitutionDetailsQuery, GetInstitutionDetailsQueryResponse>
{
	public GetInstitutionDetailsQueryHandler(IMapper mapper, IMediator mediator)
	: base(mapper, mediator) { }
	
	public async Task<GetInstitutionDetailsQueryResponse> Handle(GetInstitutionDetailsQuery request, CancellationToken cancellationToken)
	{
		long institutionId = request.Id;
		Institution? institution = (await _mediator.Send(new GetInstitutionQuery() { Id= institutionId })).Data;
		if (institution is null)
		{
			return new() { Success = false };
		}
		InstitutionDetails result = await ProcessInstitutionDetails(institution);

		return new() { Success = true, Data = result };
	}
}
