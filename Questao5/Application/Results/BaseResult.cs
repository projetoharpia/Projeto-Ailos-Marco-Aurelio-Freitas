namespace Questao5.Application.Results;

public class BaseResult<T>
{
    public bool IsSuccess { get; private set; }
    public string? Message { get; private set; }
    public string? ErrorCode { get; private set; }
    public T? Data { get; private set; }

    public static BaseResult<T> Success(T data)
    {
        return new BaseResult<T> { IsSuccess = true, Data = data };
    }

    public static BaseResult<T> Failure(string message, string errorCode)
    {
        return new BaseResult<T> { IsSuccess = false, Message = message, ErrorCode = errorCode };
    }
}