using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class CreatePublicationAuthorCommandHandler : 
	EPCCreateSimpleModelCommandHandlerBase<PublicationAuthor, PublicationAuthorModel>,
	IRequestHandler<CreatePublicationAuthorCommand, CreatePublicationAuthorCommandResponse>
{
	public CreatePublicationAuthorCommandHandler(IMapper mapper, IPublicationAuthorData repository)
	: base(mapper, repository) { }

	public async Task<CreatePublicationAuthorCommandResponse> Handle(CreatePublicationAuthorCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePublicationAuthorCommand, CreatePublicationAuthorCommandResponse>(request);
	}
}
