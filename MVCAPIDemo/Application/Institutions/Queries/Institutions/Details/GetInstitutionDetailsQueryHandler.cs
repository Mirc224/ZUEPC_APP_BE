using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetInstitutionDetailsQueryHandler :
	IRequestHandler<
		GetInstitutionDetailsQuery,
		GetInstitutionDetailsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetInstitutionDetailsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<GetInstitutionDetailsQueryResponse> Handle(GetInstitutionDetailsQuery request, CancellationToken cancellationToken)
	{
		long institutionId = request.InstitutionId;
		Institution? institution = (await _mediator.Send(new GetInstitutionQuery()
		{
			InstitutionId= institutionId
		})).Institution;

		if (institution is null)
		{
			return new() { Success = false };
		}
		InstitutionDetails result = _mapper.Map<InstitutionDetails>(institution);
		result.Names = (await _mediator.Send(new GetInstitutionNamesQuery()
		{
			InstitutionId = institutionId
		})).InstitutionNames;

		result.ExternDatabaseIds = (await _mediator.Send(new GetInstitutionExternDatabaseIdsQuery()
		{
			InstitutionId = institutionId
		})).InstitutionExternDatabaseIds;


		return new() { Success = true, InstitutionDetails = result };
	}
}
