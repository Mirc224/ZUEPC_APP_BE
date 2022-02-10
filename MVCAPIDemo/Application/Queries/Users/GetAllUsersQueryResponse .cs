﻿using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllUsersQueryResponse : CQRSBaseResponse
{
	public IEnumerable<User> Users { get; set; }
}
