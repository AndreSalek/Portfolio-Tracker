public class AuthResult
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];

    public static AuthResult Success()
        => new AuthResult { Succeeded = true };

    public static AuthResult Failure(IEnumerable<string> errors)
        => new AuthResult { Succeeded = false, Errors = errors };
}