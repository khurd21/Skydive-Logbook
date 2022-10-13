namespace LogbookService.Dependencies.Encryptor;

public interface IEncryptor
{
    void Encrypt<T>(ref T plainText, in ICollection<string>? filter = null) where T : class;
    void Decrypt<T>(ref T cipherText, in ICollection<string>? filter = null) where T : class;
}