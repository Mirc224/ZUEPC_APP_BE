using MediatR;

namespace ZUEPC.Application.Import.Commands;

public class ImportDaWinciXmlCommand :
	ImportXmlBaseCommand,
	IRequest<ImportDaWinciXmlCommandResponse>
{
}
