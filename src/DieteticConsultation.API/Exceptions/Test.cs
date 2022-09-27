namespace DieteticConsultationAPI.Exceptions;

public class StorageException : Exception
{
    private StorageException() : base("There is an storage exception")
    {
    }

    private StorageException(string message) : base(message)
    {
    }

    private StorageException(string message, Exception ex) : base(message, ex)
    {
    }

    private StorageException(Exception ex) : base(ex.Message, ex)
    {
    }

    public static void For(string message) => throw new StorageException(message);

    public static void For(string message, Exception ex) => throw new StorageException(message, ex);

    public static void For(Exception ex) => throw new StorageException(ex);
    
    public static void For() => throw new StorageException();
}