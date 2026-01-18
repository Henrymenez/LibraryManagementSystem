namespace LibraryManagementSystem.Services.Interface
{
    public interface IPasswordHasher
    {
        void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}
