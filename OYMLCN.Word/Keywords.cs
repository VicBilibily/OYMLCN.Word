using OYMLCN.Word.Keywords;
using OYMLCN.Word.Segmentation;
using System.Collections.Generic;

namespace OYMLCN.Word
{
    /// <summary>
    /// Extension
    /// </summary>
    public static class WordKeywordsExtensions
    {
        static Segmenter _segmenter;
        static Segmenter Segmenter => _segmenter ?? (_segmenter = new Segmenter());

        static TfidfExtractor _tfidfExtractor;
        static TfidfExtractor TfidfExtractor => _tfidfExtractor ?? (_tfidfExtractor = new TfidfExtractor(Segmenter));

        /// <summary>
        /// 获取关键词
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> GetKeyWords(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTags(str, count);
        /// <summary>
        /// 获取关键词（名词）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> GetKeyWordsOnlyNoun(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTags(str, count, Constants.NounPos);
        /// <summary>
        /// 获取关键词（动词）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> GetKeyWordsOnlyVerb(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTags(str, count, Constants.VerbPos);
        /// <summary>
        /// 获取关键词（名/动词）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> GetKeyWordsNounAndVerb(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTags(str, count, Constants.NounAndVerbPos);


        /// <summary>
        /// 获取关键词权重
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<WordWeightPair> GetKeyWordsWegiht(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTagsWithWeight(str, count);
        /// <summary>
        /// 获取关键词权重（名词）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<WordWeightPair> GetKeyWordsWegihtOnlyNoun(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTagsWithWeight(str, count, Constants.NounPos);
        /// <summary>
        /// 获取关键词权重（动词）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<WordWeightPair> GetKeyWordsWegihtOnlyVerb(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTagsWithWeight(str, count, Constants.VerbPos);
        /// <summary>
        /// 获取关键词权重（名/动词）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">数量</param>
        /// <param name="tfidf">词典（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<WordWeightPair> GetKeyWordsWegihtNounAndVerb(this string str, int count = 20, TfidfExtractor tfidf = null) =>
            (tfidf ?? TfidfExtractor).ExtractTagsWithWeight(str, count, Constants.NounAndVerbPos);


    }
}
