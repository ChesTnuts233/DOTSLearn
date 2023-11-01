using System;
using UnityEngine;
using static UnityEngine.Mathf;
using Random = UnityEngine.Random;

namespace KooFrame.FrameTools
{
    public class KooMathTool
    {
        #region 数值常量

        /// <summary>
        /// 十万
        /// </summary>
        private const int Hundred_Thousand = 100000;

        /// <summary>
        /// 亿
        /// </summary>
        private const int Hundred_Million = 100000000;

        /// <summary>
        /// π (圆周率,一般用系统的 这个精度高一点点)
        /// </summary>
        public const float Pi = 3.1415926535f;

        /// <summary>
        /// 自然对数的底数 一般用系统的 这个精度高一点点
        /// </summary>
        public const float Euler = 2.7182818284f;

        /// <summary>
        /// 角度转弧度的乘数
        /// </summary>
        public const float DegreesToRadians = Pi / 180.0f;

        /// <summary>
        /// 弧度转角度的乘数
        /// </summary>
        public const float RadiansToDegrees = 180.0f / Pi;

        /// <summary>
        /// 黄金比例
        /// </summary>
        public const float GoldenRatio = 1.61803f;

        /// <summary>
        /// 一小时的秒数
        /// </summary>
        public const int SecondsPerHour = 3600;

        /// <summary>
        /// 一天的秒数
        /// </summary>
        public const int SecondsPerDay = 86400;

        /// <summary>
        /// 一周的秒数
        /// </summary>
        public const int SecondsPerWeek = 604800;

        /// <summary>
        /// 一年的秒数（平均）
        /// </summary>
        public const int SecondsPerYear = 31536000;

        /// <summary>
        /// 常见的重力加速度（地球）
        /// </summary>
        public const float StandardGravity = 9.80665f; // m/s²

        /// <summary>
        /// 速度光在真空中的常数
        /// </summary>
        public const float SpeedOfLight = 299792458.0f; // m/s

        #endregion

        #region 获得数值的个,十,百,千位数值

        /// <summary>
        /// 返回一个整数的指定位数的数字
        /// </summary>
        /// <param name="number">整数</param>
        /// <param name="index">索引 1为个位 2为十位...</param>
        /// <returns></returns>
        public static int ReturnSplitOrderUnit(int number, int index)
        {
            //需要除以的小数
            float temp = Pow(10, 1 - index);
            Debug.Log(temp);
            //算出的第几位的 位数
            return FloorToInt(Abs(number) * temp) % 10;
        }


        /// <summary>
        /// 返回一个整数的个位数。
        /// </summary>
        /// <param name="number">要获取个位数的整数。</param>
        /// <returns>输入整数的个位数。</returns>
        public static int ReturnSplitUnit(int number)
        {
            return Abs(number) % 10;
        }

        /// <summary>
        /// 返回一个整数的十位数。
        /// </summary>
        /// <param name="number">要获取十位数的整数。</param>
        /// <returns>输入整数的十位数。</returns>
        public static int ReturnSplitTen(int number)
        {
            return FloorToInt(Abs(number) * 0.1f) % 10;
        }

        /// <summary>
        /// 返回一个整数的百位数。
        /// </summary>
        /// <param name="number">要获取百位数的整数。</param>
        /// <returns>输入整数的百位数。</returns>
        public static int ReturnSplitHundred(int number)
        {
            return FloorToInt(Abs(number) * 0.01f) % 10;
        }

        /// <summary>
        /// 返回一个整数的千位数。
        /// </summary>
        /// <param name="number">要获取千位数的整数。</param>
        /// <returns>输入整数的千位数。</returns>
        public static int ReturnSplitThousand(int number)
        {
            return FloorToInt(Abs(number) * 0.001f) % 10;
        }

        /// <summary>
        /// 返回一个整数的个位数和十位数。
        /// </summary>
        /// <param name="number">要获取个位数和十位数的整数。</param>
        /// <param name="unit">输出的个位数。</param>
        /// <param name="ten">输出的十位数。</param>
        public static void ReturnSplitToTen(int number, out int unit, out int ten)
        {
            number = Abs(number);
            unit = number % 10;
            //Mathf.FloorToInt 向下取整
            ten = FloorToInt(number * 0.1f) % 10;
        }

