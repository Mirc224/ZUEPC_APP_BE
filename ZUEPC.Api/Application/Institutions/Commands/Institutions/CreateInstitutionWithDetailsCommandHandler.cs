using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Entities.Inputs.Common;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionWithDetailsCommandHandler :
	IRequestHandler<
		CreateInstitutionWithDetailsCommand,
		CreateInstitutionWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public CreateInstitutionWithDetailsCommandHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<CreateInstitutionWithDetailsCommandResponse> Handle(CreateInstitutionWithDetailsCommand request, CancellationToken cancellationToken)
	{
		CreateInstitutionCommand createInstitutionCommand = _mapper.Map<CreateInstitutionCommand>(request);
		Institution? createdInstitution = (await _mediator.Send(createInstitutionCommand)).Data;
		if(createdInstitution is null)
		{
			return new() { Success = false };
		}
		InstitutionDetails responseObject = _mapper.Map<InstitutionDetails>(createdInstitution);
		long institutionId = createdInstitution.Id;
		
		responseObject.Names = await ProcessInstitutionNamesAsync(request, institutionId);

		
		responseObject.ExternDatabaseIds = await ProcessInstitutionExternDatabaseIdsAsync(request, institutionId);
		return new() { Success = true, Data = responseObject };
	}

	private async Task<ICollection<InstitutionName>> ProcessInstitutionNamesAsync(CreateInstitutionWithDetailsCommand request, long institutionId)
	{
		ICollection<CreateInstitutionNameCommandResponse> responses = await ProcessInstitutionPropertyAsync<
			CreateInstitutionNameCommandResponse,
			InstitutionNameCreateDto,
			CreateInstitutionNameCommand>(request, request.Names, institutionId);

		return responses.Select(x => x.Data).ToList();
	}

	private async Task<ICollection<InstitutionExternDatabaseId>> ProcessInstitutionExternDatabaseIdsAsync(
		CreateInstitutionWithDetailsCommand request,
		long institutionId)
	{
		ICollection<CreateInstitutionExternDatabaseIdCommandResponse> responses = await ProcessInstitutionPropertyAsync<
			CreateInstitutionExternDatabaseIdCommandResponse,
			InstitutionExternDatabaseIdCreateDto,
			CreateInstitutionExternDatabaseIdCommand>(request, request.ExternDatabaseIds, institutionId);

		return responses.Select(x => x.Data).ToList();
	}

	private async Task<ICollection<TResponse>> ProcessInstitutionPropertyAsync<TResponse, TCreateDto, TCommand>(
		CreateInstitutionWithDetailsCommand request,
		IEnumerable<TCreateDto>? propertyObjects,
		long institutionId)
		where TCreateDto : InstitutionPropertyBaseDto
	{
		List<TResponse> responses = new();
		foreach (TCreateDto propertyObject in propertyObjects.OrEmptyIfNull())
		{
			propertyObject.InstitutionId = institutionId;
			propertyObject.OriginSourceType = request.OriginSourceType;
			propertyObject.VersionDate = request.VersionDate;
			TCommand createPropertyObjectCommand = _mapper.Map<TCommand>(propertyObject);
			TResponse createdResponse = (TResponse)(await _mediator.Send(createPropertyObjectCommand));
			responses.Add(createdResponse);
		}
		return responses;
	}
}
