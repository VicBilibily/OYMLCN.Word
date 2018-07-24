#pragma warning disable
using System.Collections.Generic;
using System.IO;

namespace OYMLCN.Word.Keywords
{
    /// <summary>
    /// 关键词提取
    /// </summary>
    public abstract class KeywordExtractor
    {
        protected static readonly List<string> DefaultStopWords = new List<string>()
        {
            "the", "of", "is", "and", "to", "in", "that", "we", "for", "an", "are",
            "by", "be", "as", "on", "with", "can", "if", "from", "which", "you", "it",
            "this", "then", "at", "have", "all", "not", "one", "has", "or", "that"
        };

        protected virtual ISet<string> StopWords { get; set; }
        /// <summary>
        /// 设置停用词
        /// </summary>
        /// <param name="stopWordsFile">文件路径</param>
        public void SetStopWords(string stopWordsFile)
        {
            StopWords = new HashSet<string>();

            var path = Path.GetFullPath(stopWordsFile);
            if (File.Exists(path))
                foreach (var line in File.ReadAllLines(path))
                    StopWords.Add(line.Trim());
        }
        public abstract IEnumerable<string> ExtractTags(string text, int count = 20, IEnumerable<string> allowPos = null);
        public abstract IEnumerable<WordWeightPair> ExtractTagsWithWeight(string text, int count = 20, IEnumerable<string> allowPos = null);
    }

}
