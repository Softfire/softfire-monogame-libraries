using Softfire.MonoGame.LOG.V2;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Softfire.MonoGame.IO.V2.Encryption
{
    public static class IOAsymmetricEncryption
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private static Logger Logger { get; }

        /// <summary>
        /// Active Public Keys.
        /// </summary>
        private static Dictionary<string, RSAParameters> ActivePuclicKeys { get; }

        /// <summary>
        /// IO Asymmetric Encryption Constructor.
        /// </summary>
        static IOAsymmetricEncryption()
        {
            Logger = new Logger(@"Config\Logs\Encryption");
            ActivePuclicKeys = new Dictionary<string, RSAParameters>();
        }

        /// <summary>
        /// Add Active Public Key.
        /// Adds a public key, usually one received from another source, to the active public keys dictionary.
        /// Uses a string as the identifying key.
        /// </summary>
        /// <param name="identifier">The identifier for the public key. Intaken as a <see cref="string"/>.</param>
        /// <param name="publicKey">The public key to store. Intaken as an array of bytes.</param>
        /// <returns>Returns a bool indicating whether the public key was added successfully.</returns>
        public static bool AddActivePublicKey(string identifier, RSAParameters publicKey)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(identifier))
            {
                ActivePuclicKeys.Add(identifier, publicKey);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Active Public Key.
        /// </summary>
        /// <param name="identifier">The identifier for the public key. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a the requested public key, otherwise null.</returns>
        public static RSAParameters GetActivePublicKey(string identifier)
        {
            RSAParameters result = new RSAParameters();

            if (ActivePuclicKeys.ContainsKey(identifier))
            {
                result = ActivePuclicKeys[identifier];
            }

            return result;
        }

        /// <summary>
        /// Remove Active Public Key.
        /// </summary>
        /// <param name="identifier">The identifer for the public key. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a bool indicating whether the public key was removed successfully.</returns>
        public static bool RemoveActivePublicKey(string identifier)
        {
            var result = false;

            if (ActivePuclicKeys.ContainsKey(identifier))
            {
                ActivePuclicKeys.Remove(identifier);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Clear All ACtive Public Keys.
        /// </summary>
        public static void ClearAllActivePublicKeys()
        {
            ActivePuclicKeys?.Clear();
        }

        /// <summary>
        /// Generate And Store RSA Key Pair.
        /// Creates a new bit key pair from a new instance of the RSACryptoServiceProvider class.
        /// </summary>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns an RSAParameters object containing a new 2048 bit key pair.</returns>
        public static RSAParameters GenerateRsaKeyPair(int keyBitSize = 2048)
        {
            RSAParameters rsaParameters = new RSAParameters();

            try
            {
                using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize))
                {
                    rsaCsp.PersistKeyInCsp = false;
                    rsaParameters = rsaCsp.ExportParameters(includePrivateParameters: true);
                    rsaCsp.Clear();
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return rsaParameters;
        }

        /// <summary>
        /// Generate And Store RSA Key Pair.
        /// Creates a new bit key pair from a new instance of the RSACryptoServiceProvider class and stores them in a container with the name provided.
        /// </summary>
        /// <param name="containerName">The key pairs container name.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        public static void GenerateAndStoreRsaKeyPair(string containerName, int keyBitSize = 2048)
        {
            try
            {
                var cspParameters = new CspParameters
                {
                    KeyContainerName = containerName
                };

                using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize, cspParameters))
                {
                    rsaCsp.PersistKeyInCsp = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }
        }

        /// <summary>
        /// Get RSA Key Pair.
        /// Gets key pair from a container with the name provided. If no container is found a new key pair and container are created.
        /// </summary>
        /// <param name="containerName">The key pair's container name.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <param name="includePrivateParameters">A bool indicating whether to return the private key along with the public key. Default is false.</param>
        /// <returns>Returns the key pair as RSAParameters.</returns>
        public static RSAParameters GetStoredRsaKeyPair(string containerName, int keyBitSize = 2048, bool includePrivateParameters = false)
        {
            var rsaParameters = new RSAParameters();

            try
            {
                var cspParameters = new CspParameters
                {
                    KeyContainerName = containerName
                };

                using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize, cspParameters))
                {
                    rsaCsp.PersistKeyInCsp = true;
                    rsaParameters = rsaCsp.ExportParameters(includePrivateParameters);
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            // Return keys.
            return rsaParameters;
        }

        /// <summary>
        /// Delete Stored RSA Key Pair.
        /// Finds and deletes the container, containing keys included, with the name provided.
        /// </summary>
        /// <param name="containerName">The key pair's container name.</param>
        public static void DeleteStoredRsaKeyPair(string containerName)
        {
            try
            {
                var cspParameters = new CspParameters
                {
                    KeyContainerName = containerName
                };

                using (var rsaCsp = new RSACryptoServiceProvider(1024, cspParameters))
                {
                    rsaCsp.PersistKeyInCsp = false;
                    rsaCsp.Clear();
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }
        }
        
        /// <summary>
        /// Encrypt.
        /// Encrypts the provided byte array using the key pair with the provided container name.
        /// If no key pair is found a new key pair
        /// </summary>
        /// <param name="containerName">The storage container name used to identify the key pair.</param>
        /// <param name="plainBytes">The byte array to encrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns an encrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static byte[] Encrypt(string containerName, byte[] plainBytes, int keyBitSize = 2048)
        {
            byte[] encryptedByteArray = null;

            try
            {
                using (var rsaCsp = new RSACryptoServiceProvider())
                {
                    rsaCsp.ImportParameters(GetStoredRsaKeyPair(containerName, keyBitSize, true));
                    encryptedByteArray = rsaCsp.Encrypt(plainBytes, false);
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return encryptedByteArray;
        }

        /// <summary>
        /// Encrypt Async.
        /// Encrypts the provided byte array using the key pair with the provided container name.
        /// If no key pair is found a new key pair
        /// </summary>
        /// <param name="containerName">The storage container name used to identify the key pair.</param>
        /// <param name="plainBytes">The byte array to encrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns an encrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static async Task<byte[]> EncryptAsync(string containerName, byte[] plainBytes, int keyBitSize = 2048)
        {
            byte[] encryptedByteArray = null;

            try
            {
                encryptedByteArray = await Task.Run(() =>
                {
                    using (var rsaCsp = new RSACryptoServiceProvider())
                    {
                        rsaCsp.ImportParameters(GetStoredRsaKeyPair(containerName, keyBitSize, true));
                        return rsaCsp.Encrypt(plainBytes, false);
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return encryptedByteArray;
        }

        /// <summary> 
        /// Encrypt.
        /// Encrypts the byte array using the provided RSAParameters with the provided key size.
        /// </summary>
        /// <param name="keyParameters">An RSAParameters object containg key pair information.</param>
        /// <param name="plainBytes">The byte array to encrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns an encrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static byte[] Encrypt(RSAParameters keyParameters, byte[] plainBytes, int keyBitSize = 2048)
        { 
            byte[] encryptedByteArray = null;

            try
            {
                using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize))
                {
                    rsaCsp.ImportParameters(keyParameters);
                    encryptedByteArray = rsaCsp.Encrypt(plainBytes, false);
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return encryptedByteArray;
        }

        /// <summary> 
        /// Encrypt Async.
        /// Encrypts the byte array using the provided RSAParameters with the provided key size.
        /// </summary>
        /// <param name="keyParameters">An RSAParameters object containg key pair information.</param>
        /// <param name="plainBytes">The byte array to encrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns an encrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static async Task<byte[]> EncryptAsync(RSAParameters keyParameters, byte[] plainBytes, int keyBitSize = 2048)
        {
            byte[] encryptedByteArray = null;

            try
            {
                encryptedByteArray = await Task.Run(() =>
                {
                    using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize))
                    {
                        rsaCsp.ImportParameters(keyParameters);
                        return rsaCsp.Encrypt(plainBytes, false);
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return encryptedByteArray;
        }

        /// <summary>
        /// Decrypt.
        /// Decrypts the byte array using the provided RSAParameters with the provided key size.
        /// </summary>
        /// <param name="containerName">The storage container name used to identify the key pair.</param>
        /// <param name="cipherBytes">The byte array to decrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns a decrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static byte[] Decrypt(string containerName, byte[] cipherBytes, int keyBitSize = 2048)
        {
            byte[] decryptedByteArray = null;

            try
            {
                using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize))
                {
                    rsaCsp.ImportParameters(GetStoredRsaKeyPair(containerName, keyBitSize, true));
                    decryptedByteArray = rsaCsp.Decrypt(cipherBytes, false);
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return decryptedByteArray;
        }

        /// <summary>
        /// Decrypt Async.
        /// Decrypts the byte array using the provided RSAParameters with the provided key size.
        /// </summary>
        /// <param name="containerName">The storage container name used to identify the key pair.</param>
        /// <param name="cipherBytes">The byte array to decrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns a decrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static async Task<byte[]> DecryptAsync(string containerName, byte[] cipherBytes, int keyBitSize = 2048)
        {
            byte[] decryptedByteArray = null;

            try
            {
                decryptedByteArray = await Task.Run(() =>
                {
                    using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize))
                    {
                        rsaCsp.ImportParameters(GetStoredRsaKeyPair(containerName, keyBitSize, true));
                        return rsaCsp.Decrypt(cipherBytes, false);
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return decryptedByteArray;
        }

        /// <summary>
        /// Decrypt.
        /// </summary>
        /// <param name="keyParameters">An RSAParameters object containg key pair information.</param>
        /// <param name="cipherBytes">The byte array to decrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns a decrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static byte[] Decrypt(RSAParameters keyParameters, byte[] cipherBytes, int keyBitSize = 2048)
        {
            byte[] decryptedByteArray = null;

            try
            {
                using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize))
                {
                    rsaCsp.ImportParameters(keyParameters);
                    decryptedByteArray = rsaCsp.Decrypt(cipherBytes, false);
                }
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return decryptedByteArray;
        }

        /// <summary>
        /// Decrypt Async.
        /// </summary>
        /// <param name="keyParameters">An RSAParameters object containg key pair information.</param>
        /// <param name="cipherBytes">The byte array to decrypt.</param>
        /// <param name="keyBitSize">The key bit size. Default is 2048 bits.</param>
        /// <returns>Returns a decrypted byte array or null if an error occured. Check logs for any errors.</returns>
        public static async Task<byte[]> DecryptAsync(RSAParameters keyParameters, byte[] cipherBytes, int keyBitSize = 2048)
        {
            byte[] decryptedByteArray = null;

            try
            {
                decryptedByteArray = await Task.Run(() =>
                {
                    using (var rsaCsp = new RSACryptoServiceProvider(keyBitSize))
                    {
                        rsaCsp.ImportParameters(keyParameters);
                        return rsaCsp.Decrypt(cipherBytes, false);
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is CryptographicException ||
                    ex is ArgumentNullException)
                {
                    Logger.Write(LogTypes.Error, ex.Message, useInlineLayout: false);
                }
            }

            return decryptedByteArray;
        }
    }
}