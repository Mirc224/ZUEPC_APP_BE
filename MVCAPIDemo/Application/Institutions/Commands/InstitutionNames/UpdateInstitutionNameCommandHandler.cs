using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class UpdateInstitutionNameCommandHandler : IRequestHandler<UpdateInstitutionNameCommand, UpdateInstitutionNameCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionNameData _repository;

	public UpdateInstitutionNameCommandHandler(IMapper mapper, IInstitutionNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<UpdateInstitutionNameCommandResponse> Handle(UpdateInstitutionNameCommand request, CancellationToken cancellationToken)
	{
		InstitutionNameModel? currentModel = await _repository.GetInstitutionNameByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		InstitutionNameModel updatedModel = _mapper.Map<InstitutionNameModel>(request);
		int rowsUpdated = await _repository.UpdateInstitutionNameAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
