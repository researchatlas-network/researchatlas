namespace ResearchAtlas.Application.Abstractions.Core;

public sealed class OperationResult
{
    private OperationResult(bool isSuccess, IReadOnlyList<OperationResultError> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<OperationResultError> Errors { get; }

    public static OperationResult Success() => new(true, Array.Empty<OperationResultError>());

    public static OperationResult Failure(params OperationResultError[] errors) => Failure((IEnumerable<OperationResultError>)errors);

    public static OperationResult Failure(IEnumerable<OperationResultError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToArray();
        if (errorList.Length == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        return new OperationResult(false, errorList);
    }
}

public sealed class OperationResult<T>
{
    private OperationResult(bool isSuccess, T? value, IReadOnlyList<OperationResultError> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public IReadOnlyList<OperationResultError> Errors { get; }

    public static OperationResult<T> Success(T value) => new(true, value, Array.Empty<OperationResultError>());

    public static OperationResult<T> Failure(params OperationResultError[] errors) => Failure((IEnumerable<OperationResultError>)errors);

    public static OperationResult<T> Failure(IEnumerable<OperationResultError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToArray();
        if (errorList.Length == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        return new OperationResult<T>(false, default, errorList);
    }
}
