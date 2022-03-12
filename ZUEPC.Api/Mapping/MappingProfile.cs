using AutoMapper;
using Users.Base.Application.Domain;
using Users.Base.Domain;
using ZUEPC.Api.Application.Auth.Commands.RefreshTokens;
using ZUEPC.Api.Application.Users.Commands.UserRoles;
using ZUEPC.Api.Application.Users.Commands.Users;
using ZUEPC.Api.Application.Users.Entities.Details;
using ZUEPC.Application.Auth.Commands.RefreshTokens;
using ZUEPC.Application.Auth.Commands.Users;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Entities.Inputs.PersonNames;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.PublicationActivities.Commands;
using ZUEPC.Application.PublicationActivities.Entities.Inputs.PublicationActivities;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.RelatedPublications.Commands;
using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.Application.RelatedPublications.Entities.Inputs.RelatedPublications;
using ZUEPC.Auth.Domain;
using ZUEPC.Common.Entities;
using ZUEPC.Common.Entities.Inputs;
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
using ZUEPC.Import.Models.Commond;
using ZUEPC.Users.Base.Domain;
using static ZUEPC.Import.Models.ImportInstitution;
using static ZUEPC.Import.Models.ImportPerson;
using static ZUEPC.Import.Models.ImportPublication;

namespace ZUEPC.Application.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateBaseClassesMapping();
		CreateUserMapping();
		CreateImportToDomainMapping();

		CreateModelToDomainMapping();
		CreateImportToCommandMapping();

		CreateDomainToCommandMapping();
		CreateCommandToModelMapping();

		CreateDomainToDetailMapping();
		CreateDomainToPreviewMapping();

		CreateCommandToCommandMapping();
		
		CreateInputDtoToCommandMapping();
	}

	private void CreateInputDtoToCommandMapping()
	{
		CreateMap<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType));

		CreateMap<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType));

		// Publication
		CreateMap<PublicationNameCreateDto, CreatePublicationNameCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name));

		CreateMap<PublicationNameUpdateDto, UpdatePublicationNameCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name));

		CreateMap<PublicationIdentifierCreateDto, CreatePublicationIdentifierCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue));

		CreateMap<PublicationIdentifierUpdateDto, UpdatePublicationIdentifierCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue));

		CreateMap<PublicationExternDatabaseIdCreateDto, CreatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		CreateMap<PublicationExternDatabaseIdUpdateDto, UpdatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// PublicationAuthor
		CreateMap<PublicationAuthorCreateDto, CreatePublicationAuthorCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		CreateMap<PublicationAuthorUpdateDto, UpdatePublicationAuthorCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));
		
		// RelatedPublication
		CreateMap<RelatedPublicationCreateDto, CreateRelatedPublicationCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));

		CreateMap<RelatedPublicationUpdateDto, UpdateRelatedPublicationCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));

		// PublicationActivity
		CreateMap<PublicationActivityCreateDto, CreatePublicationActivityCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));

		CreateMap<PublicationActivityUpdateDto, UpdatePublicationActivityCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));

		// Person
		CreateMap<PersonNameCreateDto, CreatePersonNameCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		CreateMap<PersonNameUpdateDto, UpdatePersonNameCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		CreateMap<PersonExternDatabaseIdCreateDto, CreatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		
		CreateMap<PersonExternDatabaseIdUpdateDto, UpdatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// Institution
		CreateMap<InstitutionNameCreateDto, CreateInstitutionNameCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name));

		CreateMap<InstitutionNameUpdateDto, UpdateInstitutionNameCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name));

		CreateMap<InstitutionExternDatabaseIdCreateDto, CreateInstitutionExternDatabaseIdCommand>()
			.IncludeBase<EPCBaseDto, EPCCreateCommandBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		CreateMap<InstitutionExternDatabaseIdUpdateDto, UpdateInstitutionExternDatabaseIdCommand>()
			.IncludeBase<EPCBaseDto, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
	}

	private void CreateCommandToCommandMapping()
	{
		// User
		CreateMap<RegisterUserCommand, CreateUserCommand>()
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
			.ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.Password));

		// Publication
		CreateMap<CreatePublicationWithDetailsCommand, CreatePublicationCommand>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));

		CreateMap<UpdatePublicationWithDetailsCommand, UpdatePublicationCommand>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));

		// Person
		CreateMap<CreatePersonWithDetailsCommand, CreatePersonCommand>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));

		CreateMap<UpdatePersonWithDetailsCommand, UpdatePersonCommand>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));

		// Institution
		CreateMap<CreateInstitutionWithDetailsCommand, CreateInstitutionCommand>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstititutionType, opts => opts.MapFrom(src => src.InstitutionType));

		CreateMap<UpdateInstitutionWithDetailsCommand, UpdateInstitutionCommand>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstititutionType, opts => opts.MapFrom(src => src.InstitutionType));

	}

	private void CreateDomainToPreviewMapping()
	{
		CreateMap<Publication, PublicationPreview>()
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id));
		
		CreateMap<Person, PersonPreview>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));

		CreateMap<Institution, InstitutionPreview>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstitutionType));
	}

	private void CreateDomainToDetailMapping()
	{
		CreateMap<User, UserDetails>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => src.CreatedAt))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email));

		CreateMap<EPCDomainBase, DetailsBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => src.CreatedAt))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType))
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate));

		CreateMap<Publication, PublicationDetails>()
			.IncludeBase<EPCDomainBase, DetailsBase>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));

		CreateMap<Institution, InstitutionDetails>()
			.IncludeBase<EPCDomainBase, DetailsBase>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstitutionType));

		CreateMap<PublicationAuthor, PublicationAuthorDetails>()
			.IncludeBase<EPCDomainBase, DetailsBase>()
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		CreateMap<RelatedPublication, RelatedPublicationDetails>()
			.IncludeBase<EPCDomainBase, DetailsBase>()
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));

		CreateMap<Person, PersonDetails>()
			.IncludeBase<EPCDomainBase, DetailsBase>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));
	}

	private void CreateUserMapping()
	{
		CreateMap<RegisterUserCommand, UserModel>();
		CreateMap<RoleModel, Role>();
		CreateMap<AuthResult, LoginUserCommandResponse>();
		CreateMap<AuthResult, RefreshTokenCommandResponse>();
		CreateMap<RevokeResult, RevokeRefreshTokenCommandResponse>();
		CreateMap<RevokeResult, LogoutUserCommandResponse>();
	}

	private void CreateImportToDomainMapping()
	{
		// Publication
		CreateMap<ImportPublication, Publication>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));
		CreateMap<ImportPublicationIdentifier, PublicationIdentifier>()
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm));
		CreateMap<ImportPublicationExternDatabaseId, PublicationExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportPublicationName, PublicationName>();

		// Person
		CreateMap<ImportPerson, Person>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));
		CreateMap<ImportPersonExternDatabaseId, PersonExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportPersonName, PersonName>()
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName));

		// Institution
		CreateMap<ImportInstitution, Institution>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstititutionType));
		CreateMap<ImportInstitutionExternDatabaseId, InstitutionExternDatabaseId>()
			.IncludeBase<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>();
		CreateMap<ImportInstitutionName, InstitutionName>()
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// PublicationAuthor
		CreateMap<ImportPublicationAuthor, PublicationAuthor>()
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		// RelatedPublication
		CreateMap<ImportRelatedPublication, RelatedPublication>()
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));

		// PublicationActivity
		CreateMap<ImportPublicationActivity, PublicationActivity>()
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
	}

	private void CreateBaseClassesMapping()
	{
		CreateMap<ModelBase, DomainBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => src.CreatedAt))
			.ReverseMap();

		CreateMap<EPCModelBase, EPCDomainBase>()
			.IncludeBase<ModelBase, DomainBase>()
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType))
			.ReverseMap();

		CreateMap<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ReverseMap();

		CreateMap<EPCImportExternDatabaseIdBase, EPCExternDatabaseIdBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ReverseMap();

		// Base command to model mapping
		CreateMap<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType))
			.ReverseMap();
		CreateMap<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType))
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ReverseMap();
	}

	private void CreateModelToDomainMapping()
	{
		CreateMap<RefreshTokenModel, RefreshToken>()
			.IncludeBase<ModelBase, DomainBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
			.ForMember(dest => dest.JwtId, opts => opts.MapFrom(src => src.JwtId))
			.ForMember(dest => dest.Token, opts => opts.MapFrom(src => src.Token))
			.ForMember(dest => dest.IsRevoked, opts => opts.MapFrom(src => src.IsRevoked))
			.ForMember(dest => dest.IsUsed, opts => opts.MapFrom(src => src.IsUsed))
			.ForMember(dest => dest.ExpiryDate, opts => opts.MapFrom(src => src.ExpiryDate))
			.ForMember(dest => dest.CreatedAt, opts => opts.MapFrom(src => src.CreatedAt));

		// User
		CreateMap<UserModel, User>()
			.IncludeBase<ModelBase, DomainBase>()
			.ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ReverseMap();

		// UserRole
		CreateMap<UserRoleModel, UserRole>()
			.IncludeBase<ModelBase, DomainBase>()
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
			.ForMember(dest => dest.RoleId, opts => opts.MapFrom(src => src.RoleId))
			.ReverseMap();


		// Publications
		CreateMap<PublicationModel, Publication>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ReverseMap();
		// PublicationIdentifiers
		CreateMap<PublicationIdentifierModel, PublicationIdentifier>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ReverseMap();
		// PublicationExternDatabaseIdentifiers
		CreateMap<PublicationExternDatabaseIdModel, PublicationExternDatabaseId>()
			 .IncludeBase<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			 .ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			 .ReverseMap();
		// PublicationName
		CreateMap<PublicationNameModel, PublicationName>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ReverseMap();

		// Institutions
		CreateMap<InstitutionModel, Institution>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstitutionType))
			.ReverseMap();
		// InstitutionExternDatabaseIdentifiers
		CreateMap<InstitutionExternDatabaseIdModel, InstitutionExternDatabaseId>()
			.IncludeBase<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ReverseMap();
		// InstitutionNames
		CreateMap<InstitutionNameModel, InstitutionName>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ReverseMap();

		// Persons
		CreateMap<PersonModel, Person>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear))
			.ReverseMap();
		// PersonIdentifiers
		CreateMap<PersonExternDatabaseIdModel, PersonExternDatabaseId>()
			.IncludeBase<EPCExternDatabaseIdBaseModel, EPCExternDatabaseIdBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ReverseMap();
		// PersonNames
		CreateMap<PersonNameModel, PersonName>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ReverseMap();

		// PublicationActivities
		CreateMap<PublicationActivityModel, PublicationActivity>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ReverseMap();

		// PublicationAuthors
		CreateMap<PublicationAuthorModel, PublicationAuthor>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role))
			.ReverseMap();

		// RelatedPublications
		CreateMap<RelatedPublicationModel, RelatedPublication>()
			.IncludeBase<EPCModelBase, EPCDomainBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory))
			.ReverseMap();
	}

	private void CreateImportToCommandMapping()
	{
		// Base doamin to command mapping
		CreateMap<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType));
		// Base doamin to command mapping
		CreateMap<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.VersionDate, opts => opts.MapFrom(src => src.VersionDate))
			.ForMember(dest => dest.OriginSourceType, opts => opts.MapFrom(src => src.OriginSourceType));

		

		// ImportPublication
		CreateMap<ImportPublication, CreatePublicationCommand>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));

		// ImportPerson
		CreateMap<ImportPerson, CreatePersonCommand>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));

		// ImportInstitution
		CreateMap<ImportInstitution, CreateInstitutionCommand>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstititutionType, opts => opts.MapFrom(src => src.InstititutionType));
	}

	private void CreateDomainToCommandMapping()
	{
		// RefreshToken
		CreateMap<RefreshToken, UpdateRefreshTokenCommand>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
			.ForMember(dest => dest.JwtId, opts => opts.MapFrom(src => src.JwtId))
			.ForMember(dest => dest.Token, opts => opts.MapFrom(src => src.Token))
			.ForMember(dest => dest.IsRevoked, opts => opts.MapFrom(src => src.IsRevoked))
			.ForMember(dest => dest.IsUsed, opts => opts.MapFrom(src => src.IsUsed))
			.ForMember(dest => dest.ExpiryDate, opts => opts.MapFrom(src => src.ExpiryDate));

		// RefreshToken
		CreateMap<RefreshToken, CreateRefreshTokenCommand>()
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
			.ForMember(dest => dest.JwtId, opts => opts.MapFrom(src => src.JwtId))
			.ForMember(dest => dest.Token, opts => opts.MapFrom(src => src.Token))
			.ForMember(dest => dest.IsRevoked, opts => opts.MapFrom(src => src.IsRevoked))
			.ForMember(dest => dest.IsUsed, opts => opts.MapFrom(src => src.IsUsed))
			.ForMember(dest => dest.ExpiryDate, opts => opts.MapFrom(src => src.ExpiryDate));

		// Publication
		CreateMap<Publication, UpdatePublicationCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType))
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear));

		// PublicationIdentifier
		CreateMap<PublicationIdentifier, CreatePublicationIdentifierCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm));
		CreateMap<PublicationIdentifier, UpdatePublicationIdentifierCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm));

		// PublicationExternDatabaseId
		CreateMap<PublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));
		CreateMap<PublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));

		// PublicationName
		CreateMap<PublicationName, CreatePublicationNameCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));
		CreateMap<PublicationName, UpdatePublicationNameCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId));

		// Person
		CreateMap<Person, UpdatePersonCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));
			
		// PersonExternDatabaseId
		CreateMap<PersonExternDatabaseId, CreatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId));
		CreateMap<PersonExternDatabaseId, UpdatePersonExternDatabaseIdCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId));

		// PersonName
		CreateMap<PersonName, CreatePersonNameCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName));
		CreateMap<PersonName, UpdatePersonNameCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName));

		// Institution
		CreateMap<Institution, UpdateInstitutionCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstititutionType, opts => opts.MapFrom(src => src.InstitutionType));

		//  InstitutionExternDatabaseId
		CreateMap<InstitutionExternDatabaseId, CreateInstitutionExternDatabaseIdCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<InstitutionExternDatabaseId, UpdateInstitutionExternDatabaseIdCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// InstitutionName
		CreateMap<InstitutionName, CreateInstitutionNameCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<InstitutionName, UpdateInstitutionNameCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// PublicationAuthor
		CreateMap<PublicationAuthor, CreatePublicationAuthorCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));
		CreateMap<PublicationAuthor, UpdatePublicationAuthorCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		// RelatedPublication
		CreateMap<RelatedPublication, CreateRelatedPublicationCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType));
		CreateMap<RelatedPublication, UpdateRelatedPublicationCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType));

		// PublicationActivity
		CreateMap<PublicationActivity, CreatePublicationActivityCommand>()
			.IncludeBase<EPCDomainBase, EPCCreateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
		CreateMap<PublicationActivity, UpdatePublicationActivityCommand>()
			.IncludeBase<EPCDomainBase, EPCUpdateCommandBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
	}

	private void CreateCommandToModelMapping()
	{
		// RefreshToken
		CreateMap<UpdateRefreshTokenCommand, RefreshTokenModel>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
			.ForMember(dest => dest.JwtId, opts => opts.MapFrom(src => src.JwtId))
			.ForMember(dest => dest.Token, opts => opts.MapFrom(src => src.Token))
			.ForMember(dest => dest.IsRevoked, opts => opts.MapFrom(src => src.IsRevoked))
			.ForMember(dest => dest.IsUsed, opts => opts.MapFrom(src => src.IsUsed))
			.ForMember(dest => dest.ExpiryDate, opts => opts.MapFrom(src => src.ExpiryDate));

		CreateMap<CreateRefreshTokenCommand, RefreshTokenModel>()
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
			.ForMember(dest => dest.JwtId, opts => opts.MapFrom(src => src.JwtId))
			.ForMember(dest => dest.Token, opts => opts.MapFrom(src => src.Token))
			.ForMember(dest => dest.IsRevoked, opts => opts.MapFrom(src => src.IsRevoked))
			.ForMember(dest => dest.IsUsed, opts => opts.MapFrom(src => src.IsUsed))
			.ForMember(dest => dest.ExpiryDate, opts => opts.MapFrom(src => src.ExpiryDate));

		// User
		CreateMap<CreateUserCommand, UserModel>()
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email));

		CreateMap<UpdateUserCommand, UserModel>()
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ReverseMap();

		// UserRole
		CreateMap<CreateUserRoleCommand, UserRoleModel>()
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.UserId))
			.ForMember(dest => dest.RoleId, opts => opts.MapFrom(src => src.RoleType));

		// Publication 
		CreateMap<CreatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType));
		CreateMap<UpdatePublicationCommand, PublicationModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublishYear, opts => opts.MapFrom(src => src.PublishYear))
			.ForMember(dest => dest.DocumentType, opts => opts.MapFrom(src => src.DocumentType));
		// PublicationIdentifier
		CreateMap<CreatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName));
		CreateMap<UpdatePublicationIdentifierCommand, PublicationIdentifierModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.IdentifierValue, opts => opts.MapFrom(src => src.IdentifierValue))
			.ForMember(dest => dest.ISForm, opts => opts.MapFrom(src => src.ISForm))
			.ForMember(dest => dest.IdentifierName, opts => opts.MapFrom(src => src.IdentifierName));

		// PublicationExternIdentifier
		CreateMap<CreatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<UpdatePublicationExternDatabaseIdCommand, PublicationExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// PublicationName
		CreateMap<CreatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<UpdatePublicationNameCommand, PublicationNameModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// Person
		CreateMap<CreatePersonCommand, PersonModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));
		CreateMap<UpdatePersonCommand, PersonModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.BirthYear, opts => opts.MapFrom(src => src.BirthYear))
			.ForMember(dest => dest.DeathYear, opts => opts.MapFrom(src => src.DeathYear));

		// PersonExternDatabaseId
		CreateMap<CreatePersonExternDatabaseIdCommand, PersonExternDatabaseIdModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<UpdatePersonExternDatabaseIdCommand, PersonExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// PersonName
		CreateMap<CreatePersonNameCommand, PersonNameModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<UpdatePersonNameCommand, PersonNameModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// Institution
		CreateMap<CreateInstitutionCommand, InstitutionModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstititutionType));
		CreateMap<UpdateInstitutionCommand, InstitutionModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level))
			.ForMember(dest => dest.InstitutionType, opts => opts.MapFrom(src => src.InstititutionType));

		// InstitutionExternDatabaseId
		CreateMap<CreateInstitutionExternDatabaseIdCommand, InstitutionExternDatabaseIdModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));
		CreateMap<UpdateInstitutionExternDatabaseIdCommand, InstitutionExternDatabaseIdModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.ExternIdentifierValue, opts => opts.MapFrom(src => src.ExternIdentifierValue));

		// InstitutionName
		CreateMap<CreateInstitutionNameCommand, InstitutionNameModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));
		CreateMap<UpdateInstitutionNameCommand, InstitutionNameModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
			.ForMember(dest => dest.NameType, opts => opts.MapFrom(src => src.NameType));

		// PublicationAuthor
		CreateMap<CreatePublicationAuthorCommand, PublicationAuthorModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));
		CreateMap<UpdatePublicationAuthorCommand, PublicationAuthorModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.InstitutionId, opts => opts.MapFrom(src => src.InstitutionId))
			.ForMember(dest => dest.PersonId, opts => opts.MapFrom(src => src.PersonId))
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.ContributionRatio, opts => opts.MapFrom(src => src.ContributionRatio))
			.ForMember(dest => dest.Role, opts => opts.MapFrom(src => src.Role));

		// RelatedPublication
		CreateMap<CreateRelatedPublicationCommand, RelatedPublicationModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));
		CreateMap<UpdateRelatedPublicationCommand, RelatedPublicationModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.RelatedPublicationId, opts => opts.MapFrom(src => src.RelatedPublicationId))
			.ForMember(dest => dest.RelationType, opts => opts.MapFrom(src => src.RelationType))
			.ForMember(dest => dest.CitationCategory, opts => opts.MapFrom(src => src.CitationCategory));

		// PublicationActivity
		CreateMap<CreatePublicationActivityCommand, PublicationActivityModel>()
			.IncludeBase<EPCCreateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
		CreateMap<UpdatePublicationActivityCommand, PublicationActivityModel>()
			.IncludeBase<EPCUpdateCommandBase, EPCModelBase>()
			.ForMember(dest => dest.PublicationId, opts => opts.MapFrom(src => src.PublicationId))
			.ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
			.ForMember(dest => dest.ActivityYear, opts => opts.MapFrom(src => src.ActivityYear))
			.ForMember(dest => dest.GovernmentGrant, opts => opts.MapFrom(src => src.GovernmentGrant));
	}
}

