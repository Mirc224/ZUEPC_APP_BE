using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Institution;

namespace ZUEPC.DataAccess.Data.Institutions;

public interface IInstitutionData : 
	IRepositoryBase<InstitutionModel>
{
	Task<IEnumerable<InstitutionModel>> GetAllAsync();
}
