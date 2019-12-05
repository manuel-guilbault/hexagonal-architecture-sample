using System;
using System.Text;
using HexagonalArchitectureSample.UseCases;

namespace HexagonalArchitectureSample.Infrastructure
{
    internal static class VersionTagConverter
    {
        public static VersionTag FromBytes(byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var @byte in bytes)
            {
                hex.AppendFormat("{0:x2}", @byte);
            }

            return new VersionTag(hex.ToString());
        }

        public static byte[] ToBytes(VersionTag versionTag)
        {
            var hex = versionTag.ToString();
            var numberOfChars = hex.Length;
            var bytes = new byte[numberOfChars / 2];
            for (var i = 0; i < numberOfChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
