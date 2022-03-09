﻿using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class UpdatePublicationAuthorCommandHandler :
	EPCUpdateSimpleModelCommandHandlerBase<PublicationAuthorModel>,
	IRequestHandler<
		UpdatePublicationAuthorCommand, 
		UpdatePublicationAuthorCommandResponse>
{
	public UpdatePublicationAuthorCommandHandler(IMapper mapper, IPublicationAuthorData repository)
	: base(mapper, repository) { }

	public async Task<UpdatePublicationAuthorCommandResponse> Handle(UpdatePublicationAuthorCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePublicationAuthorCommand,
			UpdatePublicationAuthorCommandResponse>(request);
	}
}
