﻿namespace StatementProcessingService.Application.Exceptions;

public class FileUploadException : Exception
{
    public FileUploadException(string message) : base(message)
    { }
}