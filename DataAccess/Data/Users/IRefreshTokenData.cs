using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public interface IRefreshTokenData : IRepositoryBase<RefreshTokenModel>
{
	Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken);
	Task<RefreshTokenModel?> GetUserRefreshTokenByUserIdAndJwtIdAsync(long userId, string jwtId);
	Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId);
}
