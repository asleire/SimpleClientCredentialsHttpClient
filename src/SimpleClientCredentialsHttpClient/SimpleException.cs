namespace SimpleClientCredentialsHttpClient;

public class SimpleException : Exception
{
}

public class TokenErrorException : Exception
{
    public TokenErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

public class AuthorizationHeaderAlreadySetException : SimpleException
{
}

public class NonOkTokenResponseException : SimpleException
{
    public required int StatusCode { get; set; }

    public required string? ResponseBody { get; set; }

    public required Exception? ResponseBodyReadException { get; set; }
}

public class NullTokenResponseException : SimpleException
{
}

public class EmptyTokenResponseAccessTokenException : SimpleException
{
}

public class EmptyTokenResponseExpiresInException : SimpleException
{
}
