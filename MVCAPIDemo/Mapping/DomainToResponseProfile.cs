using AutoMapper;
using Users.Base.Application.Domain;
using Users.Base.Domain;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Auth.Domain;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.DataAccess.Models.Institution;
using ZUEPC.DataAccess.Models.Person;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.DataAccess.Models.PublicationAuthor;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPublication;

namespace ZUEPC.Application.Mapping;

public class DomainToResponseProfile: Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<UserModel, User>()
			.ReverseMap();
        CreateMap<RegisterUserCommand, UserModel>();
		CreateMap<RoleModel, Role>();
		CreateMap<AuthResult, LoginUserCommandResponse>();
		CreateMap<AuthResult, RefreshTokenCommandResponse>();
		CreateMap<RevokeResult, RevokeTokenCommandResponse>();
		CreateMap<RevokeResult, LogoutUserCommandResponse>();
		CreateMap<ImportPublication, PublicationModel>();
		CreateMap<ImportPublicationNameDetails, PublicationNameModel>();

		CreateMap<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Publications
		CreateMap<PublicationModel, Publication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		CreateMap<PublicationNameModel, PublicationName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		CreateMap<PublicationIdentifierModel, PublicationIdentifier>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		CreateMap<PublicationExternDatabaseIdModel, PublicationExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Institutions
		CreateMap<InstitutionModel, Institution>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		CreateMap<InstitutionExternDatabaseIdModel, InstitutionExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		CreateMap<InstitutionNameModel, InstitutionName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Persons
		CreateMap<PersonModel, Person>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		CreateMap<PersonNameModel, PersonName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		CreateMap<PersonExternDatabaseIdModel, PersonExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Publication activities
		CreateMap<PublicationActivityModel, PublicationActivity>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Publication authors
		CreateMap<PublicationAuthorModel, PublicationAuthor>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Related publications
		CreateMap<RelatedPublicationModel, RelatedPublication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();


		// ImportPublication
		CreateMap<ImportPublication, CreatePublicationCommand>();

		// Base command to model mapping
		CreateMap<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<EPCUpdateBaseCommand, EPCBaseModel>();
		
		// Base doamin to command mapping
		CreateMap<EPCBase, EPCUpdateBaseCommand>();

		CreateMap<CreatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		
		CreateMap<UpdatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		CreateMap<ImportPublication, UpdatePublicationCommand>();

		CreateMap<Publication, UpdatePublicationCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		// Create publication identifier
		CreateMap<ImportPublicationIdentifier, CreatePublicationIdentifierCommand>();
		CreateMap<CreatePublicationIdentifierCommand, PublicationIdentifierModel>();

		// Update publication identifier
		CreateMap<PublicationIdentifier, UpdatePublicationIdentifierCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();

		CreateMap<UpdatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		CreateMap<ImportPublicationIdentifier, UpdatePublicationIdentifierCommand>();
	}
}
