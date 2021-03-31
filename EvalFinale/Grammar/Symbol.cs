namespace EvalFinale.Grammar
{
    public class Symbol
    {
        public string Value { get; set; }
        public bool IsTerminal { get; set; }

        public Symbol(string value, bool isTerminal = true)
        {
            Value = value;
            IsTerminal = isTerminal;
        }

        public override string ToString() => Value;

        protected bool Equals(Symbol other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Symbol) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator ==(Symbol left, Symbol right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Symbol left, Symbol right)
        {
            return !Equals(left, right);
        }
    }
}