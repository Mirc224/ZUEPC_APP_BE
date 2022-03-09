using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class CreateInstitutionExternDatabaseIdCommandHandler : 
	CreateBaseHandler,
	IRequestHandler<
		CreateInstitutionExternDatabaseIdCommand, 
		CreateInstitutionExternDatabaseIdCommandResponse>
{
	private readonly IInstitutionExternDatabaseIdData _repository;

	public CreateInstitutionExternDatabaseIdCommandHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<CreateInstitutionExternDatabaseIdCommandResponse> Handle(CreateInstitutionExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		InstitutionExternDatabaseIdModel insertModel = 
			CreateInsertModelFromRequest<InstitutionExternDatabaseIdModel, CreateInstitutionExternDatabaseIdCommand>(request);
		
		long insertedId = await _repository.InsertModelAsync(insertModel);
		return CreateSuccessResponseWithDataFromInsertModel
			<CreateInstitutionExternDatabaseIdCommandResponse,
			InstitutionExternDatabaseId,
			InstitutionExternDatabaseIdModel>(insertModel, insertedId);
	}
}
