using System;
using System.Linq;
using System.Text;

namespace NotHash
{
    class Program
    {
        static void Main(string[] args)
        {
            var hasher = new NotHash();
            var hash = hasher.HashPassword("test111");

            Console.WriteLine(hash);

            var unhasher = new UnHash();
            var password = unhasher.UnHashString(hash);

            Console.WriteLine(password);

            Console.ReadKey();
        }
    }

    class UnHash
    {
        public string UnHashString(string passwordHash)
        {
            var result = new byte[passwordHash.Length / 2];

            for (int i = 0; i < passwordHash.Length; i += 2)
            {
                var v = passwordHash.Substring(i, 2);
                var b = byte.Parse(v, System.Globalization.NumberStyles.HexNumber);
                var r = (byte)~b;
                result[i / 2] = r;
            }

            var rawString = result.ToArray();
            return Encoding.Unicode.GetString(rawString);
        }
    }


    /// <summary>
    /// Not very secure hash mechanism but if you want it, you will get it
    /// </summary>
    class NotHash
    {
        public string HashPassword(string password)
        {
            var bytes = Encoding.Unicode.GetBytes(password);

            var result = bytes
                .Select(HashByte)
                .Select(ToReadableChar)
                .Aggregate(Concat);

            return result;
        }

        private byte HashByte(byte b)
        {
            return (byte)(~b);
        }

        private string ToReadableChar(byte b)
        {
            return b.ToString("X").PadLeft(2, '0');
        }

        private string Concat(string x, string y)
        {
            return x + y;
        }
    }
}
