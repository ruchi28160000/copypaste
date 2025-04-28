using System;

using System.Linq;

using System.Threading.Tasks;
namespace EmailVerification.Services
{

public class UserService

{

    private readonly List<User> _users = new List<User>();  // You will use a database instead in real apps

    private readonly EmailService _emailService;

    public UserService(EmailService emailService)

    {

        _emailService = emailService;

    }

    public async Task<User> RegisterUserAsync(string email, string password)

    {

        // Check if the user already exists

        if (_users.Any(u => u.Email == email))

            throw new Exception("User already exists.");

        var verificationToken = Guid.NewGuid().ToString();

        var newUser = new User

        {

            Id = _users.Count + 1,

            Email = email,

            Password = password,

            IsEmailVerified = false,

            VerificationToken = verificationToken

        };

        _users.Add(newUser);

        // Send the verification email

        var verificationLink = $"http://localhost:5104/api/auth/verify-email?token={verificationToken}";

        var emailBody = $"<h3>Verify your email address</h3><p>Please click the link below to verify your email:</p><p><a href='{verificationLink}'>Verify Email</a></p>";

        await _emailService.SendEmailAsync(email, "Email Verification", emailBody);

        return newUser;

    }

    public User VerifyEmail(string token)

    {

        var user = _users.FirstOrDefault(u => u.VerificationToken == token);

        if (user == null)

            throw new Exception("Invalid verification token.");

        user.IsEmailVerified = true;

        user.VerificationToken = null;  // Invalidate the token after use

        return user;

    }

}
}
 