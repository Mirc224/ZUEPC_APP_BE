namespace ZUEPC.EvidencePublication.Base.Commands;

public abstract class DeleteModelCommandBase<TId>
{
	public TId Id { get; set; }
}
