using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace CardEncoder
{
    class MessageTransport
    {
        private SerialPort port = new SerialPort();

        public bool isOpen() {
            if (port == null) {
                return false;
            }
            return port.IsOpen;
        }

        public void close() {
            if (port != null)
            {
                port.Close();
            }
        }

        public void open(string com) {
            port.PortName = com;
            port.Open();
        }

        private byte[] write(byte[] writeBytes, int returnSize, int returnTimeOut) {
            if (isOpen() == false)
            {
                throw new Exception("串口未打开");
            }

            port.DiscardInBuffer();
            port.DiscardOutBuffer();
            port.Write(writeBytes,0,writeBytes.Length);

            byte[] result = new byte[returnSize];
            Thread.Sleep(200);
            port.ReadTimeout = returnTimeOut;
            port.Read(result, 0, result.Length);
            return result;
        }

        public string write(string writeStr, int returnSize, int returnTimeOut) {
            Console.WriteLine("串口：" + port.PortName);
            Console.WriteLine("发送内容：" + writeStr + " 预定返回长度：" + returnSize + " 预定返回超时：" + returnTimeOut);
            byte[] writeBytes = ByteUtil.hexStrToBytes(writeStr);
            byte[] returnBytes = write(writeBytes, returnSize, returnTimeOut);
            string returnStr = ByteUtil.bytesToHexStr(returnBytes," ");
            Console.WriteLine("返回内容：" + returnStr);
            return returnStr;
        }



        internal string[] getComPortItems()
        {
            return SerialPort.GetPortNames();
        }
    }
}
