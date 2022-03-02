using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetPersonPreviewQueryResponse : ResponseBase
{
	public PersonPreview? PersonPreview { get; set; }
}