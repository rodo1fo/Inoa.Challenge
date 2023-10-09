namespace Inoa.Challenge.Console.Infrastructure.Settings;

public class EmailSettings
{
    public string Smtp { get; set; }
    public int? Port { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public Credentials? Credentials { get; set; }
    public bool EnableSsl { get; set; }
}

public class Credentials
{
    public string Username { get; set; }
    public string Password { get; set; }
}