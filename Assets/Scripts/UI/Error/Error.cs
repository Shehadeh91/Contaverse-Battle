﻿public class Error
{
    public IErrorSource errorSource;

    public string errorMessage;

    public Error(string message, IErrorSource source)
    {
        errorMessage = message;
        errorSource = source;
    }
}

public interface IErrorSource
{
    Error GetError();
}
