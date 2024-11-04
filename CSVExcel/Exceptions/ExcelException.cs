// ExcelException.cs
using System;

namespace CSVExcel.Exceptions;

public class ExcelException : Exception
{
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public ExcelException(string message, List<string> errors) : base(message)
    {
        Message = message;
        Errors = errors;
    }
}
