using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TI2
{
    static class Encoder
    {
        
        static public (byte[] bytesRes, string binStr, string keyStr) Encode(byte[] src, LFSR register) {
            
            int len = src.Length;
            StringBuilder res = new(len * 8);
            StringBuilder key = new(len * 8);

            for (int i = 0;i < len;i ++) {

                var ret = XOR(src, register, i);
                res.Append(ret.Item1);
                key.Append(ret.Item2);

            }

            return ( src, res.ToString(), key.ToString() );
        }

        static private (StringBuilder, StringBuilder) XOR(byte[] src, LFSR reg, int srcIndex) {
            StringBuilder res = new(8);
            StringBuilder key = new(8);

            for (int i = 7; i >= 0; i--)
            {
                byte regVal = reg.Val;

                src[srcIndex] ^= (byte)(regVal << i);
                key.Append((char)(regVal+'0'));
                res.Append((src[srcIndex] & (1 << i)) == 0 ? '0': '1');
            }

            return (res, key);
        }
    }
}
