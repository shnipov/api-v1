using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MxDelivery.Data;
using MxDelivery.Models;
using MxDelivery.Services;

namespace MxDelivery
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtBearerAuthentication(GetKeyParameters());

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            RsaSecurityKey key,
            TokenAuthOptions tokenOptions)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            var jwtBearerOptions = new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    //// Basic settings - signing key to validate with, audience and issuer.
                    IssuerSigningKey = key,
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Issuer,

                    // When receiving a token, check that we've signed it.
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    //ValidateSignature = true,

                    // When receiving a token, check that it is still valid.
                    ValidateLifetime = true,

                    // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                    // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                    // machines which should have synchronised time, this can be set to zero. Where external tokens are
                    // used, some leeway here could be useful.
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            };

            app.UseJwtBearerAuthentication(jwtBearerOptions);

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });



            var header = Convert.FromBase64String("eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2IiwidHlwIjoiSldUIn0=");
            var payload = Convert.FromBase64String("eyJuYW1laWQiOiIzNmQyNTZmMy0yYWQ0LTQ5OTEtYWI4Zi02YmI3YTA0MTdlNGQiLCJ1bmlxdWVfbmFtZSI6InNobmlwb3ZAZ21haWwuY29tIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI3YzMxMmQxNy1kMDEyLTRlNGMtOTRhNC0wZTA0NmY3NWU5NDYiLCJuYmYiOjE0NjY2ODAxMTMsImV4cCI6MTQ2NjY4NzMxMywiaWF0IjoxNDY2NjgwMTEzLCJpc3MiOiJNWCIsImF1ZCI6Ik1YIn0=");


            Console.WriteLine(Encoding.UTF8.GetString(header));
            Console.WriteLine(Encoding.UTF8.GetString(payload));
            Console.WriteLine("");
            Console.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"unique_name\":\"d.shnipov@gmail.com\",\"EntityID\":1,\"nbf\":1466627358,\"exp\":1466634558,\"iat\":1466627358,\"iss\":\"MX\",\"aud\":\"MX\"}")));

            var rawSignature = "Wa7sqAEMd27FtuwAgh3Kes_W0w25fgANUsetaMTExcQNmMQKONHIe9TGX-bu4yogALgn9GUL22ykuxQ4RHH4wDqksGhCNTPPVRyYUQU5niQaHiX0JsuVFcnr-4AFFe1dT2dNm6qPSfRKS6yZVtLfUZguVj4Tf71KLZTwx8UAhYDI7hvPsInXTl2Q00BrDTflyFNgM012WaZDqUmuvxq2BwAUM2o4XSVHblBNPP0nNE7JtJ-1I45leqGyoTtFAZ0vW_WQHNN-YKaMl1troRZNVLlC2WDsFLf8neatITcbmtTOqBEd2oigtG07KLSqg55AuKhZbQZ3q8MB61MyeeygYQ";
            var bytes = Encoding.UTF8.GetBytes(header + "." + payload);
            byte[] signature = DecodeBytes(rawSignature);


            string token =
                "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2IiwidHlwIjoiSldUIn0.eyJuYW1laWQiOiIzNmQyNTZmMy0yYWQ0LTQ5OTEtYWI4Zi02YmI3YTA0MTdlNGQiLCJ1bmlxdWVfbmFtZSI6InNobmlwb3ZAZ21haWwuY29tIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI3YzMxMmQxNy1kMDEyLTRlNGMtOTRhNC0wZTA0NmY3NWU5NDYiLCJuYmYiOjE0NjY2ODAxMTMsImV4cCI6MTQ2NjY4NzMxMywiaWF0IjoxNDY2NjgwMTEzLCJpc3MiOiJNWCIsImF1ZCI6Ik1YIn0.Wa7sqAEMd27FtuwAgh3Kes_W0w25fgANUsetaMTExcQNmMQKONHIe9TGX-bu4yogALgn9GUL22ykuxQ4RHH4wDqksGhCNTPPVRyYUQU5niQaHiX0JsuVFcnr-4AFFe1dT2dNm6qPSfRKS6yZVtLfUZguVj4Tf71KLZTwx8UAhYDI7hvPsInXTl2Q00BrDTflyFNgM012WaZDqUmuvxq2BwAUM2o4XSVHblBNPP0nNE7JtJ-1I45leqGyoTtFAZ0vW_WQHNN-YKaMl1troRZNVLlC2WDsFLf8neatITcbmtTOqBEd2oigtG07KLSqg55AuKhZbQZ3q8MB61MyeeygYQ";
                //"eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2IiwidHlwIjoiSldUIn0.eyJ1bmlxdWVfbmFtZSI6ImQuc2huaXBvdkBnbWFpbC5jb20iLCJFbnRpdHlJRCI6MSwibmJmIjoxNDY2NjI3MzU4LCJleHAiOjE0NjY2MzQ1NTgsImlhdCI6MTQ2NjYyNzM1OCwiaXNzIjoiTVgiLCJhdWQiOiJNWCJ9.Wa7sqAEMd27FtuwAgh3Kes_W0w25fgANUsetaMTExcQNmMQKONHIe9TGX-bu4yogALgn9GUL22ykuxQ4RHH4wDqksGhCNTPPVRyYUQU5niQaHiX0JsuVFcnr-4AFFe1dT2dNm6qPSfRKS6yZVtLfUZguVj4Tf71KLZTwx8UAhYDI7hvPsInXTl2Q00BrDTflyFNgM012WaZDqUmuvxq2BwAUM2o4XSVHblBNPP0nNE7JtJ-1I45leqGyoTtFAZ0vW_WQHNN-YKaMl1troRZNVLlC2WDsFLf8neatITcbmtTOqBEd2oigtG07KLSqg55AuKhZbQZ3q8MB61MyeeygYQ";
            var handler = new JwtSecurityTokenHandler();

            SecurityToken secToken;
            var result = handler.ValidateToken(token, jwtBearerOptions.TokenValidationParameters, out secToken);
            Console.WriteLine(result);
        }

        private RSAParameters GetKeyParameters()
        {
            RSAParameters parameters = new RSAParameters();

            IConfigurationSection section = Configuration.GetSection("KeyParameters");
            parameters.D = Convert.FromBase64String(section["D"]);
            parameters.DP = Convert.FromBase64String(section["DP"]);
            parameters.DQ = Convert.FromBase64String(section["DQ"]);
            parameters.Exponent = Convert.FromBase64String(section["Exponent"]);
            parameters.InverseQ = Convert.FromBase64String(section["InverseQ"]);
            parameters.Modulus = Convert.FromBase64String(section["Modulus"]);
            parameters.P = Convert.FromBase64String(section["P"]);
            parameters.Q = Convert.FromBase64String(section["Q"]);

            //using (var rsa = new RSACryptoServiceProvider(2048))
            //{
            //try
            //{
            //RSAParameters parameters = rsa.ExportParameters(true);

            //var stringWriter = new StringWriter();
            //ExportPublicKey(parameters, stringWriter);
            //File.WriteAllText("c:\\tmp\\token_public_key.txt", stringWriter.ToString());

            //rsa.ImportParameters(parameters);

            return parameters;
            //}
            //finally
            //{
            //    rsa.PersistKeyInCsp = false;
            //}
            //}
        }

        private static char base64PadCharacter = '=';
        private static string doubleBase64PadCharacter = "==";
        private static char base64Character62 = '+';
        private static char base64Character63 = '/';
        private static char base64UrlCharacter62 = '-';
        private static char _base64UrlCharacter63 = '_';

        public static byte[] DecodeBytes(string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            str = str.Replace(base64UrlCharacter62, base64Character62);
            str = str.Replace(_base64UrlCharacter63, base64Character63);
            switch (str.Length % 4)
            {
                case 0:
                    return Convert.FromBase64String(str);
                case 2:
                    str += doubleBase64PadCharacter;
                    goto case 0;
                case 3:
                    str += base64PadCharacter.ToString();
                    goto case 0;
                default:
                    throw new Exception();
            }
        }
        private static void ExportPrivateKey(RSACryptoServiceProvider csp, TextWriter outputStream)
        {
            //if (csp.PublicOnly) throw new ArgumentException("CSP does not contain a private key", "csp");
            //var parameters = csp.ExportParameters(true);
            //using (var stream = new MemoryStream())
            //{
            //    var writer = new BinaryWriter(stream);
            //    writer.Write((byte)0x30); // SEQUENCE
            //    using (var innerStream = new MemoryStream())
            //    {
            //        var innerWriter = new BinaryWriter(innerStream);
            //        EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
            //        EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
            //        EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
            //        EncodeIntegerBigEndian(innerWriter, parameters.D);
            //        EncodeIntegerBigEndian(innerWriter, parameters.P);
            //        EncodeIntegerBigEndian(innerWriter, parameters.Q);
            //        EncodeIntegerBigEndian(innerWriter, parameters.DP);
            //        EncodeIntegerBigEndian(innerWriter, parameters.DQ);
            //        EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
            //        var length = (int)innerStream.Length;
            //        EncodeLength(writer, length);
            //        writer.Write(innerStream.GetBuffer(), 0, length);
            //    }

            //    var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
            //    outputStream.WriteLine("-----BEGIN RSA PRIVATE KEY-----");
            //    // Output as Base64 with lines chopped at 64 characters
            //    for (var i = 0; i < base64.Length; i += 64)
            //    {
            //        outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
            //    }
            //    outputStream.WriteLine("-----END RSA PRIVATE KEY-----");
            //}
        }

        private static void ExportPublicKey(RSAParameters parameters, TextWriter outputStream)
        {
            //RSAParameters parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);

                            ArraySegment<byte> paramsStreamArr;
                            paramsStream.TryGetBuffer(out paramsStreamArr);
                            bitStringWriter.Write(paramsStreamArr.ToArray(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);

                        ArraySegment<byte> bitStringStreamArr;
                        bitStringStream.TryGetBuffer(out bitStringStreamArr);
                        innerWriter.Write(bitStringStreamArr.ToArray(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);

                    ArraySegment<byte> innerStreamArr;
                    innerStream.TryGetBuffer(out innerStreamArr);
                    writer.Write(innerStreamArr.ToArray(), 0, length);
                }

                ArraySegment<byte> streamArr;
                stream.TryGetBuffer(out streamArr);
                var base64 = Convert.ToBase64String(streamArr.ToArray(), 0, (int)stream.Length).ToCharArray();
                //outputStream.WriteLine("-----BEGIN PUBLIC KEY-----");
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
                }
                //outputStream.WriteLine("-----END PUBLIC KEY-----");
            }
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }
    }

    public static class JwtBearerAuthenticationExtension
    {
        const string TokenAudience = "MX";
        const string TokenIssuer = "MX";

        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, RSAParameters keyParameters)
        {
            // *** CHANGE THIS FOR PRODUCTION USE ***
            // Here, we're generating a random key to sign tokens - obviously this means
            // that each time the app is started the key will change, and multiple servers 
            // all have different keys. This should be changed to load a key from a file 
            // securely delivered to your application, controlled by configuration.
            //
            // See the RSAKeyUtils.GetKeyParameters method for an examle of loading from
            // a JSON file.
            //RSAParameters keyParams = GetKey();

            // Create the key, and a set of token options to record signing credentials 
            // using that key, along with the other parameters we will need in the 
            // token controlller.
            RsaSecurityKey key = new RsaSecurityKey(keyParameters);
            TokenAuthOptions tokenOptions = new TokenAuthOptions
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
            };

            // Save the token options into an instance so they're accessible to the 
            // controller.
            services.AddSingleton(key);
            services.AddSingleton(tokenOptions);

            return services;
        }
    }

    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
