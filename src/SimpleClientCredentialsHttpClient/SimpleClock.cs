namespace SimpleClientCredentialsHttpClient;

internal interface IClock
{
    DateTime UtcNow { get; }
}

internal class SimpleClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
