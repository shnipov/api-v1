using System.Security.Cryptography;

namespace TodoApi
{
    public class RSAKeyUtils
    {
        public static RSAParameters GetRandomKey()
        {
            //X509Certificate2 cert = new X509Certificate2(new byte[100], null);
            //RSA pkey = cert.GetRSAPrivateKey();
            //RSAParameters p = pkey.ExportParameters(true);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    return rsa.ExportParameters(true);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}