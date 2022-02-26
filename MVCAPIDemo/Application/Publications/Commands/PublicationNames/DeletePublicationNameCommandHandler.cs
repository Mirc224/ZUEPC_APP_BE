using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class DeletePublicationNameCommandHandler : IRequestHandler<DeletePublicationNameCommand, DeletePublicationNameCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public DeletePublicationNameCommandHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<DeletePublicationNameCommandResponse> Handle(DeletePublicationNameCommand request, CancellationToken cancellationToken)
	{
		var rowsDeleted = await _repository.DeletePublicationNameByIdAsync(request.Id);

		return new DeletePublicationNameCommandResponse() { Success = rowsDeleted == 1 };
	}
}
