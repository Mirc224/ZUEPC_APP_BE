using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Base.Responses;
using ZUEPC.Users.Domain;

namespace ZUEPC.Api.Application.Auth.Queries.RefreshTokens;

public class GetRefreshTokenByJwtIdQueryResponse : ResponseWithDataBase<RefreshToken>
{
}