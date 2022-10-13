using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using LogbookService.Settings;

namespace LogbookService.Dependencies.Encryptor;

/// <summary>
/// TODO: Integer encryption is not working
/// </summary>
public class EncryptorProvider : IEncryptor
{
    private static string KeyFile => ProjectSettings.KeyFile;

    private static string PathToKeyFile { get; } = Path.Combine(ProjectSettings.GetSolutionDirectoryInfo().FullName, EncryptorProvider.KeyFile);

    public void Encrypt<T>(ref T plainText, in ICollection<string>? filter = null) where T : class
    {
        if (plainText is null)
        {
            return;
        }
        else if (plainText is string)
        {
            plainText = (T)(object)this.Encrypt((plainText as string)!);
            return;
        }
        else if (plainText is int?)
        {
            plainText = (T)(object)this.Encrypt((plainText as int?)!.Value);
            return;
        }

        Console.WriteLine($"Encrypting {typeof(T).Name}...");
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            Console.WriteLine($"Encrypting {property.Name}...");
        }

        foreach (PropertyInfo property in plainText.GetType().GetProperties())
        {
            if (filter != null && !filter.Contains(property.Name))
            {
                continue;
            }

            else if (property.PropertyType == typeof(string))
            {
                string? value = property.GetValue(plainText) as string;
                if (value != null)
                {
                    property.SetValue(plainText, this.Encrypt(value));
                }
            }

            else if (property.PropertyType == typeof(int?))
            {
                continue;
                /*int? value = property.GetValue(plainText) as int?;
                if (value != null)
                {
                    property.SetValue(plainText, this.Encrypt(value.Value));
                }*/
            }

            else
            {
                var value = property.GetValue(plainText);
                if (value != null)
                {
                    this.Encrypt(ref value, filter);
                    property.SetValue(plainText, value);
                }
            }
        }
    }

    public void Decrypt<T>(ref T cipherText, in ICollection<string>? filter = null) where T : class
    {
        if (cipherText is null)
        {
            return;
        }
        else if (cipherText is string)
        {
            cipherText = (T)(object)this.Decrypt((cipherText as string)!);
            return;
        }
        else if (cipherText is int?)
        {
            cipherText = (T)(object)this.Decrypt((cipherText as int?)!.Value);
            return;
        }

        foreach (PropertyInfo property in cipherText.GetType().GetProperties())
        {
            if (filter != null && !filter.Contains(property.Name))
            {
                continue;
            }

            if (property.PropertyType == typeof(string))
            {
                string? value = property.GetValue(cipherText) as string;
                if (value != null)
                {
                    property.SetValue(cipherText, this.Decrypt(value));
                }
            }

            if (property.PropertyType == typeof(int?))
            {
                continue;
                /*int? value = property.GetValue(cipherText) as int?;
                if (value != null)
                {
                    property.SetValue(cipherText, this.Decrypt(value.Value));
                }*/
            }
        }
    }

    public string Encrypt(in string plainText)
    {
        string key = EncryptorProvider.RetrieveKey();
        byte[] iv = new byte[16];
        byte[] encrypted;  

        using(Aes aes = Aes.Create()) {  
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor();  

            using(MemoryStream ms = new MemoryStream()) {  
                using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {  
                    using(StreamWriter sw = new StreamWriter(cs))  
                    {
                        sw.Write(plainText);  
                    }
                    encrypted = ms.ToArray();  
                }  
            }  
        }  
        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(in string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        string key = EncryptorProvider.RetrieveKey();
        byte[] iv = new byte[16];
        string plaintext = null!;

        using(Aes aes = Aes.Create()) {  
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor();  
            using (MemoryStream ms = new MemoryStream(cipherBytes))
            {  
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {  
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        plaintext = reader.ReadToEnd();  
                    }
                }  
            }  
        }  
        return plaintext;  
    }  

    private int Encrypt(in int plainText)
    {
        string password = EncryptorProvider.RetrieveKey();
        byte[] iv = new byte[16];

        using (Aes aes = Aes.Create())   // System.Security.Cryptography
    {
        aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
        aes.IV = iv;
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var result = -1;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write, true))
            using (var bw = new BinaryWriter(cs))
                bw.Write(plainText);
            ms.Seek(0, SeekOrigin.Begin);
            using (var br = new BinaryReader(ms))
                while (ms.Position < ms.Length)
                    result = br.ReadInt32();
        }
        return result;
    }
    }

    private int Decrypt(in int cipherText)
    {
        string password = EncryptorProvider.RetrieveKey();
        byte[] iv = new byte[16];

        using (Aes aes = Aes.Create())  
    {
        aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
        aes.IV = iv;
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        var result = -1;
        using (var ms = new MemoryStream())
        {
            using (var bw = new BinaryWriter(ms, Encoding.UTF8, true))
                    bw.Write(cipherText);
            ms.Seek(0, SeekOrigin.Begin);
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var br = new BinaryReader(cs))
                    try
                    {
                        result = br.ReadInt32();
                    } catch (EndOfStreamException) { }
        }
        return result;
    }
    }

    private static string RetrieveKey()
    {
        using StreamReader file = new(PathToKeyFile);
        string? key = file.ReadLine();
        file.Close();

        if (key == null)
        {
            throw new FileNotFoundException("Key file could not be found");
        }

        return key;
    }
}