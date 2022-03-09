﻿using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Institutions;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;

public class UpdateInstitutionExternDatabaseIdCommandHandler :
	EPCUpdateSimpleModelCommandHandlerBase<InstitutionExternDatabaseIdModel>,
	IRequestHandler<
		UpdateInstitutionExternDatabaseIdCommand,
		UpdateInstitutionExternDatabaseIdCommandResponse>
{
	public UpdateInstitutionExternDatabaseIdCommandHandler(IMapper mapper, IInstitutionExternDatabaseIdData repository)
		: base(mapper, repository) { }

	public async Task<UpdateInstitutionExternDatabaseIdCommandResponse> Handle(UpdateInstitutionExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdateInstitutionExternDatabaseIdCommand, 
			UpdateInstitutionExternDatabaseIdCommandResponse>(request);
	}
}
