namespace ZUEPC.Base.ItemInterfaces;

public interface IItemWithID<TId>
{
	public TId Id { get; set; }
}
