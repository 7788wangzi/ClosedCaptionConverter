using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosedCaptionConverter.Model
{
    public class TimeFormatter
    {
        ///
        // type = t1 (01:11:12.001)
        // type = t2 (01:11:12,001)
        public static string ToHHMMSS(double value, string type)
        {
            int hh = (int)value / (60 * 60 * 1000);
            double remain = value % (60 * 60 * 1000);
            int mm = (int)remain / (60 * 1000);
            remain = remain % (60 * 1000);
            int ss = (int)remain / (1000);
            remain = remain % (1000);
            double mm2 = remain;

            if (type == "t1")
                return string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hh, mm, ss, mm2);
            else if (type == "t2")
            {
                return string.Format("{0:00}:{1:00}:{2:00},{3:000}", hh, mm, ss, mm2);
            }
            else
            {
                return string.Format("{0:00}:{1:00}:{2:00}.{3:000}", hh, mm, ss, mm2);
            }
        }

        /// <summary>
        /// Support format
        /// 00:00:05.530
        /// 00:00:05,530
        /// 00:05.530
        /// 00:05,530
        /// 00:00:05
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(string value)
        {
            double accumateValue = 0;

            // handle 00:00:26:08
            string[] spParts = value.Split(new char[] { ':' });
            if (spParts.Length == 4)
            {
                for (int i = 0; i < spParts.Length; i++)
                {
                    if (i == 0)
                    {
                        accumateValue += double.Parse(spParts[i]) * 60 * 60 * 1000;
                    }
                    else if (i == 1)
                    {
                        accumateValue += double.Parse(spParts[i]) * 60 * 1000;
                    }
                    else if (i == 2)
                    {
                        accumateValue += double.Parse(spParts[i]) * 1000;
                    }
                    else if (i == 3)
                    {
                        accumateValue += double.Parse(spParts[i]);
                    }
                }

                return accumateValue;
            }

            // handle 00:00:03.050 or 00:00:03,050
            string[] parts = value.Split(new char[] { '.', ',' });

            string[] values = parts[0].Split(new char[] { ':' });
            if (values.Length == 3)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (i == 0)
                    {
                        accumateValue += double.Parse(values[i]) * 60 * 60 * 1000;
                    }
                    else if (i == 1)
                    {
                        accumateValue += double.Parse(values[i]) * 60 * 1000;
                    }
                    else if (i == 2)
                    {
                        accumateValue += double.Parse(values[i]) * 1000;
                    }
                }
            }
            else if (values.Length == 2)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (i == 0)
                    {
                        accumateValue += double.Parse(values[i]) * 60 * 1000;
                    }
                    else if (i == 1)
                    {
                        accumateValue += double.Parse(values[i]) * 1000;
                    }
                }
            }

            if (parts.Length == 2)
            {
                accumateValue += double.Parse(parts[1]);
            }
            return accumateValue;
        }
    }
}
