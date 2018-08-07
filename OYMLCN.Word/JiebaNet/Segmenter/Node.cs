#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
namespace JiebaNet.Segmenter
{
    public class Node
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