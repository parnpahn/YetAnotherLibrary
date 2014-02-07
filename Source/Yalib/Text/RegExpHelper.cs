using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hlt.Text
{
    public static class RegExpHelper
    {
        /// <summary>
        /// 尋找成對的標籤。例如：<姓名>Michael</姓名>。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MatchCollection FindTagPairs(string s)
        {
            // 此樣式會找成對的標籤，但如果字串中出現重複的標籤，就只會找到第一個。例如：<姓名>Michael</姓名>。
            const string OneTagPair = @"<(?<tag>\\w*)>(?<text>.*)</\\k<tag>>";

            return Regex.Matches(s, OneTagPair);
        }

        /// <summary>
        /// 尋找起始標籤或結束標籤。例如：<姓名>、</aa>。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MatchCollection FindTags(string s)
        {
            // 此樣式只要是起始標籤或結束標前都會尋找，但不管是否成對。例如：<姓名>、</aa>。
            const string Tags = @"<([^<>\s]*)(\s[^<>]*)?>";

            return Regex.Matches(s, Tags);
        }
    }
}
