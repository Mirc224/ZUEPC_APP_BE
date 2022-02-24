using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.DataAccess.Data.Publications;

public interface IPublicationNameData
{
	Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByNameAsync(string name);
	Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByNameTypeAsync(string type);
	Task<IEnumerable<PublicationNameModel>> GetPublicationNamesByPublicationIdAsync(long publicationId);
	Task<PublicationNameModel?> GetPublicationNameByIdAsync(long nameId);
	Task<long> InsertPublicationNameAsync(PublicationNameModel model);
	Task<int> DeletePublicationNameByIdAsync(long id);
	Task<int> DeletePublicationNamesByPublicationIdAsync(long publicationId);
	Task<int> UpdatePublicationNameAsync(PublicationNameModel model);
}
