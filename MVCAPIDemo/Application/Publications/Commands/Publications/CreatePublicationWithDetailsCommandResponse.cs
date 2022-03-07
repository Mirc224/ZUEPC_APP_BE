using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationWithDetailsCommandResponse : ResponseBase
{
	public PublicationDetails CreatedPublicationDetails { get; set; }
}