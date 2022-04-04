using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public interface IRefreshTokenData
{
	Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken);
	Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId);
	Task<long> InsertModelAsync(RefreshTokenModel model);
	Task<int> UpdateModelAsync(RefreshTokenModel model);
}
