using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Persons;

namespace ZUEPC.Api.Application.Persons.Queries.Persons.Previews.BaseHandlers;

public class GetAllPersonPreviewDataForPersonIdsInSetQueryResponse : ResponseBase
{
	public IEnumerable<PersonName> PersonNames { get; set; }
	public IEnumerable<PersonExternDatabaseId> PersonExternDatabaseIds { get; set; }
}