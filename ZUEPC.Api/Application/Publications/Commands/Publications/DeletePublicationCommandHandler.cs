using MediatR;
using ZUEPC.Api.Application.PublicationActivities.Commands;
using ZUEPC.Api.Application.PublicationAuthors.Commands;
using ZUEPC.Api.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Api.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Api.Application.Publications.Commands.PublicationNames;
using ZUEPC.Api.Application.RelatedPublications.Commands;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class DeletePublicationCommandHandler : 
	IRequestHandler<
		DeletePublicationCommand, 
		DeletePublicationCommandResponse>
{
	private readonly IMediator _mediator;
	private readonly IPublicationData _repository;

	public DeletePublicationCommandHandler(IMediator mediator, IPublicationData repository)
	{
		_repository = repository;
		_mediator = mediator;
	}
	public async Task<DeletePublicationCommandResponse> Handle(DeletePublicationCommand request, CancellationToken cancellationToken)
	{
		long publicationId = request.PublicationId;

		GetPublicationQueryResponse model = await _mediator.Send(new GetPublicationQuery() { Id = publicationId });

		if (!model.Success)
		{
			return new() { Success = false };
		}
		await _mediator.Send(new DeletePublicationNamesByPublicationIdCommand() { PublicationId = publicationId });
		await _mediator.Send(new DeletePublicationIdentifiersByPublicationIdCommand() { PublicationId = publicationId });
		await _mediator.Send(new DeletePublicationExternDatabaseIdsByPublicationIdCommand() { PublicationId = publicationId });
		await _mediator.Send(new DeletePublicationAuthorsByPublicationIdCommand() { PublicationId = publicationId });
		await _mediator.Send(new DeleteRelatedPublicationsByPublicationIdCommand() { PublicationId = publicationId });
		await _mediator.Send(new DeleteRelatedPublicationsByRelatedPublicationIdCommand() { RelatedPublicationId = publicationId });
		await _mediator.Send(new DeletePublicationActivitiesByPublicationIdCommand() { PublicationId = publicationId });
		await _repository.DeleteModelByIdAsync(publicationId);
		return new() { Success = true };
	}
}
