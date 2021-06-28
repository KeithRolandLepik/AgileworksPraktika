namespace Domain.Users
{
    public class AuthenticateResult
    {
        public static AuthenticateResult Success(User user)
        {
            return new() { User = user };
        }
        public static AuthenticateResult Failure(string errorMessage)
        {
            return new() { ErrorMessage = errorMessage };
        }

        public User? User { get; set; }
        public string? ErrorMessage { get; set; }
    }
}