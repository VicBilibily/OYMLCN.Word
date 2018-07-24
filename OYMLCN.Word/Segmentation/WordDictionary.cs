using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OYMLCN.Word.Segmentation
{
    internal class WordDictionary
    {
        private static readonly Lazy<WordDictionary> lazy = new Lazy<WordDictionary>(() => new WordDictionary());

        internal IDictionary<string, int> Trie = new Dictionary<string, int>();

        public double Total { get; set; }

        private WordDictionary() => LoadDict();

        public static WordDictionary Instance => lazy.Value;

        private void LoadDict()
        {

            foreach (var item in Segmentation.Word.Dict)
            {
                Trie[item.Key] = item.Frequency;
                Total += item.Frequency;

                foreach (var ch in Enumerable.Range(0, item.Key.Length))
                {
                    var wfrag = item.Key.Sub(0, ch + 1);
                    if (!Trie.ContainsKey(wfrag))
                        Trie[wfrag] = 0;
                }
            }
        }

        public bool ContainsWord(string word)=>
            Trie.ContainsKey(word) && Trie[word] > 0;

        public int GetFreqOrDefault(string key) =>
            ContainsWord(key) ? Trie[key] : 1;

        public void AddWord(string word, int freq, string tag = null)
        {
            if (ContainsWord(word))
                Total -= Trie[word];

            Trie[word] = freq;
            Total += freq;
            for (var i = 0; i < word.Length; i++)
            {
                var wfrag = word.Substring(0, i + 1);
                if (!Trie.ContainsKey(wfrag))
                    Trie[wfrag] = 0;
            }
        }

        public void DeleteWord(string word)=> AddWord(word, 0);

        internal int SuggestFreq(string word, IEnumerable<string> segments)
        {
            double freq = 1;
            foreach (var seg in segments)
                freq *= GetFreqOrDefault(seg) / Total;

            return Math.Max((int)(freq * Total) + 1, GetFreqOrDefault(word));
        }
    }

}
