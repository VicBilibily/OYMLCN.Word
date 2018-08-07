using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using JiebaNet.Segmenter.Spelling;
using OYMLCN.Word.Extensions;
using System.Collections.Generic;

namespace OYMLCN.Word.Handlers
{
    /// <summary>
    /// 句子拆词处理
    /// </summary>
    public class SegmenterHandler
    {
        private JiebaSegmenter JiebaSegmenter = JiebaNetExtensions.JiebaSegmenter;

        private string Str;
        internal SegmenterHandler(string str) => Str = str;

        /// <summary>
        /// 分词（不包含长词）
        /// </summary>
        public IEnumerable<string> Cut => JiebaSegmenter.Cut(Str);
        /// <summary>
        /// 分词（不使用HMM算法）
        /// </summary>
        public IEnumerable<string> CutWithoutHMM => JiebaSegmenter.Cut(Str, false, false);

        /// <summary>
        /// 全模式分词（长短词均包含在结果中）
        /// </summary>
        public IEnumerable<string> CutAllApart => JiebaSegmenter.Cut(Str, true);
        /// <summary>
        /// 全模式分词（不使用HMM算法）
        /// </summary>
        public IEnumerable<string> CutAllWithoutHMM => JiebaSegmenter.Cut(Str, true, false);

        /// <summary>
        /// 搜索引擎分词
        /// </summary>
        public IEnumerable<string> CutForSearch => JiebaSegmenter.CutForSearch(Str);
        /// <summary>
        /// 搜索引擎分词（不使用HMM算法）
        /// </summary>
        public IEnumerable<string> CutForSearchWithoutHMM => JiebaSegmenter.CutForSearch(Str, false);

        /// <summary>
        /// 获取分词词类标记
        /// </summary>
        public IEnumerable<Pair> CutAndGetFlag => new PosSegmenter(JiebaSegmenter).Cut(Str);
        /// <summary>
        /// 获取分词词类标记（不使用HMM算法）
        /// </summary>
        public IEnumerable<Pair> CutAndGetFlagWithoutHMM => new PosSegmenter(JiebaSegmenter).Cut(Str, false);


        /// <summary>
        /// 获取分词词语以及位置标记
        /// </summary>
        public IEnumerable<Token> WordTokenize => JiebaSegmenter.Tokenize(Str);
        /// <summary>
        /// 获取分词词语以及位置标记（不使用HMM算法）
        /// </summary>
        public IEnumerable<Token> WordTokenizeWithoutHMM => JiebaSegmenter.Tokenize(Str, TokenizerMode.Default, false);

        /// <summary>
        /// 获取搜索引擎分词词语以及位置标记
        /// </summary>
        public IEnumerable<Token> WordTokenizeForSearch => JiebaSegmenter.Tokenize(Str, TokenizerMode.Search);
        /// <summary>
        /// 获取搜索引擎分词词语以及位置标记（不使用HMM算法）
        /// </summary>
        public IEnumerable<Token> WordTokenizeForSearchWithoutHMM => JiebaSegmenter.Tokenize(Str, TokenizerMode.Search, false);


        SpellChecker _spellChecker = new SpellChecker();
        /// <summary>
        /// 检查词语是否为在典词语（仅中文）
        /// </summary>
        public bool IsWordInDic => (_spellChecker.Suggests(Str) as string[]).Length > 0;


    }
}
