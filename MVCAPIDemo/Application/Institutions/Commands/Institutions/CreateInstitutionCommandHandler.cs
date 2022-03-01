using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionCommandHandler : IRequestHandler<CreateInstitutionCommand, CreateInstitutionCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionData _repository;

	public CreateInstitutionCommandHandler(IMapper mapper, IInstitutionData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreateInstitutionCommandResponse> Handle(CreateInstitutionCommand request, CancellationToken cancellationToken)
	{
		InstitutionModel insertModel = _mapper.Map<InstitutionModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			insertModel.CreatedAt = DateTime.UtcNow;
		}
		long institutionId = await _repository.InsertInstitutionAsync(insertModel);
		Institution domain = _mapper.Map<Institution>(insertModel);
		domain.Id = institutionId;
		return new() { Success = true, Institution = domain };
	}
}
