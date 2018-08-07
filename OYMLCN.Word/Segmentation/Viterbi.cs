﻿using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace OYMLCN.Word.Segmentation
{
    internal interface IFinalSeg
    {
        IEnumerable<string> Cut(string sentence);
    }
    internal class Viterbi : IFinalSeg
    {
        private static readonly Lazy<Viterbi> Lazy = new Lazy<Viterbi>(() => new Viterbi());
        private static readonly char[] States = { 'B', 'M', 'E', 'S' };

        private static readonly Regex RegexChinese = new Regex(@"([\u4E00-\u9FD5]+)", RegexOptions.Compiled);
        private static readonly Regex RegexSkip = new Regex(@"(\d+\.\d+|[a-zA-Z0-9]+)", RegexOptions.Compiled);

        private static IDictionary<char, IDictionary<char, double>> _emitProbs;
        private static IDictionary<char, double> _startProbs;
        private static IDictionary<char, IDictionary<char, double>> _transProbs;
        private static IDictionary<char, char[]> _prevStatus;

        private Viterbi() => LoadModel();

        public static Viterbi Instance=> Lazy.Value;

        public IEnumerable<string> Cut(string sentence)
        {
            var tokens = new List<string>();
            foreach (var blk in RegexChinese.Split(sentence))
                if (RegexChinese.IsMatch(blk))
                    tokens.AddRange(ViterbiCut(blk));
                else
                    tokens.AddRange(RegexSkip.Split(blk).Where(seg => !string.IsNullOrEmpty(seg)));
            return tokens;
        }


        private void LoadModel()
        {
            _prevStatus = new Dictionary<char, char[]>()
            {
                {'B', new []{'E', 'S'}},
                {'M', new []{'M', 'B'}},
                {'S', new []{'S', 'E'}},
                {'E', new []{'B', 'M'}}
            };

            _startProbs = new Dictionary<char, double>()
            {
                {'B', -0.26268660809250016},
                {'E', -3.14e+100},
                {'M', -3.14e+100},
                {'S', -1.4652633398537678}
            };

            _transProbs = Resources.prob_trans_json.GZipDecompress().ConvertToString().DeserializeJsonToObject<IDictionary<char, IDictionary<char, double>>>(); 
            _emitProbs = Resources.prob_emit_json.GZipDecompress().ConvertToString().DeserializeJsonToObject<IDictionary<char, IDictionary<char, double>>>();
        }

        private IEnumerable<string> ViterbiCut(string sentence)
        {
            var v = new List<IDictionary<char, double>>();
            IDictionary<char, Node> path = new Dictionary<char, Node>();

            v.Add(new Dictionary<char, double>());
            foreach (var state in States)
            {
                var emP = _emitProbs[state].SelectValueOrDefault(sentence[0], Constants.MinProb);
                v[0][state] = _startProbs[state] + emP;
                path[state] = new Node(state, null);
            }

            for (var i = 1; i < sentence.Length; ++i)
            {
                IDictionary<char, double> vv = new Dictionary<char, double>();
                v.Add(vv);
                IDictionary<char, Node> newPath = new Dictionary<char, Node>();
                foreach (var y in States)
                {
                    var emp = _emitProbs[y].SelectValueOrDefault(sentence[i], Constants.MinProb);

                    Pair<char> candidate = new Pair<char>('\0', double.MinValue);
                    foreach (var y0 in _prevStatus[y])
                    {
                        var tranp = _transProbs[y0].SelectValueOrDefault(y, Constants.MinProb);
                        tranp = v[i - 1][y0] + tranp + emp;
                        if (candidate.Freq <= tranp)
                        {
                            candidate.Freq = tranp;
                            candidate.Key = y0;
                        }
                    }
                    vv[y] = candidate.Freq;
                    newPath[y] = new Node(y, path[candidate.Key]);
                }
                path = newPath;
            }

            var probE = v[sentence.Length - 1]['E'];
            var probS = v[sentence.Length - 1]['S'];
            var finalPath = probE < probS ? path['S'] : path['E'];

            var posList = new List<char>(sentence.Length);
            while (finalPath != null)
            {
                posList.Add(finalPath.Value);
                finalPath = finalPath.Parent;
            }
            posList.Reverse();

            var tokens = new List<string>();
            int begin = 0, next = 0;
            for (var i = 0; i < sentence.Length; i++)
            {
                var pos = posList[i];
                if (pos == 'B')
                    begin = i;
                else if (pos == 'E')
                {
                    tokens.Add(sentence.Sub(begin, i + 1));
                    next = i + 1;
                }
                else if (pos == 'S')
                {
                    tokens.Add(sentence.Sub(i, i + 1));
                    next = i + 1;
                }
            }
            if (next < sentence.Length)
                tokens.Add(sentence.Substring(next));

            return tokens;
        }

    }

}