        /// <summary>
        /// 返回一个整数的个位数、十位数和百位数。
        /// </summary>
        /// <param name="number">要获取个位数、十位数和百位数的整数。</param>
        /// <param name="unit">输出的个位数。</param>
        /// <param name="ten">输出的十位数。</param>
        /// <param name="hundred">输出的百位数。</param>
        public static void ReturnSplitToHundred(int number, out int unit, out int ten, out int hundred)
        {
            number = Abs(number);
            unit = number % 10;
            ten = FloorToInt(number * 0.1f) % 10;
            hundred = FloorToInt(number * 0.01f) % 10;
        }

        /// <summary>
        /// 返回一个整数的个位数、十位数、百位数和千位数。
        /// </summary>
        /// <param name="number">要获取个位数、十位数、百位数和千位数的整数。</param>
        /// <param name="unit">输出的个位数。</param>
        /// <param name="ten">输出的十位数。</param>
        /// <param name="hundred">输出的百位数。</param>
        /// <param name="thousand">输出的千位数。</param>
        public static void ReturnSplitToThousand(int number, out int unit, out int ten, out int hundred,
            out int thousand)
        {
            number = Abs(number);
            unit = number % 10;
            ten = FloorToInt(number * 0.1f) % 10;
            hundred = FloorToInt(number * 0.01f) % 10;
            thousand = FloorToInt(number * 0.001f) % 10;
        }

        #endregion

        #region 数值转换成字符串

        /// <summary>
        /// 将大数值转换为带有对应单位的字符串
        /// </summary>
        /// <param name="number">数值，不一定为整型，且long / int = long / long</param>
        /// <returns></returns>
        public static string BigNumberToUnitString(double number)
        {
            if (number >= Hundred_Million)
            {
                return $"{GetPreciseDecimal((float)(number / Hundred_Million), 2)}亿";
            }

            if (number >= Hundred_Thousand)
            {
                return $"{GetPreciseDecimal((float)(number / Hundred_Thousand), 2) * 10}万";
            }

            return number.ToString();
        }

        /// <summary>
        /// 对应数值保留几位小数,float精度大约为6-9位数字，double精度大约15-17位数字
        /// </summary>
        /// <param name="number">原始数值</param>
        /// <param name="decimalPlaces">保留小数位数</param>
        /// <returns></returns>
        public static float GetPreciseDecimal(float number, int decimalPlaces = 0)
        {
            if (decimalPlaces < 0)
                decimalPlaces = 0;

            int powerNumber = (int)Pow(10, decimalPlaces);
            float tmeporary = number * powerNumber;
            return (float)Math.Round(tmeporary / powerNumber, decimalPlaces);
        }

        /// <summary>
        /// 对应数值保留几位小数,float精度大约为6-9位数字，double精度大约15-17位数字
        /// </summary>
        /// <param name="number">原始数值</param>
        /// <param name="decimalPlaces">保留小数位数</param>
        /// <returns></returns>
        public static double GetPreciseDecimal(double number, int decimalPlaces = 0)
        {
            if (decimalPlaces < 0)
                decimalPlaces = 0;

            int powerNumber = (int)Pow(10, decimalPlaces);
            double tmeporary = number * powerNumber;
            //Math.Round，参数二将双精度浮点值舍入到指定数量的小数位数。
            return Math.Round(tmeporary / powerNumber, decimalPlaces);
        }

        /// <summary>
        /// 数字0-9转换为中文数字
        /// </summary>
        /// <param name="num"></param>
        /// <returns>中文大写数值</returns>
        public static string SingleDigitsNumberToChinese(int num = 0)
        {
            num = Clamp(num, 0, 9);
            //切分出字符数组
            string[] unitAllString = "零,一,二,三,四,五,六,七,八,九".Split(',');
            return unitAllString[num];
        }

