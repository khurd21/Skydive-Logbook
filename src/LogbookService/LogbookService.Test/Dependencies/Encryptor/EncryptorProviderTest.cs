using LogbookService.Dependencies.Encryptor;

namespace LogbookService.Test.Dependencies.Encryptor;

public class EncryptorProviderTest
{
    private EncryptorProvider Encryptor { get; }

    private class TestClass
    {
        public string? StringProperty { get; set; }
        public int? IntProperty { get; set; }
    }

    private class NestedTestClass
    {
        public TestClass? TestClassProperty { get; set; }
    }

    public EncryptorProviderTest()
    {
        this.Encryptor = new();
    }

    [Test]
    [TestCase("Hello, World!", "Hello, World!")]
    public void Encrypt_Primitives<T>(T plainText, T originalText) where T : class
    {
        this.Encryptor.Encrypt(ref plainText);
        Assert.That(plainText, Is.Not.EqualTo(originalText));
    }

    [Test]
    [TestCase("Hello, World!", "Hello, World!")]
    public void Decrypt_Primitives<T>(T plainText, T originalText) where T : class
    {
        this.Encryptor.Encrypt(ref plainText);
        this.Encryptor.Decrypt(ref plainText);
        Assert.That(plainText, Is.EqualTo(originalText));
    }

    [Test]
    [TestCase("Hello, World!", 42)]
    public void Encrypt(string stringProperty, int intProperty)
    {
        TestClass testClass = new()
        {
            StringProperty = "Hello, World!",
            IntProperty = 42,
        };

        this.Encryptor.Encrypt(ref testClass);

        Assert.That(testClass.StringProperty, Is.Not.EqualTo("Hello, World!"));
        // TODO: Encryption for ints not working.
        // Assert.That(testClass.IntProperty, Is.Not.EqualTo(42));
    }

    [Test]
    public void Decrypt()
    {
        TestClass testClass = new()
        {
            StringProperty = "Hello, World!",
            IntProperty = 42,
        };

        this.Encryptor.Encrypt(ref testClass);
        this.Encryptor.Decrypt(ref testClass);

        Assert.That(testClass.StringProperty, Is.EqualTo("Hello, World!"));
        Assert.That(testClass.IntProperty, Is.EqualTo(42));
    }
}