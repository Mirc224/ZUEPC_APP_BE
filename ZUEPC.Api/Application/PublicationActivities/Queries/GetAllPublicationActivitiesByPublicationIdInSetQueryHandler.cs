using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;

namespace ZUEPC.Api.Application.PublicationActivities.Queries;

public class GetAllPublicationActivitiesByPublicationIdInSetQueryHandler :
	IRequestHandler<GetAllPublicationActivitiesByPublicationIdInSetQuery, GetAllPublicationActivitiesByPublicationIdInSetQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationActivityData _repository;

	public GetAllPublicationActivitiesByPublicationIdInSetQueryHandler(IMapper mapper, IPublicationActivityData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetAllPublicationActivitiesByPublicationIdInSetQueryResponse> Handle(GetAllPublicationActivitiesByPublicationIdInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<PublicationActivityModel> activities = await _repository.GetAllPublicationActivitiesByPublicationIdInSetAsync(request.PublicationIds);
		if (activities is null)
		{
			return new() { Success = false };
		}

		IEnumerable<PublicationActivity> mappedResult = _mapper.Map<List<PublicationActivity>>(activities);
		return new() { Success = true, Data = mappedResult };
	}
}
