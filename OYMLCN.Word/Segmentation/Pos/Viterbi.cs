#pragma warning disable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace OYMLCN.Word.Segmentation.Pos
{
    internal class Viterbi
    {
        private static readonly Lazy<Viterbi> Lazy = new Lazy<Viterbi>(() => new Viterbi());

        private static IDictionary<string, double> _startProbs;
        private static IDictionary<string, IDictionary<string, double>> _transProbs;
        private static IDictionary<string, IDictionary<char, double>> _emitProbs;
        private static IDictionary<char, List<string>> _stateTab;

        private Viterbi() => LoadModel();

        public static Viterbi Instance => Lazy.Value;

        public IEnumerable<Pair> Cut(string sentence)
        {
            var probPosList = ViterbiCut(sentence);
            var posList = probPosList.Item2;

            var tokens = new List<Pair>();
            int begin = 0, next = 0;
            for (var i = 0; i < sentence.Length; i++)
            {
                var parts = posList[i].Split('-');
                var charState = parts[0][0];
                var pos = parts[1];
                if (charState == 'B')
                    begin = i;
                else if (charState == 'E')
                {
                    tokens.Add(new Pair(sentence.Sub(begin, i + 1), pos));
                    next = i + 1;
                }
                else if (charState == 'S')
                {
                    tokens.Add(new Pair(sentence.Sub(i, i + 1), pos));
                    next = i + 1;
                }
            }
            if (next < sentence.Length)
                tokens.Add(new Pair(sentence.Substring(next), posList[next].Split('-')[1]));

            return tokens;
        }


        private static void LoadModel()
        {
            _startProbs = Resources.pos_prob_start_json.GZipDecompress().ConvertToString().DeserializeJsonString<IDictionary<string, double>>();
            _transProbs = Resources.pos_prob_trans_json.GZipDecompress().ConvertToString().DeserializeJsonString<IDictionary<string, IDictionary<string, double>>>();
            _emitProbs = Resources.pos_prob_emit_json.GZipDecompress().ConvertToString().DeserializeJsonString<IDictionary<string, IDictionary<char, double>>>();
            _stateTab = Resources.char_state_tab_json.GZipDecompress().ConvertToString().DeserializeJsonString<IDictionary<char, List<string>>>();
        }

        private Tuple<double, List<string>> ViterbiCut(string sentence)
        {
            var v = new List<IDictionary<string, double>>();
            var memPath = new List<IDictionary<string, string>>();

            var allStates = _transProbs.Keys.ToList();

            v.Add(new Dictionary<string, Double>());
            memPath.Add(new Dictionary<string, string>());
            foreach (var state in _stateTab.SelectValueOrDefault(sentence[0], allStates))
            {
                var emP = _emitProbs[state].SelectValueOrDefault(sentence[0], Constants.MinProb);
                v[0][state] = _startProbs[state] + emP;
                memPath[0][state] = string.Empty;
            }

            for (var i = 1; i < sentence.Length; ++i)
            {
                v.Add(new Dictionary<string, double>());
                memPath.Add(new Dictionary<string, string>());

                var prevStates = memPath[i - 1].Keys.Where(k => _transProbs[k].Count > 0);
                var curPossibleStates = new HashSet<string>(prevStates.SelectMany(s => _transProbs[s].Keys));

                IEnumerable<string> obsStates = _stateTab.SelectValueOrDefault(sentence[i], allStates);
                obsStates = curPossibleStates.Intersect(obsStates);

                if (!obsStates.Any())
                {
                    if (curPossibleStates.Count > 0)
                        obsStates = curPossibleStates;
                    else
                        obsStates = allStates;
                }

                foreach (var y in obsStates)
                {
                    var emp = _emitProbs[y].SelectValueOrDefault(sentence[i], Constants.MinProb);

                    var prob = double.MinValue;
                    var state = string.Empty;

                    foreach (var y0 in prevStates)
                    {
                        var tranp = _transProbs[y0].SelectValueOrDefault(y, double.MinValue);
                        tranp = v[i - 1][y0] + tranp + emp;
                        if (prob < tranp || (prob == tranp && string.Compare(state, y0, StringComparison.CurrentCultureIgnoreCase) < 0))
                        {
                            prob = tranp;
                            state = y0;
                        }
                    }
                    v[i][y] = prob;
                    memPath[i][y] = state;
                }
            }

            var vLast = v.Last();
            var last = memPath.Last().Keys.Select(y => new { State = y, Prob = vLast[y] });
            var endProb = double.MinValue;
            var endState = string.Empty;
            foreach (var endPoint in last)
                if (endProb < endPoint.Prob || (endProb == endPoint.Prob && String.Compare(endState, endPoint.State, StringComparison.CurrentCultureIgnoreCase) < 0))
                {
                    endProb = endPoint.Prob;
                    endState = endPoint.State;
                }

            var route = new string[sentence.Length];
            var n = sentence.Length - 1;
            var curState = endState;
            while (n >= 0)
            {
                route[n] = curState;
                curState = memPath[n][curState];
                n--;
            }

            return new Tuple<double, List<string>>(endProb, route.ToList());
        }
    }
}
