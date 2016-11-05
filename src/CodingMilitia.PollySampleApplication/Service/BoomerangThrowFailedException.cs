using System;

namespace CodingMilitia.PollySampleApplication.Service
{
    internal class BoomerangThrowFailedException : Exception
    {
        public BoomerangThrowFailedException()
        {
        }

        public BoomerangThrowFailedException(string message) : base(message)
        {
        }

        public BoomerangThrowFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}