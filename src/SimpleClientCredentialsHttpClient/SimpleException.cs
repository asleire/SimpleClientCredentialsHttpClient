namespace SimpleClientCredentialsHttpClient;

public class TokenErrorException : Exception
{
    public TokenErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
    
    public TokenErrorException(string? message) : base(message)
    {
    }
    
    public TokenErrorException() 
    {
    }
}

public class AuthorizationHeaderAlreadySetException : TokenErrorException
{
}

public class NonOkTokenResponseException : TokenErrorException
{
    public NonOkTokenResponseException(int statusCode, string? responseBody, Exception? responseBodyReadException) : base(FormatMessage(statusCode, responseBody, responseBodyReadException))
    {
    }

    private static string FormatMessage(int statusCode, string? responseBody, Exception? responseBodyReadException)
    {
        var responseBodyString = responseBodyReadException != null
            ? $"Error reading response body: {responseBodyReadException.Message}"
            : (responseBody == null ? "Response body is empty." : $"Response body: {responseBody}");
        return $"Received status code {statusCode} from token endpoint. {responseBodyString}";
    }
}

public class NullTokenResponseException : TokenErrorException
{
}

public class EmptyTokenResponseAccessTokenException : TokenErrorException
{
}

public class EmptyTokenResponseExpiresInException : TokenErrorException
{
}
