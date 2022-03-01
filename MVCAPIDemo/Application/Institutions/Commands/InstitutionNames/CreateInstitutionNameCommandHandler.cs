using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class CreateInstitutionNameCommandHandler :
	IRequestHandler<
		CreateInstitutionNameCommand,
		CreateInstitutionNameCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionNameData _repository;

	public CreateInstitutionNameCommandHandler(IMapper mapper, IInstitutionNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreateInstitutionNameCommandResponse> Handle(CreateInstitutionNameCommand request, CancellationToken cancellationToken)
	{
		InstitutionNameModel insertModel = _mapper.Map<InstitutionNameModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			insertModel.CreatedAt = DateTime.UtcNow;
		}
		long insertedId = await _repository.InsertInstitutionNameAsync(insertModel);
		InstitutionName domain = _mapper.Map<InstitutionName>(insertModel);
		domain.InstitutionId = insertedId;

		return new() { Success = true, InstitutionName = domain };
	}
}
