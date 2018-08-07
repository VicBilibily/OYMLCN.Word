using JiebaNet.Analyser;
using JiebaNet.Segmenter;
using OYMLCN.Word.Handlers;

namespace OYMLCN.Word.Extensions
{
    /// <summary>
    /// Extension
    /// </summary>
    public static class JiebaNetExtensions
    {
        /// <summary>
        /// 分词处理主处理类，本库已内置基本词库，增加词典请参阅 https://github.com/anderscui/jieba.NET
        /// </summary>
        public static JiebaSegmenter JiebaSegmenter { get; } = new JiebaSegmenter();

        private static TfidfExtractor _tfidfExtractor;
        internal static TfidfExtractor TfidfExtractor => _tfidfExtractor ?? (_tfidfExtractor = new TfidfExtractor(JiebaSegmenter));

        /// <summary>
        /// 拆分句子处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static SegmenterHandler AsSegmenter(this string str) => new SegmenterHandler(str);
        /// <summary>
        /// 获取分析关键词
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static AnalyserHandler AsKeywordsAnalyser(this string str, int length = 20) => new AnalyserHandler(str, length);

    }
}
