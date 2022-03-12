using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Responses;
using ZUEPC.Users.Base.Domain;

namespace ZUEPC.Api.Application.Auth.Queries.RefreshTokens;

public class GetRefreshTokenByJwtIdQueryResponse : ResponseWithDataBase<RefreshToken>
{
}