namespace ZUEPC.DataAccess.Interfaces;

public interface IItemWithID<TId>
{
	public TId Id { get; set; }
}
