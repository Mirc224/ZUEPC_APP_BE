using MediatR;

namespace ZUEPC.Application.Import.Commands;

public class ImportCREPCXmlCommand : 
	ImportXmlBaseCommand,
	IRequest<ImportCREPCXmlCommandResponse>
{
}
