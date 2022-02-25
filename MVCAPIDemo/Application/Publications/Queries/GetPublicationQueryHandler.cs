﻿using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries;

public class GetPublicationQueryHandler : IRequestHandler<GetPublicationQuery, GetPublicationQueryResponse>
{
	private readonly IPublicationData _repository;
	private readonly IMapper _mapper;

	public GetPublicationQueryHandler(IMapper mapper, IPublicationData repository)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<GetPublicationQueryResponse> Handle(GetPublicationQuery request, CancellationToken cancellationToken)
	{
		var result = await _repository.GetPublicationByIdAsync(request.PublicationId);
		if (result is null)
		{
			return new GetPublicationQueryResponse() { Success = false };
		}
		var mappedResult = _mapper.Map<Publication>(result);
		return new() { Success = true, Publication = mappedResult };
	}
}
