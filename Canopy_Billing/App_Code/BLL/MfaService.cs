using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Vendor_Portal.App_Code.BLL
{
    public static class MfaService
    {
        private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private const int SecretBytes = 20;
        private const int TimeStepSeconds = 30;
        private const int CodeDigits = 6;
        private static readonly string[] MachineKeyPurposes = { "Canopy_Billing", "MfaSecret", "v1" };

        public static string GenerateSecret()
        {
            byte[] secret = new byte[SecretBytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secret);
            }

            return ToBase32(secret);
        }

        public static string FormatSecretForDisplay(string secret)
        {
            string normalized = NormalizeSecret(secret);
            return string.Join(" ", Enumerable.Range(0, (normalized.Length + 3) / 4)
                .Select(i => normalized.Substring(i * 4, Math.Min(4, normalized.Length - (i * 4)))));
        }

        public static string BuildOtpAuthUri(string issuer, string accountName, string secret)
        {
            string normalizedIssuer = string.IsNullOrWhiteSpace(issuer) ? "Canopy Billing" : issuer.Trim();
            string normalizedAccount = string.IsNullOrWhiteSpace(accountName) ? "user" : accountName.Trim();
            string label = HttpUtility.UrlEncode(normalizedIssuer + ":" + normalizedAccount);
            string queryIssuer = HttpUtility.UrlEncode(normalizedIssuer);

            return string.Format(
                "otpauth://totp/{0}?secret={1}&issuer={2}&algorithm=SHA1&digits={3}&period={4}",
                label,
                NormalizeSecret(secret),
                queryIssuer,
                CodeDigits,
                TimeStepSeconds);
        }

        public static string ProtectSecret(string secret)
        {
            byte[] protectedBytes = MachineKey.Protect(Encoding.UTF8.GetBytes(NormalizeSecret(secret)), MachineKeyPurposes);
            return Convert.ToBase64String(protectedBytes);
        }

        public static string UnprotectSecret(string protectedSecret)
        {
            if (string.IsNullOrWhiteSpace(protectedSecret))
            {
                return string.Empty;
            }

            byte[] protectedBytes = Convert.FromBase64String(protectedSecret);
            byte[] secretBytes = MachineKey.Unprotect(protectedBytes, MachineKeyPurposes);
            return secretBytes == null ? string.Empty : Encoding.UTF8.GetString(secretBytes);
        }

        public static bool ValidateCode(string secret, string code)
        {
            string normalizedCode = NormalizeCode(code);
            if (normalizedCode.Length != CodeDigits)
            {
                return false;
            }

            long currentStep = GetCurrentTimeStepNumber();
            for (int drift = -1; drift <= 1; drift++)
            {
                string expectedCode = GenerateCode(secret, currentStep + drift);
                if (FixedTimeEquals(expectedCode, normalizedCode))
                {
                    return true;
                }
            }

            return false;
        }

        private static string GenerateCode(string secret, long timeStep)
        {
            byte[] key = FromBase32(secret);
            byte[] counter = BitConverter.GetBytes(timeStep);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            using (HMACSHA1 hmac = new HMACSHA1(key))
            {
                byte[] hash = hmac.ComputeHash(counter);
                int offset = hash[hash.Length - 1] & 0x0f;
                int binary =
                    ((hash[offset] & 0x7f) << 24) |
                    ((hash[offset + 1] & 0xff) << 16) |
                    ((hash[offset + 2] & 0xff) << 8) |
                    (hash[offset + 3] & 0xff);

                int otp = binary % (int)Math.Pow(10, CodeDigits);
                return otp.ToString(new string('0', CodeDigits));
            }
        }

        private static long GetCurrentTimeStepNumber()
        {
            long unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTime / TimeStepSeconds;
        }

        private static string ToBase32(byte[] bytes)
        {
            StringBuilder result = new StringBuilder();
            int buffer = bytes[0];
            int next = 1;
            int bitsLeft = 8;

            while (bitsLeft > 0 || next < bytes.Length)
            {
                if (bitsLeft < 5)
                {
                    if (next < bytes.Length)
                    {
                        buffer <<= 8;
                        buffer |= bytes[next++] & 0xff;
                        bitsLeft += 8;
                    }
                    else
                    {
                        int pad = 5 - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }

                int index = 0x1f & (buffer >> (bitsLeft - 5));
                bitsLeft -= 5;
                result.Append(Base32Alphabet[index]);
            }

            return result.ToString();
        }

        private static byte[] FromBase32(string input)
        {
            string normalized = NormalizeSecret(input);
            List<byte> bytes = new List<byte>();
            int bitBuffer = 0;
            int bitsInBuffer = 0;

            foreach (char c in normalized)
            {
                int value = Base32Alphabet.IndexOf(c);
                if (value < 0)
                {
                    throw new FormatException("Invalid Base32 secret.");
                }

                bitBuffer = (bitBuffer << 5) | value;
                bitsInBuffer += 5;

                if (bitsInBuffer >= 8)
                {
                    bytes.Add((byte)(bitBuffer >> (bitsInBuffer - 8)));
                    bitsInBuffer -= 8;
                    bitBuffer &= (1 << bitsInBuffer) - 1;
                }
            }

            return bytes.ToArray();
        }

        private static string NormalizeSecret(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
            {
                return string.Empty;
            }

            return new string(secret
                .Where(c => !char.IsWhiteSpace(c) && c != '-')
                .Select(char.ToUpperInvariant)
                .ToArray());
        }

        private static string NormalizeCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return string.Empty;
            }

            return new string(code.Where(char.IsDigit).ToArray());
        }

        private static bool FixedTimeEquals(string left, string right)
        {
            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < left.Length; i++)
            {
                diff |= left[i] ^ right[i];
            }

            return diff == 0;
        }
    }
}
