﻿using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivitiesCommandResponse : ResponseBase
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public IEnumerable<PublicationActivity> PublicationActivities { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}