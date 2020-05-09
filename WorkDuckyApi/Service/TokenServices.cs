using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.Configuration;

namespace WorkDuckyAPI.Service
{
    public class TokenServices
    {

        private readonly JwtConfiguration jwtConfiguration;
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public TokenServices(IConfiguration config, ILogger logger)
        {

            var jwtconf = new JwtConfiguration();
            config.GetSection("jwt").Bind(jwtconf);
            jwtConfiguration = jwtconf;
            this.logger = logger;
            this.config = config;
        }

        public Token GenerateToken(string userId)
        {
            var expiration = DateTimeOffset.UtcNow.AddHours(jwtConfiguration.ExpirationTimeInHours);
            var tokenPayload = new TokenPayload()
            {
                ExpirationDate = expiration.ToUnixTimeMilliseconds(),
                Uid = userId
            };
            var propertyArray = typeof(TokenPayload).GetProperties();
            var payload = new Dictionary<string, object>();

            foreach (var property in propertyArray)
            {
                payload.Add(property.Name, property.GetValue(tokenPayload));
            }

            var token = new Token()
            {
                ExpirationDate = expiration,
                jwt = new JwtEncoder(new HMACSHA256Algorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder())
                        .Encode(payload, jwtConfiguration.Secret)
            };
            return token;
        }

        public async Task<User> DecodeJwt(string jwt)
        {
            try
            {
                var json = new JwtDecoder(new JsonNetSerializer(), new JwtBase64UrlEncoder()).Decode(jwt, jwtConfiguration.Secret, false);

                var jwtPayload = new JsonNetSerializer().Deserialize<TokenPayload>(json);

                var accountServices = new AccountServices(config, logger);

                return await accountServices.GetUser(jwtPayload.Uid);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }
            return null;
        }

        public string ValidateMailToken(string token)
        {
            try
            {
                var clearToken = DecryptAes(token);
                var mail = clearToken.Split(',')[0];
                var timeStamp = clearToken.Split(',')[1];

                return mail;
            }
            catch (Exception ex)
            {
                logger.LogWarning("Error Validating Account", ex);
                throw ex;
            }

        }

        /// <summary>
        /// Generates an AES Encrypted String of the email Address and the time of generation 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GenerateMailToken(string email)
        {
            var timeStamp = DateTime.UtcNow.ToString();
            var cypherText = EncryptAes(email + "," + timeStamp);
            return cypherText;
        }

        public string EncryptAes(string plainText)
        {

            byte[] encrypted;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(jwtConfiguration.Secret);
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor();

                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string DecryptAes(string cipherText)
        {
            string plaintext = null;
            var cypherBytes = Convert.FromBase64String(cipherText);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(jwtConfiguration.Secret);
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor();

                using (MemoryStream msDecrypt = new MemoryStream(cypherBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }

            }

            return plaintext;
        }
    }
}