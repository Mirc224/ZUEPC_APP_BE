using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class CreateInstitutionExternDatabaseIdCommandHandler : 
	IRequestHandler<
		CreateInstitutionExternDatabaseIdCommand, 
		CreateInstitutionExternDatabaseIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IInstitutionExternDatabaseIdData _repository;

	public CreateInstitutionExternDatabaseIdCommandHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<CreateInstitutionExternDatabaseIdCommandResponse> Handle(CreateInstitutionExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		InstitutionExternDatabaseIdModel insertModel = _mapper.Map<InstitutionExternDatabaseIdModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			insertModel.CreatedAt = DateTime.UtcNow;
		}
		long insertedId = await _repository.InsertInstitutionExternDatabaseIdAsync(insertModel);
		InstitutionExternDatabaseId domain = _mapper.Map<InstitutionExternDatabaseId>(insertModel);
		domain.Id = insertedId;
		return new() { Success = true, InstitutionExternDatabaseId = domain };
	}
}
