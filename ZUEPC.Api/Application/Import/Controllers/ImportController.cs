using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Text;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
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

		[HttpPost("crepc")]
		[Consumes("application/xml")]
		public async Task<IActionResult> ImportCREPC([FromBody] XElement input)
		{
			ImportCREPCXmlCommand request = new() { XEelementBody = input };

			ImportCREPCXmlCommandResponse? response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.PublicationsIds);
		}

		[HttpPost("dawinci")]
		[Consumes("application/xml")]
		public async Task<IActionResult> ImportDaWinci([FromBody] XElement input)
		{
			ImportDaWinciXmlCommand request = new() { XEelementBody = input};

			ImportDaWinciXmlCommandResponse response = await _mediator.Send(request);
			if (!response.Success)
				return StatusCode(500);
			return Ok(response.PublicationsIds);
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