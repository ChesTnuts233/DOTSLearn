using System.Text.RegularExpressions;

namespace KooFrame.FrameTools
{
    public static class KooStringTool
    {
        #region 字符串拆分与替换

        /// <summary>
        /// 按照,#+|，将字符串进行拆分
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string[] GetSpliteStringFormat(string inputStr)
        {
            Regex reg = new Regex("[,#+|]");
            return reg.Split(inputStr);
        }

        /// <summary>
        /// 将输入字符串中的空格、换行符、新行、tab字符替换为指定的字符串。
        /// </summary>
        /// <param name="inputStr">要替换的输入字符串。</param>
        /// <param name="replaceStr">要用于替换空格的字符串（默认为空字符串）。</param>
        /// <returns>替换后的字符串。</returns>
        public static string ReplaceSpaceByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"\s");
            return reg.Replace(inputStr, replaceStr);
        }

        /// <summary>
        /// 仅以指定格式替换左右两端的空格，string里的 Trim也可删除左右两端空格
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="replaceStr">要替换成的字符</param>
        /// <returns></returns>
        public static string ReplaceTrimSpaceByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"(^\s*)|(\s*$)");
            return reg.Replace(inputStr, replaceStr);
        }

        /// <summary>
        /// 仅以指定格式替换左端的空格，string里的 TrimStart也可删除左端空格
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="replaceStr">要替换成的字符</param>
        /// <returns></returns>
        public static string ReplaceTrimStartByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"(^\s*)");
            return reg.Replace(inputStr, replaceStr);
        }

        /// <summary>
        /// 仅以指定格式替换右端的空格，string里的 TrimEnd也可删除右端空格
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="replaceStr">要替换成的字符</param>
        /// <returns></returns>
        public static string ReplaceTrimEndByFormat(string inputStr, string replaceStr = "")
        {
            Regex reg = new Regex(@"(\s*$)");
            return reg.Replace(inputStr, replaceStr);
        }

        #endregion
        
        /// <summary>
        /// 是否是数字和字母组成
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsNumberAndLetterStruct(string inputStr)
        {
            Regex reg = new Regex("^[A-Za-z0-9]+$");
            return reg.IsMatch(inputStr);
        }

        /// <summary>
        /// 是否是小写英文字母组成
        /// </summary>
        /// <param name="inputStr">例如russel</param>
        /// <returns></returns>
        public static bool IsLowercaseLetterStruct(string inputStr)
        {
            Regex reg = new Regex("^[a-z]+$");
            return reg.IsMatch(inputStr);
        }

        /// <summary>
        /// 是否由大写英文字母组成
        /// </summary>
        /// <param name="inputStr">例如ABC</param>
        /// <returns></returns>
        public static bool IsUppercaseLetterStruct(string inputStr)
        {
            Regex reg = new Regex("^[A-Z]+$");
            return reg.IsMatch(inputStr);
        }
    }
}