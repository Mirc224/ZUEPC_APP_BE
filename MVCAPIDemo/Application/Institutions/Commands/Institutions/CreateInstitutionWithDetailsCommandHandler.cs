using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;
using ZUEPC.Common.Extensions;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

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
		Institution createdInstitution = (await _mediator.Send(createInstitutionCommand)).Institution;
		InstitutionDetails responseObject = _mapper.Map<InstitutionDetails>(createdInstitution);
		long institutionId = createdInstitution.Id;
		
		responseObject.Names = await ProcessInstitutionNamesAsync(request, institutionId);

		
		responseObject.ExternDatabaseIds = await ProcessInstitutionExternDatabaseIdsAsync(request, institutionId);
		return new() { Success = true, CreatedInstitutionDetails = responseObject };
	}

	private async Task<ICollection<InstitutionName>> ProcessInstitutionNamesAsync(CreateInstitutionWithDetailsCommand request, long institutionId)
	{
		List<InstitutionName> institutionNames = new();
		foreach (InstitutionNameCreateDto name in request.Names.OrEmptyIfNull())
		{
			name.InstitutionId = institutionId;
			CreateInstitutionNameCommand createInstitutionNameCommand = _mapper.Map<CreateInstitutionNameCommand>(name);
			createInstitutionNameCommand.OriginSourceType = request.OriginSourceType;
			createInstitutionNameCommand.VersionDate = request.VersionDate;
			InstitutionName createdName = (await _mediator.Send(createInstitutionNameCommand)).InstitutionName;
			institutionNames.Add(createdName);
		}
		
		return institutionNames;
	}

	private async Task<ICollection<InstitutionExternDatabaseId>> ProcessInstitutionExternDatabaseIdsAsync(
		CreateInstitutionWithDetailsCommand request,
		long institutionId)
	{
		List<InstitutionExternDatabaseId> institutionExternIds = new();
		foreach (InstitutionExternDatabaseIdCreateDto externIdentifier in request.ExternDatabaseIds.OrEmptyIfNull())
		{
			externIdentifier.InstitutionId = institutionId;
			CreateInstitutionExternDatabaseIdCommand createInstitutionIdentifierCommand = _mapper.Map<CreateInstitutionExternDatabaseIdCommand>(externIdentifier);
			createInstitutionIdentifierCommand.OriginSourceType = request.OriginSourceType;
			createInstitutionIdentifierCommand.VersionDate = request.VersionDate;
			InstitutionExternDatabaseId createdExternDbId = (await _mediator.Send(createInstitutionIdentifierCommand)).InstitutionExternDatabaseId;
			institutionExternIds.Add(createdExternDbId);
		}
		return institutionExternIds;
	}
}
