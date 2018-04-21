using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Softfire.MonoGame.LOG;

namespace Softfire.MonoGame.IO.Encryption
{
    public static class IOSymmetricEncryption
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private static Logger Logger { get; }

        /// <summary>
        /// Derivation Iterations.
        /// The number of iterations for the password bytes generation.
        /// </summary>
        private const int DerivationIterations = 100000;

        /// <summary>
        /// IO Symmetric Encryption Constructor.
        /// </summary>
        static IOSymmetricEncryption()
        {
            Logger = new Logger(@"Config\Logs\Encryption");
        }

        #region Encryption Methods

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="secretKey">Secret key used to encrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted string otherwise null.</returns>
        public static string Encrypt(string plainText, string secretKey, int keySize = 256)
        {
            string result = null;

            try
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                var saltStringBytes = GenerateRandomBytes();
                var ivStringBytes = GenerateRandomBytes();
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();

                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                    var encryptedTextBytes = saltStringBytes;
                                    encryptedTextBytes = encryptedTextBytes.Concat(ivStringBytes).ToArray();
                                    encryptedTextBytes = encryptedTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                    result = Convert.ToBase64String(encryptedTextBytes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException || 
                    ex is EncoderFallbackException ||
                    ex is ArgumentException || 
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Encrypt Async.
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="secretKey">Secret key used to encrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted string otherwise null.</returns>
        public static async Task<string> EncryptAsync(string plainText, string secretKey, int keySize = 256)
        {
            string result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                    // so that the same Salt and IV values can be used when decrypting.  
                    var saltStringBytes = GenerateRandomBytes();
                    var ivStringBytes = GenerateRandomBytes();
                    var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                    using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                    {
                                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                        cryptoStream.FlushFinalBlock();

                                        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                        var encryptedTextBytes = saltStringBytes;
                                        encryptedTextBytes = encryptedTextBytes.Concat(ivStringBytes).ToArray();
                                        encryptedTextBytes = encryptedTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                        return Convert.ToBase64String(encryptedTextBytes);
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is EncoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="plainBytes">Byte array to encrypt.</param>
        /// <param name="secretKey">Secret key used to encrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted byte array.</returns>
        public static byte[] Encrypt(byte[] plainBytes, string secretKey, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                var saltStringBytes = GenerateRandomBytes();
                var ivStringBytes = GenerateRandomBytes();

                using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                                    cryptoStream.FlushFinalBlock();

                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                    var encryptedBytes = saltStringBytes;
                                    encryptedBytes = encryptedBytes.Concat(ivStringBytes).ToArray();
                                    encryptedBytes = encryptedBytes.Concat(memoryStream.ToArray()).ToArray();
                                    result = encryptedBytes;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is EncoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Encrypt Async.
        /// </summary>
        /// <param name="plainBytes">Byte array to encrypt.</param>
        /// <param name="secretKey">Secret key used to encrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted byte array.</returns>
        public static async Task<byte[]> EncryptAsync(byte[] plainBytes, string secretKey, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                    // so that the same Salt and IV values can be used when decrypting.  
                    var saltStringBytes = GenerateRandomBytes();
                    var ivStringBytes = GenerateRandomBytes();

                    using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                    {
                                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                                        cryptoStream.FlushFinalBlock();

                                        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                        var encryptedBytes = saltStringBytes;
                                        encryptedBytes = encryptedBytes.Concat(ivStringBytes).ToArray();
                                        encryptedBytes = encryptedBytes.Concat(memoryStream.ToArray()).ToArray();
                                        return encryptedBytes;
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is EncoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to encrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted string.</returns>
        public static string Encrypt(string plainText, byte[] secretKeyBytes, int keySize = 256)
        {
            string result = null;

            try
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                var saltStringBytes = GenerateRandomBytes();
                var ivStringBytes = GenerateRandomBytes();
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();

                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                    var encryptedTextBytes = saltStringBytes;
                                    encryptedTextBytes = encryptedTextBytes.Concat(ivStringBytes).ToArray();
                                    encryptedTextBytes = encryptedTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                    result = Convert.ToBase64String(encryptedTextBytes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is EncoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Encrypt Async.
        /// </summary>
        /// <param name="plainText">Text to encrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to encrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted string.</returns>
        public static async Task<string> EncryptAsync(string plainText, byte[] secretKeyBytes, int keySize = 256)
        {
            string result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                    // so that the same Salt and IV values can be used when decrypting.  
                    var saltStringBytes = GenerateRandomBytes();
                    var ivStringBytes = GenerateRandomBytes();
                    var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                    using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                    {
                                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                        cryptoStream.FlushFinalBlock();

                                        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                        var encryptedTextBytes = saltStringBytes;
                                        encryptedTextBytes = encryptedTextBytes.Concat(ivStringBytes).ToArray();
                                        encryptedTextBytes = encryptedTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                        return Convert.ToBase64String(encryptedTextBytes);
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is EncoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Encrypt.
        /// </summary>
        /// <param name="plainBytes">Byte array to encrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to encrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted byte array.</returns>
        public static byte[] Encrypt(byte[] plainBytes, byte[] secretKeyBytes, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                var saltStringBytes = GenerateRandomBytes();
                var ivStringBytes = GenerateRandomBytes();

                using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                                    cryptoStream.FlushFinalBlock();

                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                    var encryptedBytes = saltStringBytes;
                                    encryptedBytes = encryptedBytes.Concat(ivStringBytes).ToArray();
                                    encryptedBytes = encryptedBytes.Concat(memoryStream.ToArray()).ToArray();
                                    result = encryptedBytes;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is EncoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Encrypt Async.
        /// </summary>
        /// <param name="plainBytes">Byte array to encrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to encrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns an encrypted byte array.</returns>
        public static async Task<byte[]> EncryptAsync(byte[] plainBytes, byte[] secretKeyBytes, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                    // so that the same Salt and IV values can be used when decrypting.  
                    var saltStringBytes = GenerateRandomBytes();
                    var ivStringBytes = GenerateRandomBytes();

                    using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                    {
                                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                                        cryptoStream.FlushFinalBlock();

                                        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                        var encryptedBytes = saltStringBytes;
                                        encryptedBytes = encryptedBytes.Concat(ivStringBytes).ToArray();
                                        encryptedBytes = encryptedBytes.Concat(memoryStream.ToArray()).ToArray();
                                        return encryptedBytes;
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is EncoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        #endregion

        #region Decryption Methods

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="secretKey">Secret key used to decrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns a decrypted string.</returns>
        public static string Decrypt(string encryptedText, string secretKey, int keySize = 256)
        {
            string result = null;

            try
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedText]
                var encryptedTextBytesWithSaltAndIv = Convert.FromBase64String(encryptedText);
                // Get the salt bytes by extracting the first 32 bytes from the supplied encryptedText bytes.
                var saltStringBytes = encryptedTextBytesWithSaltAndIv.Take(keySize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedText bytes.
                var ivStringBytes = encryptedTextBytesWithSaltAndIv.Skip(keySize / 8).Take(keySize / 8).ToArray();
                // Get the actual encrypted text bytes by removing the first 64 bytes from the encryptedText string.
                var encryptedTextBytes = encryptedTextBytesWithSaltAndIv.Skip((keySize / 8) * 2).Take(encryptedTextBytesWithSaltAndIv.Length - ((keySize / 8) * 2)).ToArray();

                using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(encryptedTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[encryptedTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt Async.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="secretKey">Secret key used to decrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns a decrypted string.</returns>
        public static async Task<string> DecryptAsync(string encryptedText, string secretKey, int keySize = 256)
        {
            string result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Get the complete stream of bytes that represent:
                    // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedText]
                    var encryptedTextBytesWithSaltAndIv = Convert.FromBase64String(encryptedText);
                    // Get the salt bytes by extracting the first 32 bytes from the supplied encryptedText bytes.
                    var saltStringBytes = encryptedTextBytesWithSaltAndIv.Take(keySize / 8).ToArray();
                    // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedText bytes.
                    var ivStringBytes = encryptedTextBytesWithSaltAndIv.Skip(keySize / 8).Take(keySize / 8).ToArray();
                    // Get the actual encrypted text bytes by removing the first 64 bytes from the encryptedText string.
                    var encryptedTextBytes = encryptedTextBytesWithSaltAndIv.Skip((keySize / 8) * 2).Take(encryptedTextBytesWithSaltAndIv.Length - ((keySize / 8) * 2)).ToArray();

                    using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream(encryptedTextBytes))
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                    {
                                        var plainTextBytes = new byte[encryptedTextBytes.Length];
                                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="encryptedBytesArray">The encrypted byte array to decrypt.</param>
        /// <param name="secretKey">Secret key used to decrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns the decrypted bytes.</returns>
        public static byte[] Decrypt(byte[] encryptedBytesArray, string secretKey, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedBytesArray]
                // Get the saltbytes by extracting the first 32 bytes from the supplied encryptedBytesArray.
                var saltStringBytes = encryptedBytesArray.Take(keySize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedBytesArray.
                var ivStringBytes = encryptedBytesArray.Skip(keySize / 8).Take(keySize / 8).ToArray();
                // Get the actual encrypted bytes by removing the first 64 bytes from the encryptedBytesArray.
                var encryptedBytes = encryptedBytesArray.Skip((keySize / 8) * 2).Take(encryptedBytesArray.Length - ((keySize / 8) * 2)).ToArray();

                using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(encryptedBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainBytes = new byte[encryptedBytesArray.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
                                    result = plainBytes.Take(decryptedByteCount).ToArray();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt Async.
        /// </summary>
        /// <param name="encryptedBytesArray">The encrypted byte array to decrypt.</param>
        /// <param name="secretKey">Secret key used to decrypt the text.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns the decrypted bytes.</returns>
        public static async Task<byte[]> DecryptAsync(byte[] encryptedBytesArray, string secretKey, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Get the complete stream of bytes that represent:
                    // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedBytesArray]
                    // Get the saltbytes by extracting the first 32 bytes from the supplied encryptedBytesArray.
                    var saltStringBytes = encryptedBytesArray.Take(keySize / 8).ToArray();
                    // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedBytesArray.
                    var ivStringBytes = encryptedBytesArray.Skip(keySize / 8).Take(keySize / 8).ToArray();
                    // Get the actual encrypted bytes by removing the first 64 bytes from the encryptedBytesArray.
                    var encryptedBytes = encryptedBytesArray.Skip((keySize / 8) * 2).Take(encryptedBytesArray.Length - ((keySize / 8) * 2)).ToArray();

                    using (var password = new Rfc2898DeriveBytes(secretKey, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream(encryptedBytes))
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                    {
                                        var plainBytes = new byte[encryptedBytesArray.Length];
                                        var decryptedByteCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
                                        return plainBytes.Take(decryptedByteCount).ToArray();
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to decrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns a decrypted string.</returns>
        public static string Decrypt(string encryptedText, byte[] secretKeyBytes, int keySize = 256)
        {
            string result = null;

            try
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedText]
                var encryptedTextBytesWithSaltAndIv = Convert.FromBase64String(encryptedText);
                // Get the salt bytes by extracting the first 32 bytes from the supplied encryptedText bytes.
                var saltStringBytes = encryptedTextBytesWithSaltAndIv.Take(keySize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedText bytes.
                var ivStringBytes = encryptedTextBytesWithSaltAndIv.Skip(keySize / 8).Take(keySize / 8).ToArray();
                // Get the actual encrypted text bytes by removing the first 64 bytes from the encryptedText string.
                var encryptedTextBytes = encryptedTextBytesWithSaltAndIv.Skip((keySize / 8) * 2).Take(encryptedTextBytesWithSaltAndIv.Length - ((keySize / 8) * 2)).ToArray();

                using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(encryptedTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[encryptedTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt Async.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to decrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns a decrypted string.</returns>
        public static async Task<string> DecryptAsync(string encryptedText, byte[] secretKeyBytes, int keySize = 256)
        {
            string result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Get the complete stream of bytes that represent:
                    // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedText]
                    var encryptedTextBytesWithSaltAndIv = Convert.FromBase64String(encryptedText);
                    // Get the salt bytes by extracting the first 32 bytes from the supplied encryptedText bytes.
                    var saltStringBytes = encryptedTextBytesWithSaltAndIv.Take(keySize / 8).ToArray();
                    // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedText bytes.
                    var ivStringBytes = encryptedTextBytesWithSaltAndIv.Skip(keySize / 8).Take(keySize / 8).ToArray();
                    // Get the actual encrypted text bytes by removing the first 64 bytes from the encryptedText string.
                    var encryptedTextBytes = encryptedTextBytesWithSaltAndIv.Skip((keySize / 8) * 2).Take(encryptedTextBytesWithSaltAndIv.Length - ((keySize / 8) * 2)).ToArray();

                    using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream(encryptedTextBytes))
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                    {
                                        var plainTextBytes = new byte[encryptedTextBytes.Length];
                                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="encryptedBytesArray">The encrypted byte array to decrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to decrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns the decrypted bytes.</returns>
        public static byte[] Decrypt(byte[] encryptedBytesArray, byte[] secretKeyBytes, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedBytesArray]
                // Get the saltbytes by extracting the first 32 bytes from the supplied encryptedBytesArray.
                var saltStringBytes = encryptedBytesArray.Take(keySize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedBytesArray.
                var ivStringBytes = encryptedBytesArray.Skip(keySize / 8).Take(keySize / 8).ToArray();
                // Get the actual encrypted bytes by removing the first 64 bytes from the encryptedBytesArray.
                var encryptedBytes = encryptedBytesArray.Skip((keySize / 8) * 2).Take(encryptedBytesArray.Length - ((keySize / 8) * 2)).ToArray();

                using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(keySize / 8);

                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;

                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(encryptedBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainBytes = new byte[encryptedBytesArray.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
                                    result = plainBytes.Take(decryptedByteCount).ToArray();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt Async.
        /// </summary>
        /// <param name="encryptedBytesArray">The encrypted byte array to decrypt.</param>
        /// <param name="secretKeyBytes">Secret byte array used to decrypt the array.</param>
        /// <param name="keySize">The keysize of the encryption algorithm in bits. Divide by 8 to get the equivalent number of bytes.</param>
        /// <returns>Returns the decrypted bytes.</returns>
        public static async Task<byte[]> DecryptAsync(byte[] encryptedBytesArray, byte[] secretKeyBytes, int keySize = 256)
        {
            byte[] result = null;

            try
            {
                result = await Task.Run(() =>
                {
                    // Get the complete stream of bytes that represent:
                    // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of EncryptedBytesArray]
                    // Get the saltbytes by extracting the first 32 bytes from the supplied encryptedBytesArray.
                    var saltStringBytes = encryptedBytesArray.Take(keySize / 8).ToArray();
                    // Get the IV bytes by extracting the next 32 bytes from the supplied encryptedBytesArray.
                    var ivStringBytes = encryptedBytesArray.Skip(keySize / 8).Take(keySize / 8).ToArray();
                    // Get the actual encrypted bytes by removing the first 64 bytes from the encryptedBytesArray.
                    var encryptedBytes = encryptedBytesArray.Skip((keySize / 8) * 2).Take(encryptedBytesArray.Length - ((keySize / 8) * 2)).ToArray();

                    using (var password = new Rfc2898DeriveBytes(secretKeyBytes, saltStringBytes, DerivationIterations))
                    {
                        var keyBytes = password.GetBytes(keySize / 8);

                        using (var symmetricKey = new RijndaelManaged())
                        {
                            symmetricKey.BlockSize = 256;
                            symmetricKey.Mode = CipherMode.CBC;
                            symmetricKey.Padding = PaddingMode.PKCS7;

                            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                            {
                                using (var memoryStream = new MemoryStream(encryptedBytes))
                                {
                                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                    {
                                        var plainBytes = new byte[encryptedBytesArray.Length];
                                        var decryptedByteCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
                                        return plainBytes.Take(decryptedByteCount).ToArray();
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is FormatException ||
                    ex is ArgumentNullException ||
                    ex is ArgumentOutOfRangeException ||
                    ex is DecoderFallbackException ||
                    ex is ArgumentException ||
                    ex is NotSupportedException ||
                    ex is InvalidOperationException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return result;
        }

        #endregion

        #region Generation Methods

        /// <summary>
        /// Generate Random Bytes.
        /// </summary>
        /// <param name="keySize">The key size in bytes.</param>
        /// <returns>Returns a byte aray of cryptographically secure random bytes.</returns>
        private static byte[] GenerateRandomBytes(int keySize = 256)
        {
            var randomBytes = new byte[keySize / 8];

            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(randomBytes);
            }

            return randomBytes;
        }

        /// <summary>
        /// Generate UTF-8 Symmetric Key.
        /// </summary>
        /// <param name="keySize">The key size in bytes.</param>
        /// <returns>Returns a symmetric key as a UTF-8 encoded string.</returns>
        public static string GenerateUtf8SymmetricKey(int keySize = 256)
        {
            return Encoding.UTF8.GetString(GenerateRandomBytes(keySize));
        }

        /// <summary>
        /// Generate Symmertic Key.
        /// </summary>
        /// <param name="keySize">The key size in bytes.</param>
        /// <returns>Returns a symmetric key as an array of bytes.</returns>
        public static byte[] GenerateSymmetricKey(int keySize = 256)
        {
            return GenerateRandomBytes(keySize);
        }

        #endregion
    }
}