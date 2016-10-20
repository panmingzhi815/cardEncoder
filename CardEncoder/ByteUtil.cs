using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardEncoder
{
    class ByteUtil
    {
        public static string bytesToHexStr(byte[] returnBytes,string split)
        {
            string byteStr = string.Empty;
            if (returnBytes != null || returnBytes.Length > 0)
            {
                foreach (var item in returnBytes)
                {
                    byteStr =byteStr + string.Format("{0:X2}", item) + split;
                }
            }
            return byteStr;
        }

        public static byte[] hexStrToBytes(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes; 
        }

        internal static void updateCheckByte(byte[] bytes, int start_position, int length, int checkPosition)
        {
            int check = bytes[start_position] ^ bytes[start_position + 1];
            for (int i = start_position + 2; i < start_position + length; i++)
            {
                check ^= bytes[i];
            }
            bytes[checkPosition] = (Byte)check;
        }
    }
}
