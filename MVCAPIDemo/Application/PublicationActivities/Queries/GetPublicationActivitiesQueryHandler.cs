using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivitiesQueryHandler : IRequestHandler<GetPublicationActivitiesQuery, GetPublicationActivitiesQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationActivityData _repository;

	public GetPublicationActivitiesQueryHandler(IMapper mapper, IPublicationActivityData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetPublicationActivitiesQueryResponse> Handle(GetPublicationActivitiesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationActivityModel> queryResult = await _repository.GetPublicationActivitiesByPublicationIdAsync(request.PublicationId);

		List<PublicationActivity> mappedResult = _mapper.Map<List<PublicationActivity>>(queryResult);
		return new() { Success = true, PublicationActivities = mappedResult };
	}
}
