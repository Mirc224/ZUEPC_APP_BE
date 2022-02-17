namespace KIS.Import.Models;
public class Person
{
	public int Id { get; set; }
	public int? CREPCId { get; set; }
	public int? KISId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? MiddleName { get; set; }
	public string? Number { get; set; }
	public DateTime? BirthDate { get; set; }
	public DateTime? DeathDate { get; set; }
}
