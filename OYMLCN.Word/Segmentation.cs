using System;
using System.Collections.Generic;
using System.Text;
using OYMLCN.Word.Segmentation;
using OYMLCN.Word.Segmentation.Pos;

namespace OYMLCN.WordExtensions
{
    /// <summary>
    /// Extension
    /// </summary>
    public static class WorkSegmentationExtensions
    {
        static Segmenter _segmenter;
        static Segmenter Segmenter => _segmenter ?? (_segmenter = new Segmenter());

        /// <summary>
        /// 分词（不包含长词）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> CutApart(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Cut(str);
        /// <summary>
        /// 分词（不使用HMM算法）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> CutApartWithoutHMM(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Cut(str, false, false);

        /// <summary>
        /// 全模式分词（长短词均包含在结果中）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> CutAllApart(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Cut(str, true);
        /// <summary>
        /// 全模式分词（不使用HMM算法）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> CutAllApartWithoutHMM(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Cut(str, true, false);

        /// <summary>
        /// 搜索引擎分词
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> CutApartForSearch(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).CutForSearch(str);
        /// <summary>
        /// 搜索引擎分词（不使用HMM算法）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<string> CutApartForSearchWithoutHMM(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).CutForSearch(str, false);

        /// <summary>
        /// 获取分词词类标记
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<Pair> CutApartAndGetFlag(this string str, Segmenter segmenter = null) =>
            new PosSegmenter(segmenter ?? Segmenter).Cut(str);
        /// <summary>
        /// 获取分词词类标记（不使用HMM算法）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<Pair> CutApartAndGetFlagWithoutHMM(this string str, Segmenter segmenter = null) =>
            new PosSegmenter(segmenter ?? Segmenter).Cut(str, false);


        /// <summary>
        /// 获取分词词语以及位置标记
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<Token> GetWordTokenize(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Tokenize(str);
        /// <summary>
        /// 获取分词词语以及位置标记（不使用HMM算法）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<Token> GetWordTokenizeWithoutHMM(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Tokenize(str, TokenizerMode.Default, false);

        /// <summary>
        /// 获取搜索引擎分词词语以及位置标记
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<Token> GetWordTokenizeForSearch(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Tokenize(str, TokenizerMode.Search);
        /// <summary>
        /// 获取搜索引擎分词词语以及位置标记（不使用HMM算法）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="segmenter">分词器（不提供则使用默认分词词典）</param>
        /// <returns></returns>
        public static IEnumerable<Token> GetWordTokenizeForSearchWithoutHMM(this string str, Segmenter segmenter = null) =>
            (segmenter ?? Segmenter).Tokenize(str, TokenizerMode.Search, false);


        static SpellChecker _spellChecker;
        /// <summary>
        /// 检查词语是否为在典词语（仅中文）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckSpell(this string str) =>
            ((_spellChecker ?? (_spellChecker = new SpellChecker())).Suggests(str) as string[]).Length > 0;

    }
}
