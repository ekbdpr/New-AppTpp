using System;
using System.Windows;

namespace AppTpp.Exceptions
{
    internal class DuplicateDataException : Exception
    {
        public DuplicateDataException(string message) : base(message) { }
    }
}
