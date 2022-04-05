namespace ZUEPC.Import.Models;

public class ImportRecord
{
	public DateTime RecordVersionDate { get; set; }
	public ImportPublication? Publication { get; set; }
	public string? RecordVersionDateString
	{
		set
		{
			if (value is null)
			{
				return;
			}
			if (!DateTime.TryParse(value, out DateTime resultDate))
			{
				return;
			}
			RecordVersionDate = resultDate;
		}
	}
}