using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class UpdateInstitutionCommandHandler : IRequestHandler<UpdateInstitutionCommand, UpdateInstitutionCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionData _repository;

	public UpdateInstitutionCommandHandler(IMapper mapper, IInstitutionData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<UpdateInstitutionCommandResponse> Handle(UpdateInstitutionCommand request, CancellationToken cancellationToken)
	{
		InstitutionModel? institutionRecord = await _repository.GetModelByIdAsync(request.Id);
		if (institutionRecord is null)
		{
			return new() { Success = false };
		}

		InstitutionModel institutionModel = _mapper.Map<InstitutionModel>(request);
		int rowsUpdated = await _repository.UpdateModelAsync(institutionModel);

		return new() { Success = rowsUpdated == 1 };
	}
}
