
public class LoginInfo
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterInfo
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public enum AuthStatusBits
{
    USERNAME_TAKEN  = 0b00001,
    EMAIL_TAKEN     = 0b00010,
    BAD_REQUEST     = 0b00100,
    SIGN_IN_FAILED  = 0b01000,
    SUCCESS         = 0b10000
}
