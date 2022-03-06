using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;
using ZUEPC.Common.Extensions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class UpdateInstitutionWithDetailsCommandHandler : 
	IRequestHandler<UpdateInstitutionWithDetailsCommand, UpdateInstitutionWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public UpdateInstitutionWithDetailsCommandHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<UpdateInstitutionWithDetailsCommandResponse> Handle(UpdateInstitutionWithDetailsCommand request, CancellationToken cancellationToken)
	{
		UpdateInstitutionCommand updateInstitutionCommand = _mapper.Map<UpdateInstitutionCommand>(request);
		bool updated = (await _mediator.Send(updateInstitutionCommand)).Success;
		if (!updated)
		{
			return new() { Success = false };
		}
		await ProcessInstitutionNamesAsync(request);

		await ProcessInstitutionExternDatabaseIdsAsync(request);

		return new() { Success = true };
	}

	private async Task ProcessInstitutionExternDatabaseIdsAsync(UpdateInstitutionWithDetailsCommand request)
	{
		long institutionId = request.Id;
		foreach (long idToDelete in request.ExternDatabaseIdsToDelete.OrEmptyIfNull())
		{
			DeleteInstitutionExternDatabaseIdsCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		foreach (InstitutionExternDatabaseIdUpdateDto externDbId in request.ExternDatabaseIdsToUpdate.OrEmptyIfNull())
		{
			externDbId.InstitutionId = institutionId;
			externDbId.VersionDate = request.VersionDate;
			externDbId.OriginSourceType = request.OriginSourceType;
			UpdateInstitutionExternDatabaseIdCommand updateCommand = _mapper.Map<UpdateInstitutionExternDatabaseIdCommand>(externDbId);
			await _mediator.Send(updateCommand);
		}

		foreach (InstitutionExternDatabaseIdCreateDto externDbId in request.ExternDatabaseIdsToInsert.OrEmptyIfNull())
		{
			externDbId.InstitutionId = institutionId;
			externDbId.VersionDate = request.VersionDate;
			externDbId.OriginSourceType = request.OriginSourceType;
			CreateInstitutionExternDatabaseIdCommand createInstitutionIdentifierCommand = _mapper.Map<CreateInstitutionExternDatabaseIdCommand>(externDbId);
			await _mediator.Send(createInstitutionIdentifierCommand);
		}
	}

	private async Task ProcessInstitutionNamesAsync(UpdateInstitutionWithDetailsCommand request)
	{
		long institutionId = request.Id;
		foreach (long nameIdToDelete in request.NamesToDelete.OrEmptyIfNull())
		{
			DeleteInstitutionNameCommand deleteCommand = new() { Id = nameIdToDelete };
			await _mediator.Send(deleteCommand);
		}

		foreach (InstitutionNameUpdateDto nameToUpdate in request.NamesToUpdate.OrEmptyIfNull())
		{
			nameToUpdate.InstitutionId = institutionId;
			nameToUpdate.VersionDate = request.VersionDate;
			nameToUpdate.OriginSourceType = request.OriginSourceType;
			UpdateInstitutionNameCommand updateCommand = _mapper.Map<UpdateInstitutionNameCommand>(nameToUpdate);
			await _mediator.Send(updateCommand);
		}

		foreach (InstitutionNameCreateDto name in request.NamesToInsert.OrEmptyIfNull())
		{
			name.InstitutionId = institutionId;
			name.VersionDate = request.VersionDate;
			name.OriginSourceType = request.OriginSourceType;
			CreateInstitutionNameCommand createInstitutionNameCommand = _mapper.Map<CreateInstitutionNameCommand>(name);
			await _mediator.Send(createInstitutionNameCommand);
		}
	}
}
