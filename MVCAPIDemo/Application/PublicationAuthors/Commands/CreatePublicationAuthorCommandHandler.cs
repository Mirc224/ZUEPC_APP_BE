using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class CreatePublicationAuthorCommandHandler : IRequestHandler<CreatePublicationAuthorCommand, CreatePublicationAuthorCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationAuthorData _repository;

	public CreatePublicationAuthorCommandHandler(IMapper mapper, IPublicationAuthorData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePublicationAuthorCommandResponse> Handle(CreatePublicationAuthorCommand request, CancellationToken cancellationToken)
	{
		PublicationAuthorModel insertModel = _mapper.Map<PublicationAuthorModel>(request);
		long insertedId = await _repository.InsertPublicationAuthorAsync(insertModel);
		PublicationAuthor domain = _mapper.Map<PublicationAuthor>(insertModel);
		insertModel.Id = insertedId;
		return new() { Success = true, PublicationAuthor = domain };
	}
}
