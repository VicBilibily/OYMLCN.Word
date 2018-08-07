using JiebaNet.Analyser;
using JiebaNet.Segmenter;
using OYMLCN.Word.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OYMLCN.Word.Handlers
{
    /// <summary>
    /// 词语分析处理
    /// </summary>
    public class AnalyserHandler
    {
        private TfidfExtractor TfidfExtractor = JiebaNetExtensions.TfidfExtractor;

        private string Str;
        private int Length;
        internal AnalyserHandler(string str, int length)
        {
            Str = str;
            Length = length;
        }

        /// <summary>
        /// 获取关键词
        /// </summary>
        public IEnumerable<string> Keywords => TfidfExtractor.ExtractTags(Str, Length);
        /// <summary>
        /// 获取关键词（名词）
        /// </summary>
        public IEnumerable<string> KeywordsOnlyNoun => TfidfExtractor.ExtractTags(Str, Length, Constants.NounPos);
        /// <summary>
        /// 获取关键词（动词）
        /// </summary>
        public IEnumerable<string> KeywordsOnlyVerb => TfidfExtractor.ExtractTags(Str, Length, Constants.VerbPos);
        /// <summary>
        /// 获取关键词（名/动词）
        /// </summary>
        public IEnumerable<string> KeywordsNounAndVerb => TfidfExtractor.ExtractTags(Str, Length, Constants.NounAndVerbPos);

        /// <summary>
        /// 获取关键词权重
        /// </summary>
        public IEnumerable<WordWeightPair> KeywordsWegiht => TfidfExtractor.ExtractTagsWithWeight(Str, Length);
        /// <summary>
        /// 获取关键词权重（名词）
        /// </summary>
        public IEnumerable<WordWeightPair> KeywordsWegihtOnlyNoun => TfidfExtractor.ExtractTagsWithWeight(Str, Length, Constants.NounPos);
        /// <summary>
        /// 获取关键词权重（动词）
        /// </summary>
        public IEnumerable<WordWeightPair> KeywordsWegihtOnlyVerb => TfidfExtractor.ExtractTagsWithWeight(Str, Length, Constants.VerbPos);
        /// <summary>
        /// 获取关键词权重（名/动词）
        /// </summary>
        public IEnumerable<WordWeightPair> KeywordsWegihtNounAndVerb => TfidfExtractor.ExtractTagsWithWeight(Str, Length, Constants.NounAndVerbPos);

    }
}
