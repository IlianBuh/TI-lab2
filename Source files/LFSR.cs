using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TI2
{
    class LFSR
    {
        static public int InitLength = 39;
        List<int> bitsToXor = [ InitLength - 39, InitLength - 4 ];
        public int Length { get; }
        private byte[] val { get; set; }
        public byte Val {
            get {
                byte res = val[0];
                this.NextVal();
                return res;
            } 
        }

        public byte[] GetAllBytes() {
            return (byte[])this.val.Clone();
        }
        public LFSR() : this(39) { }
        public LFSR(int length) {
            this.Length = length;
            this.val = new byte[length];
        }
        public LFSR(string val, int length) {
            int res;
            string[] messages = new string[] { "Length of string is greater than register length",
                                               "Length of string is less than register length, thus rest is filled with 1" };
            this.Length = length;
            this.val = new byte[this.Length];
            res = this.SetValue(val);
            if (res > 0) {
                MessageBox.Show(messages[res-1]);
            }

        }
        
        /*
         * return value:
         *  - 0 - length of string is equal to register length
         *  - 1 - length of string is greater than register length
         *  - 2 - length of string is less than register length, thus rest is filled with 1
         */
        public int SetValue(string key) {
            int i = 0;
            int res = 0;

            foreach (var c in key) {
                if ((c | 1) == '1') {
                    if (i == this.Length) {
                        res = 1;
                        break;
                    }
                    this.val[i++] = (byte)(c - '0');
                }
                
            }

            if (i < this.Length) {
                res = 2;
                this.val[i++] = 1;
                for (; i < this.Length; i++) {
                    this.val[i] = 1;
                }
            }
            return res;

        }
        
        private void NextVal() {
            byte lastBit = this.calcLastBit();
            this.shift();
            this.val[this.Length - 1] = lastBit;
        }
        private void shift (){
            int shiftLen = this.Length - 1;
            for (int i = 0; i < shiftLen; i++) {
                this.val[i] = this.val[i + 1];
            }
        }
        private byte calcLastBit() {
            int res = 0;
            foreach (byte ind in this.bitsToXor) {
                res ^= this.val[ind];
            }
            return (byte) res;
        }

        static public string getBinValue(string src, int len) {
            var res = new StringBuilder();

            res = LFSR.clearTrash(src, len);

            return res.ToString();
        }
        static private StringBuilder clearTrash(string src, int len) {
            var res = new StringBuilder();

            foreach (var c in src) { 
                if (c == '0' || c == '1') {
                    res.Append(c);
                }
                if (res.Length == len) {
                    break;
                }
            }

            return res;
        }
        public string ToString() {
            StringBuilder res = new StringBuilder(this.Length);

            foreach (var b in this.val) {
                res.Append(b == 0 ? '0' : '1');
            }

            return res.ToString();
        }
    }
}
