using System;

namespace TodoApi.Services.Errors
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message)
            : base(message)
        {
        }
    }
}
