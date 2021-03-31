using System;
using System.Collections.Generic;
using System.Linq;
using EvalFinale.Analyzer;
using EvalFinale.Grammar;
using XmlJsonManager.Xml;

namespace EvalFinale
{
    class Program
    {
        static void Main(string[] args)
        {
            #region NonTerminals

            Symbol PROG = new Symbol("PROG", false);
            Symbol LIGNE = new Symbol("LIGNE", false);
            Symbol DECL = new Symbol("DECL", false);
            Symbol STRUCT = new Symbol("STRUCT", false);
            Symbol ID = new Symbol("ID", false);
            Symbol EXPR = new Symbol("EXPR", false);
            Symbol SUITE = new Symbol("SUITE", false);
            Symbol ELEM = new Symbol("ELEM", false);
            Symbol OP = new Symbol("OP", false);

            #endregion
            #region Terminals

            Symbol type = new Symbol("type");
            Symbol epsylon = new Symbol("ε");
            Symbol pv = new Symbol(";");
            Symbol id = new Symbol("id");
            Symbol eq = new Symbol("=");
            Symbol po = new Symbol("(");
            Symbol pf = new Symbol(")");
            Symbol ao = new Symbol("{");
            Symbol af = new Symbol("}");
            Symbol @while = new Symbol("while");
            Symbol @if = new Symbol("if");
            Symbol @else = new Symbol("else");
            Symbol val = new Symbol("val");
            Symbol plus = new Symbol("+");
            Symbol moins = new Symbol("-");
            Symbol fois = new Symbol("*");
            Symbol div = new Symbol("/");
            Symbol inf = new Symbol("<");
            Symbol sup = new Symbol(">");
            Symbol modulo = new Symbol("%");
            Symbol dollar = new Symbol("$");

            #endregion
            #region Rules

            Rule R1 = new Rule(PROG, new[] {LIGNE, PROG});
            Rule R2 = new Rule(PROG, new[] {epsylon});
            Rule R3 = new Rule(LIGNE, new[] {DECL, pv});
            Rule R4 = new Rule(LIGNE, new[] {STRUCT});
            Rule R5 = new Rule(LIGNE, new[] {id, ID, pv});
            Rule R6 = new Rule(ID, new[] {eq, EXPR});
            Rule R7 = new Rule(ID, new[] {po, EXPR, pf});
            Rule R8 = new Rule(DECL, new[] {type, id, eq, EXPR});
            Rule R9 = new Rule(STRUCT, new[] {@while, po, EXPR, pf, ao, PROG, af});
            Rule R10 = new Rule(STRUCT, new[] {@if, po, EXPR, pf, ao, PROG, af, @else, ao, PROG, af});
            Rule R11 = new Rule(EXPR, new[] {ELEM, SUITE});
            Rule R12 = new Rule(SUITE, new[] {OP, EXPR});
            Rule R24 = new Rule(SUITE, new[] {OP, id, ID});
            Rule R13 = new Rule(SUITE, new[] {epsylon});
            Rule R14 = new Rule(ELEM, new[] {id});
            Rule R15 = new Rule(ELEM, new[] {val});
            Rule R23 = new Rule(ELEM, new[] {id, ID});
            Rule R16 = new Rule(OP, new[] {plus});
            Rule R17 = new Rule(OP, new[] {moins});
            Rule R18 = new Rule(OP, new[] {fois});
            Rule R19 = new Rule(OP, new[] {div});
            Rule R20 = new Rule(OP, new[] {inf});
            Rule R21 = new Rule(OP, new[] {sup});
            Rule R22 = new Rule(OP, new[] {modulo});

            #endregion

            List<Rule> grammar = new List<Rule>
            {
                R1, R2, R3, R4, R5, R6, R7, R8, R9, R10,
                R11, R12, R13, R14, R15, R16, R17, R18, R19, R20,
                R21, R22, R23, R24
            };

            Dictionary<Symbol, Dictionary<Symbol, Rule>> table =
                new Dictionary<Symbol, Dictionary<Symbol, Rule>>
            {
                {
                    PROG, new Dictionary<Symbol, Rule>
                    {
                        {type, R1}, {@while, R1}, {@if, R1}, {id, R1}, {dollar, R2}, {af, R2}
                    }
                },
                {
                    LIGNE, new Dictionary<Symbol, Rule>
                    {
                        {type, R3}, {@while, R4}, {@if, R4}, {id, R5}
                    }
                },
                {
                    DECL, new Dictionary<Symbol, Rule>
                    {
                        {type, R8}
                    }
                },
                {
                    STRUCT, new Dictionary<Symbol, Rule>
                    {
                        {@while, R9}, {@if, R10}
                    }
                },
                {
                    ID, new Dictionary<Symbol, Rule>
                    {
                        {eq, R6}, {po, R7}, 
                    }
                },
                {
                    EXPR, new Dictionary<Symbol, Rule>
                    {
                        {id, R11}, {val, R11}, 
                    }
                },
                {
                    SUITE, new Dictionary<Symbol, Rule>
                    {
                        {plus, R12}, {moins, R12}, {fois, R12}, {div, R12}, {sup, R12},
                        {inf, R12}, {modulo, R12}, {pf, R13}, {pv, R13},
                        {po, R7}
                    }
                },
                {
                    ELEM, new Dictionary<Symbol, Rule>
                    {
                        {id, R14}, {val, R15}
                    }
                },
                {
                    OP, new Dictionary<Symbol, Rule>
                    {
                        {plus, R16}, {moins, R17}, {fois, R18}, {div, R19},
                        {inf, R20}, {sup, R21}, {modulo, R22},
                        {po, R7}
                    }
                }
            };

            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.Write("Bienvenue dans le traducteur ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Java ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("PHP ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("C#\n");
            Console.ForegroundColor = defaultColor;

            string code = "int x = 0;\n" +
                          "int y = 10;\n" +
                          "int e = Math.E;\n" +
                          "int pi = Math.PI;\n" +
                          "int i = Math.sqrt(pi);\n" +
                          "bool b = true;\n" +
                          "while(x<42) { \n" +
                            "\tSystem.out.println(x); \n" +
                            "\tx = x+42;\n" +
                            "\tmult2(y);\n" +
                            "\tif (pi > Math.abs(e)) {\n" +
                                "\t\tMath.abs(pi);\n" +
                            "\t}\n" +
                            "\telse {\n" +
                                "\t\tMath.exp(5);\n" +
                            "\t}\n" +
                          "}";
            Console.WriteLine("Code original :\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(code);
            Console.ForegroundColor = defaultColor;

            LoadTranslationData();

            (List<string> words, List<string> lexems) = Analyze.LexicalAnalysis(code, Traductor.Language.Java);

            Node tree = Analyze.SyntaxicAnalysis(lexems, grammar, table);

            string codePHP = Traductor.Translate(tree, words, lexems, Traductor.Language.Java, Traductor.Language.PHP);
            string codeCS = Traductor.Translate(tree, words, lexems, Traductor.Language.Java, Traductor.Language.CS);

            Console.WriteLine("Traduction en PHP :\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(codePHP);
            Console.ForegroundColor = defaultColor;
            Console.WriteLine("Traduction en C# :\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(codeCS);
            Console.ForegroundColor = defaultColor;

            Console.WriteLine("DEBUG : ");
            Console.WriteLine("Liste des mots :\n");
            words.Debug();
            Console.WriteLine("\nListe des lexems :\n");
            lexems.Debug();
            Console.WriteLine("\nArbre des symboles :");
            Console.WriteLine(tree);
        }

        private static void LoadTranslationData()
        {
            Traductor.TRADUCTIONS = XmlManager.Load<List<string[]>>("Traduction.xml");
            Traductor.TYPED_LANGUAGES = new[]
            {
                Traductor.Language.CS,
                Traductor.Language.Java,
            };
        }
    }

    public static class Utility
    {
        public static void Debug<T>(this IEnumerable<T> list)
        {
            if (!list.Any() || !list.Any(x => x is not null))
            {
                Console.WriteLine("The list is empty !");
                return;
            }

            T firstNotNull = list.First(x => x is not null);

            string contour = firstNotNull switch
            {
                string => "\"",
                char => "'",
                _ => ""
            };

            string s = list.Aggregate("[ ", (current, value) => current + contour + value + contour + ", ");

            s = s.Substring(0, s.Length - 2);
            s += " ]";

            Console.WriteLine(s);
        }
    }
}
