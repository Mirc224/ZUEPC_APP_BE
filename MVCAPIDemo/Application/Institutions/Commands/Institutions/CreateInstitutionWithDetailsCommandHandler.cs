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

		List<InstitutionName> InstitutionNames = new();
		foreach (InstitutionNameCreateDto name in request.Names.OrEmptyIfNull())
		{
			name.InstitutionId = createdInstitution.Id;
			CreateInstitutionNameCommand createInstitutionNameCommand = _mapper.Map<CreateInstitutionNameCommand>(name);
			createInstitutionNameCommand.OriginSourceType = request.OriginSourceType;
			createInstitutionNameCommand.VersionDate = request.VersionDate;
			InstitutionName createdName = (await _mediator.Send(createInstitutionNameCommand)).InstitutionName;
			InstitutionNames.Add(createdName);
		}
		responseObject.Names = InstitutionNames;

		List<InstitutionExternDatabaseId> InstitutionExternIds = new();
		foreach (InstitutionExternDatabaseIdCreateDto externIdentifier in request.ExternDatabaseIds.OrEmptyIfNull())
		{
			externIdentifier.InstitutionId = createdInstitution.Id;
			CreateInstitutionExternDatabaseIdCommand createInstitutionIdentifierCommand = _mapper.Map<CreateInstitutionExternDatabaseIdCommand>(externIdentifier);
			createInstitutionIdentifierCommand.OriginSourceType = request.OriginSourceType;
			createInstitutionIdentifierCommand.VersionDate = request.VersionDate;
			InstitutionExternDatabaseId createdExternDbId = (await _mediator.Send(createInstitutionIdentifierCommand)).InstitutionExternDatabaseId;
			InstitutionExternIds.Add(createdExternDbId);
		}
		responseObject.ExternDatabaseIds = InstitutionExternIds;
		return new() { Success = true, CreatedInstitutionDetails = responseObject };
	}
}
