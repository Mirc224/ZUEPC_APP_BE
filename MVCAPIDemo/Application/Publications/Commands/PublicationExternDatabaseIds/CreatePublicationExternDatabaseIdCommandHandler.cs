using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class CreatePublicationExternDatabaseIdCommandHandler : 
	CreateBaseHandler,
	IRequestHandler<CreatePublicationExternDatabaseIdCommand, CreatePublicationExternDatabaseIdCommandResponse>
{
	private readonly IPublicationExternDatabaseIdData _repository;

	public CreatePublicationExternDatabaseIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<CreatePublicationExternDatabaseIdCommandResponse> Handle(CreatePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		PublicationExternDatabaseIdModel insertModel = CreateInsertModelFromRequest
			<PublicationExternDatabaseIdModel, CreatePublicationExternDatabaseIdCommand>(request);
		long insertedId = await _repository.InsertModelAsync(insertModel);
		return CreateSuccessResponseWithDataFromInsertModel
			<CreatePublicationExternDatabaseIdCommandResponse,
			PublicationExternDatabaseId,
			PublicationExternDatabaseIdModel>(insertModel, insertedId);
	}
}
