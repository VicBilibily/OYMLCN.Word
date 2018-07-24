#pragma warning disable
namespace OYMLCN.Word.Segmentation
{
    public class Token
    {
        public string Word { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        internal Token(string word, int startIndex, int endIndex)
        {
            Word = word;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public override string ToString()=>
            string.Format("[{0}, ({1}, {2})]", Word, StartIndex, EndIndex);
    }
}
