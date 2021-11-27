namespace Authentication.Models
{
    public class LoginUser
    {
        public LoginUser(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; init; }
        public string Password { get; init; }
    }
}