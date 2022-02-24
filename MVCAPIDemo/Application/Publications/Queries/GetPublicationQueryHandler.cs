using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Queries;

public class GetPublicationQueryHandler : IRequestHandler<GetPublicationQuery, GetPublicationQueryResponse>
{
	private readonly IPublicationData _repository;

	public GetPublicationQueryHandler(IPublicationData repository)
	{
		_repository = repository;
	}

	public async Task<GetPublicationQueryResponse> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
	{
		var result = await _repository.GetPublicationByIdAsync(request.PublicationId);
		if (result is null)
		{
			return new GetPublicationQueryResponse() { Success = false };
		}
		return new() { Success = true, Publication = result };
	}
}
