using AutoMapper;
using Users.Base.Application.Domain;
using Users.Base.Domain;
using ZUEPC.Application.Auth.Commands;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
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
		CreateBaseClassesMapping();
		CreateUserMapping();
		CreateImportToDomainMapping();

		CreateModelToDomainMapping();
		CreateImportToCommandMapping();

		CreateDomainToCommandMapping();
		CreateCommandToModelMapping();
	}

	private void CreateUserMapping()
	{
		CreateMap<UserModel, User>()
			.ReverseMap();
		CreateMap<RegisterUserCommand, UserModel>();
		CreateMap<RoleModel, Role>();
		CreateMap<AuthResult, LoginUserCommandResponse>();
		CreateMap<AuthResult, RefreshTokenCommandResponse>();
		CreateMap<RevokeResult, RevokeTokenCommandResponse>();
		CreateMap<RevokeResult, LogoutUserCommandResponse>();
	}

	private void CreateImportToDomainMapping()
	{
		CreateMap<ImportPublicationIdentifier, PublicationIdentifier>();
		CreateMap<ImportPublicationExternDatabaseId, PublicationExternDatabaseId>();
		CreateMap<ImportPublicationName, PublicationName>();
	}

	private void CreateBaseClassesMapping()
	{
		CreateMap<EPCBaseModel, EPCBase>()
			.ReverseMap();

		CreateMap<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.ReverseMap();

		// Base command to model mapping
		CreateMap<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<EPCUpdateBaseCommand, EPCBaseModel>();
	}

	private void CreateModelToDomainMapping()
	{
		// Publications
		CreateMap<PublicationModel, Publication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PublicationIdentifiers
		CreateMap<PublicationIdentifierModel, PublicationIdentifier>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PublicationExternDatabaseIdentifiers
		CreateMap<PublicationExternDatabaseIdModel, PublicationExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PublicationName
		CreateMap<PublicationNameModel, PublicationName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Institutions
		CreateMap<InstitutionModel, Institution>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// InstitutionExternDatabaseIdentifiers
		CreateMap<InstitutionExternDatabaseIdModel, InstitutionExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// InstitutionNames
		CreateMap<InstitutionNameModel, InstitutionName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// Persons
		CreateMap<PersonModel, Person>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PersonIdentifiers
		CreateMap<PersonExternDatabaseIdModel, PersonExternDatabaseId>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
		// PersonNames
		CreateMap<PersonNameModel, PersonName>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// PublicationActivities
		CreateMap<PublicationActivityModel, PublicationActivity>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// PublicationAuthors
		CreateMap<PublicationAuthorModel, PublicationAuthor>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();

		// RelatedPublications
		CreateMap<RelatedPublicationModel, RelatedPublication>()
			.IncludeBase<EPCBaseModel, EPCBase>()
			.ReverseMap();
	}

	private void CreateImportToCommandMapping()
	{
		// Base doamin to command mapping
		CreateMap<EPCBase, EPCUpdateBaseCommand>();
		// Base doamin to command mapping
		CreateMap<EPCBase, EPCUpdateBaseCommand>();


		// ImportPublication
		CreateMap<ImportPublication, CreatePublicationCommand>();
		CreateMap<ImportPublication, UpdatePublicationCommand>();

		// ImportPublicationIdentifier
		CreateMap<ImportPublicationIdentifier, CreatePublicationIdentifierCommand>();
		CreateMap<ImportPublicationIdentifier, UpdatePublicationIdentifierCommand>();

		// ImportPublicationExternDatabaseId
		CreateMap<ImportPublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>();
		CreateMap<ImportPublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>();

		// ImportPerson
		CreateMap<ImportPerson, CreatePersonCommand>();

	}

	private void CreateDomainToCommandMapping()
	{
		// Publication
		CreateMap<Publication, UpdatePublicationCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();
		// ImportPublicationIdentifier
		CreateMap<PublicationIdentifier, UpdatePublicationIdentifierCommand>()
			.IncludeBase<EPCBase, EPCUpdateBaseCommand>();
	}

	private void CreateCommandToModelMapping()
	{
		// Publication 
		CreateMap<CreatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PublicationIdentifier
		CreateMap<CreatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PublicationExternIdentifier
		CreateMap<CreatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// PublicationName
		CreateMap<CreatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
		CreateMap<UpdatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCUpdateBaseCommand, EPCBaseModel>();

		// Person
		CreateMap<CreatePersonCommand, PersonModel>()
			.IncludeBase<EPCCreateBaseCommand, EPCBaseModel>();
	}
}

