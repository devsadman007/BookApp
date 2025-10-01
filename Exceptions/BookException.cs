namespace BookApp.Exceptions
{
    public class BookException : Exception
    {
        public BookException( string message) : base (message) { }
    }

    public class BookNotFoundException : BookException {
        public BookNotFoundException (int id)
            : base($"Book with Id {id} was not found.") { }
    }

    // Invalid book data
    public class InvalidBookDataException : BookException
    {
        public InvalidBookDataException(string message)
            : base(message) { }
    }

    // Simulated DB error (like DbUpdateException)
    public class BookDatabaseException : BookException
    {
        public BookDatabaseException(string message)
            : base(message) { }
    }
}
