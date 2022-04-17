using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationData :
	IRepositoryBase<PublicationModel>,
	IRepositoryWithSimpleIdBase<PublicationModel, long>,
	IRepositoryWithFilter<PublicationModel, PublicationFilter>
{
}
