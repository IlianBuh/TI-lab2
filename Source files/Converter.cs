using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TI2
{
    static class Converter
    {
        static public byte[] ReadBytesFromFile(string fileName) {
            return File.ReadAllBytes(fileName);
        }

        static public string BinString(byte[] bytes)
        {
            StringBuilder res = new(bytes.Length * 8);

            foreach (var Byte in bytes)
            {
                res.Append(ByteToBinString(Byte));
            }

            return res.ToString();

        }
        static public string BinString(string fileName)
        {
            return BinString(ReadBytesFromFile(fileName));
        }
        static private string ByteToBinString(byte Byte) {
            StringBuilder res = new(8);

            for (int i = 7; i >= 0; i--) {
                res.Append((Byte & 1 << i) == 0 ? '0' : '1');
            }

            return res.ToString();
        }


    }
}
