using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardEncoder
{
    class MessageDefine
    {
        public static string readCard = "AA 01 25 8E BB";

        public static string loadPassword(string password) {
            string loadPassword = "AA 01 26 00 " + password + " 8D BB";
            return updateCheckByte(loadPassword);
        }

        public static string modifyPassword(string block,string oldPassword, string newPassword) {
            string modify = "AA 01 22 " + block + " " + oldPassword + " " + newPassword + "81 BB";
            return updateCheckByte(modify);
        }

        private static string updateCheckByte(string s) {
            byte[] bytes = ByteUtil.hexStrToBytes(s);
            ByteUtil.updateCheckByte(bytes, 0, bytes.Length - 2, bytes.Length - 2);
            return ByteUtil.bytesToHexStr(bytes," ");
        }
    }
}
