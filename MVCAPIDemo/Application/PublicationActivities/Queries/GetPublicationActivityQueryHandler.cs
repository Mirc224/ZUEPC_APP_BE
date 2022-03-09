using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivityQueryHandler : IRequestHandler<GetPublicationActivityQuery, GetPublicationActivityQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationActivityData _repository;

	public GetPublicationActivityQueryHandler(IMapper mapper, IPublicationActivityData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationActivityQueryResponse> Handle(GetPublicationActivityQuery request, CancellationToken cancellationToken)
	{
		PublicationActivityModel? result = await _repository.GetPublicationActivityByIdAsync(request.PublicationActivityRecordId);
		if (result is null)
		{
			return new() { Success = false };
		}
		PublicationActivity mappedResult = _mapper.Map<PublicationActivity>(result);
		return new() { Success = true, PublicationActivity = mappedResult };
	}
}
