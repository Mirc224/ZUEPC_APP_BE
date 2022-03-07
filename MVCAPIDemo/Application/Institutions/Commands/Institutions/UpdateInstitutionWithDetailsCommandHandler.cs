using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Inputs.Common;
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
			DeleteInstitutionExternDatabaseIdCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessInstitutionPropertyAsync<InstitutionExternDatabaseIdUpdateDto, UpdateInstitutionExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToUpdate,
			institutionId);

		await ProcessInstitutionPropertyAsync<InstitutionExternDatabaseIdCreateDto, CreateInstitutionExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToInsert,
			institutionId);
	}

	private async Task ProcessInstitutionNamesAsync(UpdateInstitutionWithDetailsCommand request)
	{
		long institutionId = request.Id;
		foreach (long nameIdToDelete in request.NamesToDelete.OrEmptyIfNull())
		{
			DeleteInstitutionNameCommand deleteCommand = new() { Id = nameIdToDelete };
			await _mediator.Send(deleteCommand);
		}


		await ProcessInstitutionPropertyAsync<InstitutionNameUpdateDto,UpdateInstitutionNameCommand>(
			request,
			request.NamesToUpdate, 
			institutionId);

		await ProcessInstitutionPropertyAsync<InstitutionNameCreateDto,CreateInstitutionNameCommand>(
			request, 
			request.NamesToInsert,
			institutionId);
	}

	private async Task ProcessInstitutionPropertyAsync<TDto, TCommand>(
		UpdateInstitutionWithDetailsCommand request,
		IEnumerable<TDto>? propertyObjects,
		long institutionId)
		where TDto : InstitutionPropertyBaseDto
	{
		foreach (TDto propertyObject in propertyObjects.OrEmptyIfNull())
		{
			propertyObject.InstitutionId = institutionId;
			propertyObject.OriginSourceType = request.OriginSourceType;
			propertyObject.VersionDate = request.VersionDate;
			TCommand actionCommand = _mapper.Map<TCommand>(propertyObject);
			await _mediator.Send(actionCommand);
		}
	}
}