        /// <summary>
        /// 数字转换为中文，可以适用任何3位及其以下数值
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string NumberToCNString(int number)
        {
            string numStr = number.ToString();
            string resultStr = "";
            int strLength = numStr.Length;
            //切分出字符数组
            string[] unitAllString = KooStringTool.GetSpliteStringFormat("零,一,二,三,四,五,六,七,八,九");
            string units = "", tens = "", hundreds = "";
            string tenStr = "十";
            string hundredStr = "百";

            for (int i = 1; i <= strLength; i++)
            {
                //Substring第一个参数为从第几个字符索引位置开始截取， 参数二为截取的长度
                int sNum = Convert.ToInt32(numStr.Substring(i - 1, 1));
                string cnStr = unitAllString[sNum];
                if (i == 1)
                {
                    units = cnStr;
                    resultStr = cnStr;
                }
                else if (i == 2)
                {
                    tens = cnStr;
                    //判断十位是否是0
                    if (tens == unitAllString[0])
                    {
                        if (units == unitAllString[1])
                            resultStr = tenStr;
                        else
                            //例如二十，三十
                            resultStr = units + tenStr;
                    }
                    else
                    {
                        if (units == unitAllString[1])
                            resultStr = tenStr + tens;
                        else
                            resultStr = units + tenStr + tens;
                    }
                }
                else if (i == 3)
                {
                    hundreds = cnStr;
                    //判断百位是否是0
                    if (hundreds == unitAllString[0])
                    {
                        if (tens.Equals(unitAllString[0]))
                            resultStr = units + hundredStr;
                        else
                            //例如一百一，二百一
                            resultStr = units + hundredStr + tens;
                    }
                    else if (tens == unitAllString[0])
                        resultStr = units + hundredStr + tens + hundreds;
                    else
                        resultStr = units + hundredStr + tens + tenStr + hundreds;
                }
            }

            return resultStr;
        }

        /// <summary>
        /// 数值转成英文序号格式（小于100的数值）
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NumberToSequence(long num = 1)
        {
            string temporaryStr = "";
            if (num > 100)
                return temporaryStr;
            if (num % 10 == 1)
                temporaryStr = "ST";
            else if (num % 10 == 2)
                temporaryStr = "ND";
            else if (num % 10 == 3)
                temporaryStr = "RD";
            else
                temporaryStr = "TH";
            return num + temporaryStr;
        }

        #endregion

        #region 数学函数

        public delegate float MathFunctionVector2(float x, float t);

        public delegate Vector3 MathFunctionVector3(float u, float v, float t);

        private static MathFunctionVector3[] _functionVector3 =
        {
            Wave,
            MultiWaveVector3,
            Ripple,
            Sphere0,
            Sphere1,
            Torus0,
            Torus1,
            Torus2,
            Torus3,
        };


        private static MathFunctionVector2[] _functions =
        {
            Wave,
            MultiWave,
            Ripple,
        };

        public enum MathFunctionNameVector2
        {
            Wave,
            MultiWave,
            Ripple,
            MultiWaveWithZ,
        };

        public enum MathFunctionNameVector3
        {
            Wave,
            MultiWaveVector3,
            Ripple,
            Sphere0,
            Sphere1,
            Torus0,
            Torus1,
            Torus2,
            Torus3,
        };

        public static MathFunctionVector2 GetFunction(int index)
        {
            return _functions[index];
        }

        public static MathFunctionVector2 GetFunction(MathFunctionNameVector2 name)
        {
            return _functions[(int)name];
        }

        public static MathFunctionVector3 GetFunctionVector3(MathFunctionNameVector3 name)
        {
            return _functionVector3[(int)name];
        }

        public static MathFunctionNameVector3 GetNextMathFunction(MathFunctionNameVector3 name)
        {
            return name < MathFunctionNameVector3.Torus3 ? name + 1 : MathFunctionNameVector3.Wave;
        }


        /// <summary>
        /// 二维 Sin波浪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Wave(float x, float t)
        {
            return Sin(PI * (x + t));
        }

        /// <summary>
        /// 三维 Sin波浪
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Wave(float u, float v, float t)
        {
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (u + v + t));
            p.z = v;
            return p;
        }


        /// <summary>
        /// 二维 Multi Sin波浪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public static float MultiWave(float x, float t)
        {
            return MultiWave(x, t, 0);
        }

