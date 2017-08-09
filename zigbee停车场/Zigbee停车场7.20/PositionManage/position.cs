using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PositionManage
{
    class position
    {
        public static int pd(UDPData data)
        {
            string no = "0";

            byte[] c1, c2, c3, c4, c5, c6,dis;
            c1 = new byte[5] { 45, 0, 40, 43,0 };
            c2 = new byte[5] { 43, 48, 41, 0, 0 };
            c3 = new byte[5] { 42, 45, 0,47, 0 };
            c4 = new byte[5] { 43, 56, 0, 43, 0 };
            c5 = new byte[5] { 57, 52, 31,0, 0 };
            c6 = new byte[5] { 55, 43, 0, 40, 0 };
            dis = new byte[5];
            for (int i = 0; i < 3; i++)
            {

                no = data.Loc[i].ReferNo;
                switch (no)
                {
                    case "0":
                        break;
                    case "02FA":
                        dis[0] = data.Loc[i].Destance;
                        break;
                    case "02FD":
                        dis[1] = data.Loc[i].Destance;
                        break;
                    case "02F7":
                        dis[2] = data.Loc[i].Destance;
                        break;
                    case "02CD":
                        dis[3] = data.Loc[i].Destance;
                        break;
                    case "02D6":
                        dis[4] = data.Loc[i].Destance;
                        break;
                    default:
                        break;
                }
            }
            int a = 0;
            int[] q;
            q = new int[6] { 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; i++)
            {
                a = (c1[i] - dis[i]) * (c1[i] - dis[i]);
                q[0] = q[0] + a;
            }
            for (int i = 0; i < 5; i++)
            {
                a = (c2[i] - dis[i]) * (c2[i] - dis[i]);
                q[1] = q[1] + a;
            }
            for (int i = 0; i < 5; i++)
            {
                a = (c3[i] - dis[i]) * (c3[i] - dis[i]);
                q[2] = q[2] + a;
            }
            for (int i = 0; i < 5; i++)
            {
                a = (c4[i] - dis[i]) * (c4[i] - dis[i]);
                q[3] = q[3] + a;
            }
            for (int i = 0; i < 5; i++)
            {
                a = (c5[i] - dis[i]) * (c5[i] - dis[i]);
                q[4] = q[4] + a;
            }
            for (int i = 0; i < 5; i++)
            {
                a = (c6[i] - dis[i]) * (c6[i] - dis[i]);
                q[5] = q[5] + a;
            }
            int min = int.MaxValue;
            int m = 0;
            for (int i = 0; i < q.Length; i++)
            {
                if (min > q[i])
                {
                    min = q[i];
                    m = i;
                }
            }
            return m + 1;


        }
    }
}
