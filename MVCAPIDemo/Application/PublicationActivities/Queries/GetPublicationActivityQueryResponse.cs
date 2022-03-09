﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivityQueryResponse : ResponseBase
{
	public PublicationActivity? PublicationActivity { get; set; }
}