        /// <summary>
        /// Multi Sin波浪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float MultiWave(float x, float t, float c)
        {
            float y = Sin(PI * (x + t));
            y += 0.5f * Sin(2f * PI * (x + c * t));
            //虽然除法比乘法需要更多的工作  但类似 1f / 2f 这类常量表达式 已经被编译器简化成了单个数字
            return y * (2f / 3f);
        }

        /// <summary>
        /// 三维 Multi Sin波浪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public static Vector3 MultiWaveVector3(float x, float z, float t)
        {
            return MultiWaveVector3(x, z, t, 0);
        }


        /// <summary>
        /// 三维 Multi Sin波浪
        /// </summary>
        public static Vector3 MultiWaveVector3(float u, float v, float t, float c)
        {
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (u + c * t));
            p.y += 0.5f * Sin(2f * PI * (v + t));
            p.y += Sin(PI * (u + v + 0.25f * t));
            p.y *= 1f / 2.5f;
            p.z = v;
            return p;
        }

        /// <summary>
        /// 三维 Multi 3 波浪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float MultiWaveWithXZ(float x, float z, float t)
        {
            return MultiWaveWithXZ(x, z, t, 0.5f);
        }

        /// <summary>
        /// 三维 Multi 3 波浪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float MultiWaveWithXZ(float x, float z, float t, float c)
        {
            float y = Sin(PI * (x + 0.5f * t));
            y += 0.5f * Sin(2f * PI * (z + t));
            y += Sin(PI * (x + z + 0.25f * t));
            return y * (1f / 2.5f);
        }


        /// <summary>
        /// 涟漪
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Ripple(float x, float t)
        {
            float d = Abs(x);
            float y = Sin(PI * (4f * d - t));
            return y / (1f + 10f * d);
        }

        /// <summary>
        /// 涟漪函数 Vector3
        /// </summary>
        public static Vector3 Ripple(float u, float v, float t)
        {
            float d = Sqrt(u * u + v * v);
            Vector3 p;
            p.x = u;
            p.y = Sin(PI * (4f * d - t));
            p.y /= 1f + 10f * d;
            p.z = v;
            return p;
        }

        /// <summary>
        /// 球体函数
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Sphere0(float u, float v, float t)
        {
            float r = 0.5f + 0.5f * Sin(PI * t);
            float s = r * Cos(0.5f * PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r * Sin(0.5f * PI * v);
            p.z = s * Cos(PI * u);
            return p;
        }


        /// <summary>
        /// 球体函数
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Sphere1(float u, float v, float t)
        {
            float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
            float s = r * Cos(0.5f * PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r * Sin(0.5f * PI * v);
            p.z = s * Cos(PI * u);
            return p;
        }

        /// <summary>
        /// 圆环
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Torus0(float u, float v, float t)
        {
            float r = 1f;
            float s = 0.5f + r * Cos(PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r * Sin(PI * v);
            p.z = s * Cos(PI * u);
            return p;
        }

        /// <summary>
        /// 自相交的纺锤环面。
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Torus1(float u, float v, float t)
        {
            float r = 1f;
            float s = 0.5f + r * Cos(PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r * Sin(PI * v);
            p.z = s * Cos(PI * u);
            return p;
        }

        /// <summary>
        /// 环形圆环
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Torus2(float u, float v, float t)
        {
            float r1 = 0.75f;
            float r2 = 0.25f;
            float s = r1 + r2 * Cos(PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r2 * Sin(PI * v);
            p.z = s * Cos(PI * u);
            return p;
        }

        /// <summary>
        /// 扭曲的圆环
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Torus3(float u, float v, float t)
        {
            float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
            float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
            float s = r1 + r2 * Cos(PI * v);
            Vector3 p;
            p.x = s * Sin(PI * u);
            p.y = r2 * Sin(PI * v);
            p.z = s * Cos(PI * u);
            return p;
        }

        public static Vector3 Morph(
            float u, float v, float t, MathFunctionVector3 from, MathFunctionVector3 to, float progress
        )
        {
            return Vector3.LerpUnclamped(
                from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress)
            );
        }
        
        public static MathFunctionNameVector3 GetRandomFunctionNameOtherThan (MathFunctionNameVector3 name) {
            var choice = (MathFunctionNameVector3)Random.Range(1, _functionVector3.Length);
            return choice == name ? 0 : choice;
        }

        #endregion
    }
}