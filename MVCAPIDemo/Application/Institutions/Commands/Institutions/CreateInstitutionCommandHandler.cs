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
		InstitutionModel institutionModel = _mapper.Map<InstitutionModel>(request);
		institutionModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			institutionModel.CreatedAt = DateTime.UtcNow;
		}
		long institutionId = await _repository.InsertInstitutionAsync(institutionModel);
		Institution domain = _mapper.Map<Institution>(institutionModel);
		domain.Id = institutionId;
		return new() { Success = true, Institution = domain };
	}
}
