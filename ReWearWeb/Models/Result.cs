namespace ReWearWeb.Models;

public class Result
{
    public bool Success { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();

    public static Result Ok() => new() { Success = true };
    public static Result Fail(string error) => new() { Success = false, Errors = new[] { error } };
    public static Result Fail(string[] errors) => new() { Success = false, Errors = errors };
}

public class Result<T> : Result
{
    public T? Data { get; set; }

    public static Result<T> Ok(T data) => new() { Success = true, Data = data };
    public new static Result<T> Fail(string error) => new() { Success = false, Errors = new[] { error } };
    public new static Result<T> Fail(string[] errors) => new() { Success = false, Errors = errors };
}
