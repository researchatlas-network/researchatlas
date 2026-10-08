namespace ResearchAtlas.Application.Abstractions.Core;

public sealed class HandlerResult
{
    private HandlerResult(bool isSuccess, IReadOnlyList<HandlerResultError> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<HandlerResultError> Errors { get; }

    public static HandlerResult Success() => new(true, Array.Empty<HandlerResultError>());

    public static HandlerResult Failure(params HandlerResultError[] errors) => Failure((IEnumerable<HandlerResultError>)errors);

    public static HandlerResult Failure(IEnumerable<HandlerResultError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToArray();
        if (errorList.Length == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        return new HandlerResult(false, errorList);
    }
}

public sealed class HandlerResult<T>
{
    private HandlerResult(bool isSuccess, T? value, IReadOnlyList<HandlerResultError> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public IReadOnlyList<HandlerResultError> Errors { get; }

    public static HandlerResult<T> Success(T value) => new(true, value, Array.Empty<HandlerResultError>());

    public static HandlerResult<T> Failure(params HandlerResultError[] errors) => Failure((IEnumerable<HandlerResultError>)errors);

    public static HandlerResult<T> Failure(IEnumerable<HandlerResultError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToArray();
        if (errorList.Length == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        return new HandlerResult<T>(false, default, errorList);
    }
}
