using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace PackingManagement
{
    class CommonUtils
    {
        public static void ActiveAllCom()
        {
            try
            {
                string[] portlist = SerialPort.GetPortNames();
                foreach (string portName in portlist)
                {
                    SerialPort sp = new SerialPort(portName, 57600, Parity.None, 8, StopBits.One);

                    if (!sp.IsOpen)
                    {
                        sp.Open();
                        sp.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
