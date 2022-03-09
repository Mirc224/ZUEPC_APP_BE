using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class CreatePublicationAuthorCommandHandler : 
	CreateBaseHandler,
	IRequestHandler<CreatePublicationAuthorCommand, CreatePublicationAuthorCommandResponse>
{
	private readonly IPublicationAuthorData _repository;

	public CreatePublicationAuthorCommandHandler(IMapper mapper, IPublicationAuthorData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePublicationAuthorCommandResponse> Handle(CreatePublicationAuthorCommand request, CancellationToken cancellationToken)
	{
		PublicationAuthorModel insertModel = CreateInsertModelFromRequest
			<PublicationAuthorModel, CreatePublicationAuthorCommand>(request);
		long insertedId = await _repository.InsertModelAsync(insertModel);
		
		return CreateSuccessResponseWithDataFromInsertModel
			<CreatePublicationAuthorCommandResponse,
			PublicationAuthor,
			PublicationAuthorModel>(insertModel, insertedId);
	}
}
