using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Xml.Linq;
using ZUEPC.Api.Common.Extensions;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.Import.Commands.Common;
using ZUEPC.Application.Import.Validators;
using ZUEPC.Localization;

namespace ZUEPC.Application.Import.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ImportController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IStringLocalizer<DataAnnotations> _localizer;

		public ImportController(IMediator mediator, IStringLocalizer<DataAnnotations> localizer)
		{
			_mediator = mediator;
			_localizer = localizer;
		}

		[Authorize(Roles = "EDITOR,ADMIN")]
		[HttpPost("upload/{importSystem}")]
		public async Task<IActionResult> UploadFile([FromRoute] string importSystem, IFormFile file)
		{
			importSystem = importSystem.ToLower();
			if (string.IsNullOrEmpty(importSystem) || 
				(importSystem != "dawinci" && importSystem != "crepc"))
			{
				string errorMessag = string.Format(_localizer[DataAnnotationsKeyConstants.UNSUPPORTED_IMPORT_SYSTEM], "dawinci, crepc");
				return BadRequest(new { errors = new string[] { errorMessag} });
			}
			string fileContent = await file.ReadAsStringAsync();
			ImportXmlBaseCommand? command = null;
			if (importSystem == "crepc")
			{
				command = new ImportCREPCXmlCommand() { RawContent = fileContent };
			}
			if (importSystem == "dawinci")
			{
				command = new ImportDaWinciXmlCommand() { RawContent = fileContent };
			}
			IActionResult? validationResult = ValidateImportXmlCommand(command);
			if (validationResult is null)
			{
				command.XEelementBody = XElement.Parse(fileContent);
				ImportBaseResponse? response = (ImportBaseResponse)await _mediator.Send(command);
				if (response == null || !response.Success)
				{
					return StatusCode(500);
				}
				return Ok(response.PublicationsIds);
			}

			return validationResult;
		}

		private IActionResult? ValidateImportXmlCommand(ImportXmlBaseCommand command)
		{
			FluentValidation.Results.ValidationResult validationResult = new ImportXmlBaseCommandValidator(_localizer).Validate(command);
			if (!validationResult.IsValid)
			{
				foreach (FluentValidation.Results.ValidationFailure error in validationResult.Errors)
				{
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
				}
				return UnprocessableEntity(ModelState);
			}
			return null;
		}
	}
}