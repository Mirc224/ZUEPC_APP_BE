using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;

namespace ZUEPC.Api.Application.PublicationActivities.Queries;

public class GetAllPublicationActivitiesByPublicationIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<PublicationActivity>>
{
}