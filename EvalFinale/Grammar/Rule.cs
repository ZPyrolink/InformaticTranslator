namespace EvalFinale.Grammar
{
    public class Rule
    {
        public Symbol Left { get; set; }
        public Symbol[] Expression { get; set; }

        public Rule(Symbol left, Symbol[] expression)
        {
            Left = left;
            Expression = expression;
        }
    }
}