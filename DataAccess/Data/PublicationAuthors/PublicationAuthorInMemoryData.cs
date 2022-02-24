using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.DataAccess.Data.PublicationAuthors;

public class PublicationAuthorInMemoryData : InMemoryBaseRepository<PublicationAuthorModel>, IPublicationAuthorData
{
	public async Task<int> DeletePublicationAuthorByIdAsync(long id)
	{
		var deletedObject = _repository.Where(x => x.Id == id);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeletePublicationAuthorsByInstitutionIdAsync(long institutionId)
	{
		var deletedObject = _repository.Where(x => x.InstitutionId == institutionId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeletePublicationAuthorsByPersonIdAndInstitutionIdAsync(long personId, long institutionId)
	{
		var deletedObject = _repository.Where(x => x.PersonId == personId && x.InstitutionId == institutionId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<int> DeletePublicationAuthorsByPersonIdAsync(long personId)
	{
		var deletedObject = _repository.Where(x => x.PersonId == personId);
		return await DeleteRecordsAsync(deletedObject);
	}

	public async Task<PublicationAuthorModel?> GetPublicationAuthorByIdAsync(long id)
	{
		return _repository.FirstOrDefault(x=> x.Id == id);
	}

	public async Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByInstitutionIdAsync(long institutionId)
	{
		return _repository.Where(x => x.InstitutionId == institutionId);
	}

	public async Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPersonIdAsync(long personId)
	{
		return _repository.Where(x => x.PersonId == personId);
	}

	public async Task<IEnumerable<PublicationAuthorModel>> GetPublicationAuthorByPublicationIdAsync(long publicationId)
	{
		return _repository.Where(x => x.PublicationId == publicationId);
	}

	public async Task<long> InsertPublicationAuthorAsync(PublicationAuthorModel model)
	{
		return await InsertRecordAsync(model);
	}

	public async Task<int> UpdatePublicationAuthorAsync(PublicationAuthorModel model)
	{
		return await UpdateRecordAsync(model);
	}
}
