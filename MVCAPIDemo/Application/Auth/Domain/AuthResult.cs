﻿using ZUEPC.Common.Responses;

namespace ZUEPC.Auth.Domain;

public class AuthResult: ResponseBase
{
	public string? Token { get; set; }
	public Guid RefreshToken { get; set; }
}
