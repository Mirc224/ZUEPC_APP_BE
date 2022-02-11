﻿using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Commands.Auth;

public class RefreshTokenCommandResponse : ResponseBase
{
	public string Token { get; set; }
	public string RefreshToken { get; set; }
}
