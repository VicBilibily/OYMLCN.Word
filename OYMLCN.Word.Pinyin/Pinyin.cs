using OYMLCN.Extensions;
using OYMLCN.Word.Pinyin;
using OYMLCN.Word.Pinyin.Properties;
using System.Collections.Generic;
using System.Linq;

namespace OYMLCN.Word.Extensions
{
    /// <summary>
    /// PinYinExtensions
    /// </summary>
    public static class PinYinExtensions
    {
        /// <summary>
        /// 根据汉字获取拼音，如果不是汉字直接返回原字符
        /// </summary>
        /// <param name="str">要转换的汉字</param>
        /// <param name="polyphone">默认支持多音字</param>
        /// <returns></returns>
        public static PinyinModel Pinyin(this string str, bool polyphone = true)
        {
            var result = new PinyinModel();
            List<string> total = new List<string>(), first = new List<string>();
            foreach (var strChar in str.StringToArray())
            {
                string pinyin = strChar, py = strChar;
                if (strChar.AsFormat().IsChineseRegString)
                {
                    string[] a = GetPinyinByOne(strChar, polyphone);
                    if (a.Count() > 0)
                    {
                        pinyin = a.Join(" ");
                        py = a.Select(d => d.Substring(0, 1)).Distinct().Join(" ");
                    }
                }
                total.Add(pinyin);
                first.Add(py);
            }
            result.TotalPinYin = HandlePolyphone(total);
            result.FirstPinYin = HandlePolyphone(first);
            return result;
        }


        /// <summary>
        /// 处理多音字，将类似['chang zhang', 'cheng'] 转换成 ['changcheng', 'zhangcheng']
        /// </summary>
        /// <param name="list">转换前列表</param>
        /// <returns>转换后数组</returns>
        private static string[] HandlePolyphone(List<string> list)
        {
            List<string> result = new List<string>(), temp;
            for (var i = 0; i < list.Count(); i++)
            {
                temp = new List<string>();
                var t = list[i].Split(new char[] { ' ' });
                for (var j = 0; j < t.Count(); j++)
                    if (result.Count() > 0)
                        for (var k = 0; k < result.Count(); k++)
                            temp.Add(result[k] + t[j]);
                    else
                        temp.Add(t[j]);
                result = temp;
            }
            return result.Distinct().ToArray();
        }
        /// <summary>
        /// 根据单个汉字获取拼音
        /// </summary>
        /// <param name="str">单个汉字</param>
        /// <param name="polyphone">是否支持多音字 (否则根据字库中返回第一个拼音)</param>
        /// <returns></returns>
        static string[] GetPinyinByOne(string str, bool polyphone)
        {
            var result = Dic.Where(d => d.Value.Contains(str)).Select(d => d.Key);
            if (polyphone)
                return result.ToArray();
            else
                return result.Take(1).ToArray();
        }

        static Dictionary<string, string> _dic;
        /// <summary>
        /// 字典
        /// </summary>
        internal static Dictionary<string, string> Dic
        {
            get
            {
                if (_dic != null)
                    return _dic;

                _dic = new Dictionary<string, string>();

                var lines = Resources.pinyin_txt.GZipDecompress().ConvertToString().SplitByLine();
                foreach (var item in lines)
                {
                    var data = item.Split('|');
                    _dic.Add(data[0], data[1]);
                }

                return _dic;
            }
        }
    }
}
