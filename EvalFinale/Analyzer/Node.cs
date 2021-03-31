using System.Collections.Generic;
using EvalFinale.Grammar;

namespace EvalFinale.Analyzer
{
    public class Node
    {
        public Symbol Value { get; set; }
        public List<Node> Children { get; set; }

        public Node(Symbol value)
        {
            Value = value;
            Children = new List<Node>();
        }

        public void AddChildren(Node child) => Children.Add(child);

        public List<Symbol> Lr()
        {
            List<Symbol> result = new List<Symbol> {Value};

            foreach (Node child in Children)
                result.AddRange(child.Lr());

            return result;
        }

        public List<Node> Leaves()
        {
            List<Node> result = new List<Node>();

            if (Children.Count == 0)
                result.Add(this);

            foreach (Node child in Children)
                result.AddRange(child.Leaves());

            return result;
        }

        public Node Next(Node node)
        {
            List<Node> leaves = Leaves();

            if (!leaves.Contains(node))
                return null;

            int i = leaves.IndexOf(node);

            return i == leaves.Count - 1 ? null : leaves[i + 1];
        }

        public string Indent(int level)
        {
            string s = "";

            for (int i = 0; i < level; i++)
                s += " |";

            return s;
        }

        public string ToIndentedString(int level)
        {
            string s = Indent(level);

            s += (level > 0 ? "-" : "") + Value + "\n";

            foreach (Node child in Children)
                s += child.ToIndentedString(level + 1);

            return s;
        }

        public override string ToString()
        {
            return ToIndentedString(0);
        }

        protected bool Equals(Node other)
        {
            return Equals(Value, other.Value) && Equals(Children, other.Children);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value != null ? Value.GetHashCode() : 0) * 397) ^ (Children != null ? Children.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Node left, Node right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !Equals(left, right);
        }

        public Node Copy() => MemberwiseClone() as Node;
    }
}