using NUnit.Framework;

namespace Softfire.MonoGame.UTESTS
{
    public class IOEncryptionTests
    {
        /// <summary>
        /// Asymmetric Encrypt.
        /// </summary>
        /// <param name="plainBytes">The byte array to encrypt.</param>
        /// <returns>Returns an encrypted byte array.</returns>
        [Test]
        private byte[] AsymmetricEncrypt(byte[] plainBytes)
        {
            IO.Encryption.IOAsymmetricEncryption.GenerateAndStoreRsaKeyPair("RSAKeyPairTest");
            
            return IO.Encryption.IOAsymmetricEncryption.Encrypt("RSAKeyPairTest", plainBytes);
        }

        /// <summary>
        /// Asymmetric Decrypt.
        /// </summary>
        /// <param name="encryptedBytes">The byte array to decrypt.</param>
        /// <returns>Returns a decrypted byte array.</returns>
        [Test]
        private byte[] AsymmetricDecrypt(byte[] encryptedBytes)
        {
            return IO.Encryption.IOAsymmetricEncryption.Decrypt("RSAKeyPairTest", encryptedBytes);
        }

        /// <summary>
        /// Symmetric Encrypt.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>Returns an encrypted string.</returns>
        [Test]
        private string SymmetricEncrypt(string plainText, string secretKey)
        {
            return IO.Encryption.IOSymmetricEncryption.Encrypt(plainText, secretKey);
        }

        /// <summary>
        /// Symmetric Encrypt.
        /// </summary>
        /// <param name="encryptedText">The text to decrypt.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>Returns a decrypted string.</returns>
        [Test]
        private string SymmetricDecrypt(string encryptedText, string secretKey)
        {
            return IO.Encryption.IOSymmetricEncryption.Decrypt(encryptedText, secretKey);
        }

        /// <summary>
        /// Symmetric Encrypt.
        /// </summary>
        /// <param name="plainBytes">The byte array to encrypt.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>Returns an encrypted byte array.</returns>
        [Test]
        private byte[] SymmetricEncrypt(byte[] plainBytes, string secretKey)
        {
            return IO.Encryption.IOSymmetricEncryption.Encrypt(plainBytes, secretKey);
        }

        /// <summary>
        /// Symmetric Encrypt.
        /// </summary>
        /// <param name="encryptedBytes">The byte array to decrypt.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <returns>Returns a decrypted byte array.</returns>
        [Test]
        private byte[] SymmetricDecrypt(byte[] encryptedBytes, string secretKey)
        {
            return IO.Encryption.IOSymmetricEncryption.Decrypt(encryptedBytes, secretKey);
        }

        [Test]
        public void DeleteKeys()
        {
            IO.Encryption.IOAsymmetricEncryption.DeleteStoredRsaKeyPair("RSAKeyPairTest");
        }

        [Test]
        public void TestAsymmetricEncryptionDecryption()
        {
            var plainBytes = new byte[] {1, 0, 1};
            var encryptedBytes = AsymmetricEncrypt(plainBytes);
            var decryptedBytes = AsymmetricDecrypt(encryptedBytes);

            CollectionAssert.AreEqual(plainBytes, decryptedBytes);
        }

        [Test]
        public void TestSymmetricStringEncryptionDecryption()
        {
            var plaintText = "Test";
            var secretKey = IO.Encryption.IOSymmetricEncryption.GenerateUtf8SymmetricKey();
            var encryptedText = SymmetricEncrypt(plaintText, secretKey);
            var decryptedText = SymmetricDecrypt(encryptedText, secretKey);

            Assert.AreNotEqual(plaintText, encryptedText);
            Assert.AreEqual(plaintText, decryptedText);
        }

        [Test]
        public void TestSymmetricBytesEncryptionDecryption()
        {
            var plainBytes = new byte[]
            {
                1, 0, 1, 0, 0, 1, 0, 1, 0, 0,
                1, 0, 1, 0, 0, 1, 0, 1, 0, 0,
                1, 0, 1, 0, 0, 1, 0, 1, 0, 0,
                1, 0, 1, 0, 0, 1, 0, 1, 0, 0
            };
            var secretKey = IO.Encryption.IOSymmetricEncryption.GenerateUtf8SymmetricKey();
            var encryptedBytes = SymmetricEncrypt(plainBytes, secretKey);
            var decryptedBytes = SymmetricDecrypt(encryptedBytes, secretKey);

            CollectionAssert.AreNotEqual(plainBytes, encryptedBytes);
            CollectionAssert.AreEqual(plainBytes, decryptedBytes);
        }
    }
}