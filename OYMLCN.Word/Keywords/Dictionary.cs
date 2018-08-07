#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OYMLCN.Word.Keywords
{
#if DEBUG
    public static partial class Dict
#else
    internal static partial class Dict
#endif
    {
        public static ISet<string> StopWords
        {
            get
            {
                var stopWords = new HashSet<string>();
#if DEBUG

                var str = new StringBuilder();
                var dir = "Resources/";
                str.AppendLine((dir + "stopwords.txt").GetFileInfo().ReadAllText());
                str.AppendLine((dir + "stopwords_en_nltk.txt").GetFileInfo().ReadAllText());
                str.AppendLine((dir + "stopwords_zh_hit.txt").GetFileInfo().ReadAllText());
                var words = str.ToString().SplitByLine().Distinct().Where(d => !d.IsNullOrWhiteSpace()).OrderBy(d => d);
                foreach (var line in words)
                    stopWords.Add(line.Trim());
                "stopwords.full.txt".GetFileInfo().WriteAllText(stopWords.Join("\r\n"));
#else
                foreach (var item in Resources.stopwords_full_txt.GZipDecompress().ConvertToString().SplitByLine())
                    stopWords.Add(item);
#endif
                return stopWords;
            }
        }

        public static IDictionary<string, double> Idf
        {
            get
            {
                var idf = new Dictionary<string, double>();
#if DEBUG
                var str = new StringBuilder();
                var dir = "Resources/";
                str.AppendLine((dir + "idf.txt").GetFileInfo().ReadAllText());
                str.AppendLine((dir + "idf.txt.big").GetFileInfo().ReadAllText());
                var data = str.ToString();
#else
                var data = Resources.idf_full_txt.GZipDecompress().ConvertToString();
#endif
                var words = data.SplitByLine().Distinct().Where(d => !d.IsNullOrWhiteSpace()).OrderBy(d => d);
                foreach (var line in words)
                {
                    var word = line.SplitBySign(" ");
                    var key = word.First();
                    var freq = word.Skip(1).First().AsType().Double;
                    idf[key] = freq;
                }
#if DEBUG
                str = new StringBuilder();
                foreach (var item in idf)
                    str.AppendLine($"{item.Key} {item.Value}");
                "idf.full.txt".GetFileInfo().WriteAllText(str.ToString());
#endif
                return idf;
            }
        }
    }
}
