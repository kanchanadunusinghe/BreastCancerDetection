namespace BCD.Application.DTOs.Common;

public class ResultDto<T>
{
    public bool Success { get; private set; }

    public string Message { get; private set; } = string.Empty;

    public T? Data { get; private set; }

    public List<string> Errors { get; private set; } = new();

    private ResultDto() { }

    public static ResultDto<T> Ok(T data, string message = "Success")
    {
        return new ResultDto<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ResultDto<T> Failure(string message, List<string>? errors = null)
    {
        return new ResultDto<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

