using MediatR;

namespace ZUEPC.Application.Import.Commands;

public class ImportDaWinciXmlCommandHandler : IRequestHandler<ImportDaWinciXmlCommand, ImportDaWinciXmlCommandResponse>
{
	
	public async Task<ImportDaWinciXmlCommandResponse> Handle(ImportDaWinciXmlCommand request, CancellationToken cancellationToken)
	{
		return new() { Success = true };
	}
}
