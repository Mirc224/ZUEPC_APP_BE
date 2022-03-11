using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public class RefreshTokenInMemoryData : InMemoryBaseRepository<RefreshTokenModel>, IRefreshTokenData
{
	public async Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken)
	{
		return _repository.Find(x => x.Token == refreshToken);
	}

	public async Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId)
	{
		return _repository.Find(x => x.JwtId == jwtId);
	}

	public async Task<RefreshTokenModel?> GetUserRefreshTokenByUserIdAndJwtIdAsync(long userId, string jwtId)
	{
		return _repository.Find(x => x.UserId == userId && x.JwtId == jwtId);
	}
}
