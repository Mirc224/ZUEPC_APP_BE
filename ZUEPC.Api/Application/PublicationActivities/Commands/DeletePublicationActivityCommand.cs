﻿using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class DeletePublicationActivityCommand : 
	EPCDeleteModelCommandBase<long>,
	IRequest<DeletePublicationActivityCommandResponse>
{
}
