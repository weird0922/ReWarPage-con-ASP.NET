namespace ReWearWeb.Models;

public enum ResultErrorType
{
    Validation,
    NotFound,
    Conflict
}

public class Result
{
    public bool Success { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();
    public ResultErrorType ErrorType { get; set; } = ResultErrorType.Validation;

    public static Result Ok() => new() { Success = true };
    public static Result Fail(string error, ResultErrorType errorType = ResultErrorType.Validation) =>
        new() { Success = false, Errors = new[] { error }, ErrorType = errorType };
    public static Result Fail(string[] errors, ResultErrorType errorType = ResultErrorType.Validation) =>
        new() { Success = false, Errors = errors, ErrorType = errorType };
}

public class Result<T> : Result
{
    public T? Data { get; set; }

    public static Result<T> Ok(T data) => new() { Success = true, Data = data };
    public new static Result<T> Fail(string error, ResultErrorType errorType = ResultErrorType.Validation) =>
        new() { Success = false, Errors = new[] { error }, ErrorType = errorType };
    public new static Result<T> Fail(string[] errors, ResultErrorType errorType = ResultErrorType.Validation) =>
        new() { Success = false, Errors = errors, ErrorType = errorType };
}
