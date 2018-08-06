using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace Orders.Utility
{
    public static class JwtTokenHandler
    {
        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKeyResolver = (t, st, i, p)
                    => new List<SecurityKey> {new X509SecurityKey(GetCertificate())}
            };
        }

        private static X509Certificate2 GetCertificate()
        {
            return new X509Certificate2(GetCertificatePath());
        }

        private static string GetCertificatePath()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var directoryName = Path.GetDirectoryName(path);

            return Path.Combine(directoryName,
                "9707175a-ee4e-4619-8dc5-d42ac9999331_fc22dd03-c278-45d6-b0dc-36c5472f0547.pem");
        }
    }
}