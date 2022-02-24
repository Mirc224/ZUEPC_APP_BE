using ZUEPC.Common.Responses;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Queries;

public class GetPublicationQueryResponse : ResponseBase
{
	public PublicationModel? Publication { get; set; }
}