using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using PackingManagement;
using System.IO.Ports;
using ReaderB;

namespace PackingManagement
{
    public class ReaderHandle
    {
        // 端口号
        int Port = -1;

        // 读写器地址
        byte ComAdr = 0xff;


        /*  波特率对应值
            0	9600bps
            1	19200 bps
            2	38400 bps
            5	57600 bps
            6	115200 bps
        */
        byte Baud = 5;

        //波特率 57600 bps
        int lBaud = 57600;

        // 句柄
        int FrmHandle = -1;

        bool isConnect = false;


        private int EraseMaxLen = 50;
        private int WriteMaxLen = 50;
        private int WriteTryMaxCount = 20;

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {

            if (Port > 0)
            {
                // 关闭串口
                StaticClassReaderB.CloseSpecComPort(FrmHandle);
            }

            CommonUtils.ActiveAllCom();

            // 打开串口
            int iRet = StaticClassReaderB.AutoOpenComPort(ref Port, ref ComAdr, Baud, ref FrmHandle);

            // 串口打开成功
            if (iRet == 0)
            {
                return true;
            }
            else
            {
                // 串口打开失败
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool DisConnect()
        {
            // 关闭串口
            int iRet = StaticClassReaderB.CloseSpecComPort(Port);
            //
            if (iRet == 0)
            {
                Port = -1;
                FrmHandle = -1;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置桌面发卡器功率
        /// </summary>
        /// <param name="powerDb">功率对应db值，范围1-18，桌面发卡器一般采用默认值</param>
        /// <returns></returns>
        public bool SetPowerDbm(int powerDb)
        {
            int fCmdRet = -1;
            byte powerDbm = (byte)powerDb;

            // 设置桌面发卡器功率
            fCmdRet = StaticClassReaderB.SetPowerDbm(ref ComAdr, powerDbm, FrmHandle);

            if (fCmdRet == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查询EPC值
        /// </summary>
        /// <returns>返回标签EPC值</returns>
        public string ReadEpc()
        {
            return Inventory_G2((byte)0, (byte)0, (byte)0);
        }



        /// <summary>
        /// 查询标签EPC或TID信息
        /// </summary>
        /// <param name="TIDFlag">TID地址</param>
        /// <param name="AdrTID">以字为单位的TID长度</param>
        /// <param name="LenTID">标志位，0表示查询EPC,1表示查询TID</param>
        /// <returns></returns>
        public string Inventory_G2(byte TIDFlag, byte AdrTID, byte LenTID)
        {
            if (Port == 0)
            {
                return "";
            }
            // 一字节的EPC的长度+ EPC的字节数组表示 
            byte[] EPC = new byte[5000];

            // 返回的EPC的实际字节数
            int Totallen = 0;

            // 查询到的标签数量
            int CardNum = 0;
            int iCount = 0;
            // TID地址
            //byte AdrTID = 0;
            // 以字为单位的TID长度
            //byte LenTID = 8;
            // 标志位，0表示查询EPC,1表示查询TID
            //byte TIDFlag = 1;

            string hexEPC = "";

            while (iCount < 100)
            {
                int fCmdRet = StaticClassReaderB.Inventory_G2(ref ComAdr, AdrTID, LenTID, TIDFlag, EPC, ref Totallen, ref CardNum, FrmHandle);
                if ((fCmdRet == 1) | (fCmdRet == 2) | (fCmdRet == 3) | (fCmdRet == 4) | (fCmdRet == 0xFB))//代表已查找结束
                {
                    byte[] daw = new byte[Totallen];
                    Array.Copy(EPC, daw, Totallen);
                    string temps = ByteArrayToHexString(daw);
                    //
                    if (CardNum > 0)
                    {
                        int EPClen = daw[0];
                        hexEPC = temps.Substring(2, EPClen * 2);

                        break;
                    }

                }
                iCount++;
            }
            return hexEPC;
        }

        /// <summary>
        /// 将16进制EPC字符串写入EPC区
        /// </summary>
        /// <param name="HexEpc">写入EPC的字符串，长度必须为4的倍数，且其最大长度受标签芯片EPC区域存储空间。</param>
        /// <returns></returns>
        public bool WriteEpc(String HexEpc)
        {
            // 返回错误代码
            int ferrorcode = -1;

            int fCmdRet = 0;

            // 标签密码00000000
            byte[] Password = HexStringToByteArray("00000000");

            // 准备写入的EPC值的字节数组表示
            byte[] writeEpc = HexStringToByteArray(HexEpc);

            // EPC的字节数，必须为偶数
            byte WriteEPClen = (byte)writeEpc.Length;

            fCmdRet = StaticClassReaderB.WriteEPC_G2(ref ComAdr, Password, writeEpc, WriteEPClen, ref ferrorcode, FrmHandle);

            if (fCmdRet == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 十六进制字符串转为二进制数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 二进制数组转为十六进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }
    }
}
