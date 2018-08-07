#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using OYMLCN.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JiebaNet.Analyser
{
    public class IdfLoader
    {
        internal string IdfFilePath { get; set; }
        internal IDictionary<string, double> IdfFreq { get; set; }
        internal double MedianIdf { get; set; }

        public IdfLoader(string idfPath = null)
        {
            IdfFilePath = string.Empty;
            IdfFreq = new Dictionary<string, double>();
            MedianIdf = 0.0;
            if (!string.IsNullOrWhiteSpace(idfPath))
            {
                SetNewPath(idfPath);
            }
        }

        public void SetNewPath(string newIdfPath)
        {
            var idfPath = Path.GetFullPath(newIdfPath);
            if (IdfFilePath != idfPath)
            {
                IdfFilePath = idfPath;
                var lines = File.ReadAllLines(idfPath, Encoding.UTF8);
                IdfFreq = new Dictionary<string, double>();
                foreach (var line in lines)
                {
                    var parts = line.Trim().Split(' ');
                    var word = parts[0];
                    var freq = double.Parse(parts[1]);
                    IdfFreq[word] = freq;
                }

                MedianIdf = IdfFreq.Values.OrderBy(v => v).ToList()[IdfFreq.Count / 2];
            }
        }

        // Private Method
        internal void LoadFromResources()
        {
            IdfFilePath = string.Empty;
            MedianIdf = 0.0;
            IdfFreq = new Dictionary<string, double>();

            var lines = OYMLCN.Word.Resources.idf_txt.GZipDecompress().ConvertToString().SplitByLine();
            foreach (var line in lines)
            {
                var parts = line.Trim().Split(' ');
                var word = parts[0];
                var freq = double.Parse(parts[1]);
                IdfFreq[word] = freq;
            }

            MedianIdf = IdfFreq.Values.OrderBy(v => v).ToList()[IdfFreq.Count / 2];
        }

    }
}