using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.DataAccess.Data.PublicationAuthors;

public interface IPublicationAuthorData : 
	IRepositoryBase<PublicationAuthorModel>,
	IRepositoryWithSimpleIdBase<PublicationAuthorModel, long>
{
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPublicationIdAsync(long publicationId);
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPersonIdAsync(long personId);
	Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByInstitutionIdAsync(long institutionId);
	Task<int> DeletePublicationAuthorsByPersonIdAsync(long personId);
	Task<int> DeletePublicationAuthorsByPublicationIdAsync(long publicationId);
	Task<int> DeletePublicationAuthorsByInstitutionIdAsync(long institutionId);
}
