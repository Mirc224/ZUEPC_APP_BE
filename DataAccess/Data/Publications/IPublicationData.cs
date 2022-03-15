using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationData :
	IRepositoryBase<PublicationModel>,
	IRepositoryWithFilter<PublicationModel, PublicationFilter>
{
}
