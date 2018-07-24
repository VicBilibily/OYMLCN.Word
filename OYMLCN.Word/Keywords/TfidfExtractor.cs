using System;
using System.Collections.Generic;
using System.Text;
using OYMLCN.Word.Segmentation;
using System.Linq;
using OYMLCN.Word.Segmentation.Pos;

namespace OYMLCN.Word.Keywords
{
    /// <summary>
    /// TF-IDF
    /// </summary>
    public class TfidfExtractor : KeywordExtractor
    {
        private static readonly int DefaultWordCount = 20;

        private Segmenter Segmenter { get; set; }
        private PosSegmenter PosSegmenter { get; set; }
        private IdfLoader Loader { get; set; }

        private IDictionary<string, double> IdfFreq { get; set; }
        private double MedianIdf { get; set; }

        /// <summary>
        /// TF-IDF
        /// </summary>
        /// <param name="segmenter"></param>
        public TfidfExtractor(Segmenter segmenter = null)
        {
            if (segmenter.IsNull())
                Segmenter = new Segmenter();
            else
                Segmenter = segmenter;
            PosSegmenter = new PosSegmenter(Segmenter);
            StopWords = Dict.StopWords;
            if (StopWords.IsEmpty())
                StopWords.UnionWith(DefaultStopWords);

            Loader = new IdfLoader();

            IdfFreq = Loader.IdfFreq;
            MedianIdf = Loader.MedianIdf;
        }
        /// <summary>
        /// 设置IDF频率词典
        /// </summary>
        /// <param name="idfPath"></param>
        public void SetIdfPath(string idfPath)
        {
            Loader.SetNewPath(idfPath);
            IdfFreq = Loader.IdfFreq;
            MedianIdf = Loader.MedianIdf;
        }

        private IEnumerable<string> FilterCutByPos(string text, IEnumerable<string> allowPos)=>
             PosSegmenter.Cut(text).Where(p => allowPos.Contains(p.Flag)).Select(p => p.Word);

        private IDictionary<string, double> GetWordIfidf(string text, IEnumerable<string> allowPos)
        {
            IEnumerable<string> words = null;
            if (allowPos.IsNotEmpty())
                words = FilterCutByPos(text, allowPos);
            else
                words = Segmenter.Cut(text);

            var freq = new Dictionary<string, double>();
            foreach (var word in words)
            {
                var w = word;
                if (string.IsNullOrEmpty(w) || w.Trim().Length < 2 || StopWords.Contains(w.ToLower()))
                    continue;
                freq[w] = freq.SelectValueOrDefault(w, 0.0) + 1.0;
            }
            var total = freq.Values.Sum();
            foreach (var k in freq.Keys.ToList())
                freq[k] *= IdfFreq.SelectValueOrDefault(k, MedianIdf) / total;

            return freq;
        }

        /// <summary>
        /// 提取关键词
        /// </summary>
        /// <param name="text"></param>
        /// <param name="count"></param>
        /// <param name="allowPos">词类</param>
        /// <returns></returns>
        public override IEnumerable<string> ExtractTags(string text, int count = 20, IEnumerable<string> allowPos = null)
        {
            if (count <= 0)
                count = DefaultWordCount; 

            var freq = GetWordIfidf(text, allowPos);
            return freq.OrderByDescending(p => p.Value).Select(p => p.Key).Take(count);
        }
        /// <summary>
        /// 提取关键词（包含权重）
        /// </summary>
        /// <param name="text"></param>
        /// <param name="count"></param>
        /// <param name="allowPos">词类</param>
        /// <returns></returns>
        public override IEnumerable<WordWeightPair> ExtractTagsWithWeight(string text, int count = 20, IEnumerable<string> allowPos = null)
        {
            if (count <= 0)
                count = DefaultWordCount; 

            var freq = GetWordIfidf(text, allowPos);
            return freq.OrderByDescending(p => p.Value).Select(p => new WordWeightPair()
            {
                Word = p.Key,
                Weight = p.Value
            }).Take(count);
        }
    }
    /// <summary>
    /// 关键词
    /// </summary>
    public class WordWeightPair
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public double Weight { get; set; }
    }
}
