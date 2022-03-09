using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.DataAccess.Data.PublicationAuthors;

public interface IPublicationAuthorData : IRepositoryBase<PublicationAuthorModel>
{
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPersonIdAsync(long personId);
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByInstitutionIdAsync(long institutionId);
	Task<int> DeletePublicationAuthorsByPersonIdAsync(long personId);
	Task<int> DeletePublicationAuthorsByInstitutionIdAsync(long institutionId);
	Task<int> DeletePublicationAuthorsByPersonIdAndInstitutionIdAsync(long personId, long institutionId);
}
