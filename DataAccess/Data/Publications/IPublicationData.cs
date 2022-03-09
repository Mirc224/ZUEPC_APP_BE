using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationData : IRepositoryBase<PublicationModel>
{
	Task<IEnumerable<PublicationModel>> GetAllPublicationsAsync();
}
