using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.DataAccess.Data.PublicationAuthor;

public interface IPublicationAuthorData
{
	Task<PublicationAuthorModel?> GetPublicationAuthorByIdAsync(long id);
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPersonIdAsync(long personId);
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByInstitutionIdAsync(long institutionId);
	Task<long> InsertPublicationAuthorAsync(PublicationAuthorModel model);
	Task<int> UpdatePublicationAuthorAsync(PublicationAuthorModel model);
	Task<int> DeletePublicationAuthorByIdAsync(long id);
	Task<int> DeletePublicationAuthorsByPersonIdAsync(long personId);
	Task<int> DeletePublicationAuthorsByInstitutionIdAsync(long institutionId);
	Task<int> DeletePublicationAuthorsByPersonIdAndInstitutionIdAsync(long personId, long institutionId);
}
