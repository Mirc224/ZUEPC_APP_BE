using MediatR;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.DataAccess.Data.Institutions;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class DeleteInstitutionCommandHandler : IRequestHandler<DeleteInstitutionCommand, DeleteInstitutionCommandResponse>
{
	private readonly IInstitutionData _repository;
	private readonly IMediator _mediator;

	public DeleteInstitutionCommandHandler(IInstitutionData repository, IMediator mediator)
	{
		_repository = repository;
		_mediator = mediator;
	}
	public async Task<DeleteInstitutionCommandResponse> Handle(DeleteInstitutionCommand request, CancellationToken cancellationToken)
	{ 
		long institutionId = request.Id;

		GetInstitutionQueryResponse model = await _mediator.Send(new GetInstitutionQuery() { Id = institutionId });

		if (!model.Success)
		{
			return new() { Success = false };
		}
		await _mediator.Send(new DeleteInstitutionNamesByInstitutionIdCommand() { InstitutionId = institutionId });
		await _mediator.Send(new DeleteInstitutionExternDatabaseIdsByInstitutionIdCommand() { InstitutionId = institutionId });
		await _mediator.Send(new DeletePublicationAuthorsByInstitutionIdCommand() { InstitutionId = institutionId });
		await _repository.DeleteModelByIdAsync(institutionId);
		return new() { Success = true };
	}
}
