#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using System;
using System.Collections.Generic;

namespace JiebaNet.Segmenter.FinalSeg
{
    /// <summary>
    /// 在词典切分之后，使用此接口进行切分，默认实现为HMM方法。
    /// </summary>
    public interface IFinalSeg
    {
        IEnumerable<string> Cut(string sentence);
    }
}