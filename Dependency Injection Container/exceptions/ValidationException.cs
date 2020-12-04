using System;

namespace Dependency_Injection_Container.exceptions
{
    public class ValidationException : Exception
    {
        private const string ConstMessage = "Invalid configuration";

        public ValidationException() : base(ConstMessage)
        {
            
        }

        public ValidationException(string message) : base(message)
        {
            
        }
        
    }
}