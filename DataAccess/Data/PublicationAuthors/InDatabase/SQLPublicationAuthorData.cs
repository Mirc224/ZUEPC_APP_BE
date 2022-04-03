using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.DataAccess.Data.PublicationAuthors.InDatabase;

public class SQLPublicationAuthorData :
	SQLDbEPCRepositoryBase<PublicationAuthorModel>,
	IPublicationAuthorData
{
	public SQLPublicationAuthorData(ISqlDataAccess db) :
		base(db, TableNameConstants.PUBLICATION_AUTHORS_TABLE, TableAliasConstants.PUBLICATION_AUTHORS_TABLE_ALIAS)
	{
	}

	public async Task<int> DeletePublicationAuthorsByInstitutionIdAsync(long institutionId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PublicationAuthorModel.InstitutionId), institutionId);
	}

	public async Task<int> DeletePublicationAuthorsByPersonIdAsync(long personId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PublicationAuthorModel.PersonId), personId);
	}

	public async Task<int> DeletePublicationAuthorsByPublicationIdAsync(long publicationId)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(PublicationAuthorModel.PublicationId), publicationId);
	}

	public async Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByInstitutionIdAsync(long institutionId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PublicationAuthorModel.InstitutionId), institutionId);
	}

	public async Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPersonIdAsync(long personId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PublicationAuthorModel.PersonId), personId);
	}

	public async Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPublicationIdAsync(long publicationId)
	{
		return await GetModelsWithColumnValueAsync(nameof(PublicationAuthorModel.PublicationId), publicationId);
	}
}
