#pragma warning disable
namespace OYMLCN.Word.Segmentation.Pos
{
    public class Pair
    {
        public string Word { get; set; }
        public string Flag { get; set; }
        internal Pair(string word, string flag)
        {
            Word = word;
            Flag = flag;
        }

        public override string ToString() => string.Format("{0}/{1}", Word, Flag);
    }
}
