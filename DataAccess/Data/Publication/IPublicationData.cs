using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publication;

public interface IPublicationData
{
	Task<IEnumerable<PublicationModel>> GetAllPublicationsAsync();
	Task<PublicationModel?> GetPublicationByIdAsync(long id);
	Task<long> InsertPublicationAsync(PublicationModel model);
	Task<int> DeletePublicationByIdAsync(long id);
	Task<int> UpdatePublicationAsync(PublicationModel model);
}
