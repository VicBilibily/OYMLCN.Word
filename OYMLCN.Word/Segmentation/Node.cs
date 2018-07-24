#pragma warning disable
namespace OYMLCN.Word.Segmentation
{
    internal class Node
    {
        public char Value { get; private set; }
        public Node Parent { get; private set; }

        public Node(char value, Node parent)
        {
            Value = value;
            Parent = parent;
        }
    }
}
