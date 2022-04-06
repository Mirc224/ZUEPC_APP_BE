using MediatR;
using ZUEPC.Application.Import.Services;
using ZUEPC.EvidencePublication.Domain.Publications;

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
		IEnumerable<Publication>? importedPublications = await _importService.ImportFromCREPCXML(request);
		if(importedPublications is null)
		{
			return new() { Success = false };
		}

		ICollection<long> importedPublicationsIds = importedPublications.Select(x => x.Id).ToList();

		return new() { Success = true, PublicationsIds = importedPublicationsIds };
	}
}
