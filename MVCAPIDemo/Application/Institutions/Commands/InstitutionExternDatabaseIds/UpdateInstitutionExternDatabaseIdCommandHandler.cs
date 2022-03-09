using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class UpdateInstitutionExternDatabaseIdCommandHandler :
	IRequestHandler<
		UpdateInstitutionExternDatabaseIdCommand,
		UpdateInstitutionExternDatabaseIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionExternDatabaseIdData _repository;

	public UpdateInstitutionExternDatabaseIdCommandHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdateInstitutionExternDatabaseIdCommandResponse> Handle(UpdateInstitutionExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		InstitutionExternDatabaseIdModel? currentModel = await _repository.GetModelByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		InstitutionExternDatabaseIdModel updatedModel = _mapper.Map<InstitutionExternDatabaseIdModel>(request);
		int rowsUpdated = await _repository.UpdateModelAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
