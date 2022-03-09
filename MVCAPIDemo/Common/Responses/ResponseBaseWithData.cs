namespace ZUEPC.Common.Responses;

public class ResponseBaseWithData<T> : ResponseBase
{
	public T? Data { get; set; }
}
