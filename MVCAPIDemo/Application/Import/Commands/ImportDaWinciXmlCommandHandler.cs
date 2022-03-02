using MediatR;
using ZUEPC.Application.Import.Services;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Import.Commands;

public class ImportDaWinciXmlCommandHandler : IRequestHandler<ImportDaWinciXmlCommand, ImportDaWinciXmlCommandResponse>
{
	private readonly ImportService _importService;

	public ImportDaWinciXmlCommandHandler(ImportService importService)
	{
		_importService = importService;
	}

	public async Task<ImportDaWinciXmlCommandResponse> Handle(ImportDaWinciXmlCommand request, CancellationToken cancellationToken)
	{
		ICollection<Publication>? importedPublications = await _importService.ImportFromDaWinciXML(request);
		if (importedPublications is null)
		{
			return new() { Success = false };
		}

		ICollection<long> importedPublicationsIds = importedPublications.Select(x => x.Id).ToList();
		return new() { Success = true, PublicationsIds = importedPublicationsIds };
	}
}
