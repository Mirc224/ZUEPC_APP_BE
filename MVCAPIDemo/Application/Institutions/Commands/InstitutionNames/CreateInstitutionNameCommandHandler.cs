using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Commands.InstitutionNames;

public class CreateInstitutionNameCommandHandler :
	CreateBaseHandler,
	IRequestHandler<
		CreateInstitutionNameCommand,
		CreateInstitutionNameCommandResponse>
{
	private readonly IInstitutionNameData _repository;

	public CreateInstitutionNameCommandHandler(IMapper mapper, IInstitutionNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreateInstitutionNameCommandResponse> Handle(CreateInstitutionNameCommand request, CancellationToken cancellationToken)
	{
		InstitutionNameModel insertModel = CreateInsertModelFromRequest
			<InstitutionNameModel, CreateInstitutionNameCommand>(request);

		long insertedId = await _repository.InsertModelAsync(insertModel);
		return CreateSuccessResponseWithDataFromInsertModel
			<CreateInstitutionNameCommandResponse,
			InstitutionName,
			InstitutionNameModel>(insertModel, insertedId);
	}
}
