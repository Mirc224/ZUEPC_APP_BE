namespace ZUEPC.Import.Models;

public class ImportPublicationAuthor
{
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
	public ImportPerson? Person { get; set; }
	public ImportInstitution? ReportingInstitution { get; set; }

	public string? ContributionRatioString
	{
		set
		{
			ContributionRatio = default;
			string currentDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			string? convertedVal = value?.Replace(",", currentDecimalSeparator).Replace(".", currentDecimalSeparator);
			if (double.TryParse(convertedVal, out double tmpNumber))
			{
				ContributionRatio = tmpNumber;
			}
		}
	}
}
