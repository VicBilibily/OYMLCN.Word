#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OYMLCN.Word.Segmentation
{
#if DEBUG
    public partial class Word
#else
    internal partial class Word
#endif
    {
        static List<Word> _dict;
#if DEBUG
        public static List<Word> Dict
        {
            get
            {
                if (_dict != null)
                    return _dict;

                _dict = new List<Word>();
                var dic = "Resources/";
                var str = new StringBuilder();
                str.AppendLine((dic + "dict.txt").GetFileInfo().ReadAllText());
                str.AppendLine((dic + "dict.big").GetFileInfo().ReadAllText());
                str.AppendLine((dic + "dict.small").GetFileInfo().ReadAllText());
                var lines = str.ToString().SplitByLine().Where(d => !d.Trim().IsNullOrWhiteSpace()).Distinct();
                var dict = new Dictionary<string, string>();
                foreach (var item in lines)
                {
                    var line = item.SplitBySign(" ");
                    if (line.Length == 3)
                    {
                        string key = line.FirstOrDefault();
                        string value = item.SubString(key.Length + 1);
                        dict[key] = value;
                    }
                }
                str = new StringBuilder();
                foreach (var item in dict)
                    str.AppendLine($"{item.Key} {item.Value}");
                "dict.full.txt".GetFileInfo().WriteAllText(str.ToString());
                return _dict;
            }
        }
#else
        internal static List<Word> Dict
        {
            get
            {
                if (_dict != null)
                    return _dict;
                _dict = new List<Word>();
                var lines = Resources.dict_full_txt.GZipDecompress().ConvertToString().SplitByLine().Where(d => !d.Trim().IsNullOrWhiteSpace()).Distinct();
                var dict = new Dictionary<string, string>();
                foreach (var item in lines)
                {
                    var line = item.SplitBySign(" ");
                    if (line.Length == 3)
                    {
                        string key = line.FirstOrDefault();
                        string value = item.SubString(key.Length + 1);
                        dict[key] = value;
                    }
                }
                foreach (var item in dict)
                {
                    var line = item.Value.SplitBySign(" ");
                    _dict.Add(new Word()
                    {
                        Key = item.Key,
                        Frequency = line.First().AsType().Int,
                        Tag = line.Last().Trim()
                    });
                }
                return _dict;
            }
        }
#endif
        public string Key { get; set; }
        public int Frequency { get; set; }
        public string Tag { get; set; }
    }
}
