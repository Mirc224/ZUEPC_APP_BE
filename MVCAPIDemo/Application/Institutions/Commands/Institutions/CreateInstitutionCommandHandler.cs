using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class CreateInstitutionCommandHandler : 
	CreateBaseHandler,
	IRequestHandler<CreateInstitutionCommand, CreateInstitutionCommandResponse>
{
	private readonly IInstitutionData _repository;

	public CreateInstitutionCommandHandler(IMapper mapper, IInstitutionData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreateInstitutionCommandResponse> Handle(CreateInstitutionCommand request, CancellationToken cancellationToken)
	{
		InstitutionModel insertModel = CreateInsertModelFromRequest
			<InstitutionModel, CreateInstitutionCommand>(request);

		long insertedId = await _repository.InsertModelAsync(insertModel);
		return CreateSuccessResponseWithDataFromInsertModel
			<CreateInstitutionCommandResponse,
			Institution,
			InstitutionModel>(insertModel, insertedId);
	}
}
