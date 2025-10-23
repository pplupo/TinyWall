using System;
using System.Net;

namespace pylorak.TinyWall
{
    internal static class NetworkConverter
    {
        /// <summary>
        /// Converts an IP/SubnetMask string (e.g., "198.51.100.0/255.255.255.0")
        /// to an IP/CIDR string (e.g., "198.51.100.0/24").
        /// </summary>
        /// <param name="input">The network string in IP/SubnetMask format.</param>
        /// <returns>The network string in IP/CIDR format.</returns>
        /// <exception cref="ArgumentException">Thrown when the input is invalid.</exception>
        internal static string ConvertToCidr(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input string cannot be null or empty.", nameof(input));

            string[] parts = input.Split('/');
            if (parts.Length != 2)
                throw new ArgumentException("Input string is not in the expected 'IP/SubnetMask' format.", nameof(input));

            string ipAddress = parts[0];
            string subnetMask = parts[1];

            if (!IPAddress.TryParse(ipAddress, out IPAddress? _))
                throw new ArgumentException("Invalid IP address provided.", nameof(input));

            int prefixLength = ConvertMaskToPrefixLength(subnetMask);

            return string.Concat(ipAddress, "/", prefixLength.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        private static int ConvertMaskToPrefixLength(string maskString)
        {
            if (!IPAddress.TryParse(maskString, out IPAddress? maskAddress))
                throw new ArgumentException("Invalid subnet mask format.", maskString);

            byte[] bytes = maskAddress.GetAddressBytes();
            int prefixLength = 0;
            bool inZeroBlock = false;

            foreach (byte b in bytes)
            {
                if (inZeroBlock)
                {
                    if (b != 0)
                        throw new ArgumentException("Invalid subnet mask: not contiguous.", maskString);

                    continue;
                }

                switch (b)
                {
                    case 255:
                        prefixLength += 8;
                        break;
                    case 254:
                        prefixLength += 7;
                        inZeroBlock = true;
                        break;
                    case 252:
                        prefixLength += 6;
                        inZeroBlock = true;
                        break;
                    case 248:
                        prefixLength += 5;
                        inZeroBlock = true;
                        break;
                    case 240:
                        prefixLength += 4;
                        inZeroBlock = true;
                        break;
                    case 224:
                        prefixLength += 3;
                        inZeroBlock = true;
                        break;
                    case 192:
                        prefixLength += 2;
                        inZeroBlock = true;
                        break;
                    case 128:
                        prefixLength += 1;
                        inZeroBlock = true;
                        break;
                    case 0:
                        inZeroBlock = true;
                        break;
                    default:
                        throw new ArgumentException("Invalid subnet mask octet: " + b.ToString(System.Globalization.CultureInfo.InvariantCulture), maskString);
                }
            }

            return prefixLength;
        }
    }
}
