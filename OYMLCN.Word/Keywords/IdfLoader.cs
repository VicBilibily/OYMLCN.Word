﻿using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OYMLCN.Word.Keywords
{
    internal class IdfLoader
    {
        internal string IdfFilePath { get; set; }
        internal IDictionary<string, double> IdfFreq { get; set; }
        internal double MedianIdf { get; set; }

        public IdfLoader(string idfPath = null)
        {
            IdfFilePath = string.Empty;
            IdfFreq = new Dictionary<string, double>();
            MedianIdf = 0.0;
            if (!idfPath.IsNullOrWhiteSpace())
                SetNewPath(idfPath);
            else
            {
                IdfFreq = Dict.Idf;
                MedianIdf = IdfFreq.Values.OrderBy(v => v).ToList()[IdfFreq.Count / 2];
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
    }

}
