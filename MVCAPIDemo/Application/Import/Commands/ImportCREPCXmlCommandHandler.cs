using MediatR;
using ZUEPC.Application.Import.Services;

namespace ZUEPC.Application.Import.Commands;

public class ImportCREPCXmlCommandHandler : IRequestHandler<ImportCREPCXmlCommand, ImportCREPCXmlCommandResponse>
{
	private readonly ImportService _importService;

	public ImportCREPCXmlCommandHandler(ImportService importService)
	{
		_importService = importService;
	}
	public async Task<ImportCREPCXmlCommandResponse> Handle(ImportCREPCXmlCommand request, CancellationToken cancellationToken)
	{
		_importService.ImportFromCREPCXML(request);
		return new() { Success = true };
	}
}
