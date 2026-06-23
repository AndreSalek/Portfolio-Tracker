public class UpdateUserResult
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];

    public static UpdateUserResult Success()
        => new UpdateUserResult { Succeeded = true };

    public static UpdateUserResult Failure(IEnumerable<string> errors)
        => new UpdateUserResult { Succeeded = false, Errors = errors };
}