using System;
using System.Collections.Generic;
using System.Linq;
using TranslatorLibrary.Grammar;
using static TranslatorLibrary.Traductor;

namespace TranslatorLibrary.Analyzer
{
    public static class Analyze
    {
        public static char[] SEPARATORS = {' ', '\n', '\t'};

        public static (List<string>, List<string>) LexicalAnalysis(string code, Language language)
        {
            ILanguage l = language switch
            {
                Language.Java => new Java(),
                Language.CS => new CS(),
                Language.PHP => new PHP(),
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            };

            return l.LexicalAnalysis(code);
        }

        public static Node SyntaxicAnalysis(List<string> lexems, List<Rule> rules, Dictionary<Symbol, Dictionary<Symbol, Rule>> table)
        {
            Symbol actual = rules[0].Left;

            Node tree = new Node(actual);
            Node node = tree.Copy();
            int i = 0;
            int cut = 0;

            while (i < lexems.Count && cut < 1000)
            {
                string lexem = lexems[i];
                Symbol lexemT = new Symbol(lexem);
                if (actual.IsTerminal)
                {
                    if (actual != new Symbol("ε") && actual != lexemT)
                        throw new Exception("My Syntax Error: " + lexemT + " instead of " + actual);
                    
                    if (actual == lexemT)
                        i++;

                    node = tree.Next(node);
                    actual = node.Value;
                }
                else
                {
                    if (!table[actual].ContainsKey(lexemT))
                        throw new Exception("My Syntax Error: no rule from " + actual);


                    Rule rule = table[actual][lexemT];

                    foreach (Symbol symbol in rule.Expression)
                        node.AddChildren(new Node(symbol));
                    
                    node = node.Children[0];
                    actual = node.Value;
                }

                cut++;
            }

            return tree;
        }

        private class Java : ILanguage
        {
            private readonly Dictionary<string, string> _reserved = new Dictionary<string, string>
            {
                {"int", "type"},
                {"bool", "type"},
                {"true", "val"},
                {"=", "="},
                {";", ";"},
                {"(", "("},
                {")", ")"},
                {"{", "{"},
                {"}", "}"},
                {"while", "while"},
                {"if", "if"},
                {"else", "else"},
                {"<", "<"},
                {">", ">"},
                {"+", "+"},
                {"-", "-"},
                {"*", "*"},
                {"/", "/"},
                {"%", "%"},
                {"System.out.println", "id"}
            };

            private List<string> _words;
            private List<string> _lexems;

            private int _i;
            private string _builtWord;

            public (List<string>, List<string>) LexicalAnalysis(string code)
            {
                _words = new List<string>();
                _lexems = new List<string>();

                _i = 0;
                _builtWord = "";

                while (_i < code.Length)
                {
                    if (SEPARATORS.Contains(code[_i]))
                    {
                        if (_builtWord.Length != 0)
                        {
                            _words.Add(_builtWord);
                            _lexems.Add(decimal.TryParse(_builtWord, out _) || bool.TryParse(_builtWord, out _) ? "val" : "id");
                        }

                        _builtWord = "";
                        _i++;
                    }
                    else
                    {
                        string foundWord = null;
                        string foundLexem = null;

                        foreach (string key in _reserved.Keys)
                        {
                            if (_i + key.Length > code.Length)
                                continue;

                            if (key == code.Substring(_i, key.Length))
                            {
                                foundWord = key;
                                foundLexem = _reserved[key];
                                break;
                            }
                        }

                        if (foundWord is null)
                        {
                            _builtWord += code[_i];
                            _i++;
                        }
                        else
                        {
                            if (_builtWord.Length != 0)
                            {
                                _words.Add(_builtWord);
                                _lexems.Add(decimal.TryParse(_builtWord, out _) || bool.TryParse(_builtWord, out _) ? "val" : "id");
                                _builtWord = "";
                            }

                            _words.Add(foundWord);
                            _lexems.Add(foundLexem);

                            _i += foundWord.Length;
                        }
                    }
                }

                return (_words, _lexems);
            }
        }

        private class CS : ILanguage
        {
            public (List<string>, List<string>) LexicalAnalysis(string code)
            {
                throw new NotImplementedException();
            }
        }

        private class PHP : ILanguage
        {
            public (List<string>, List<string>) LexicalAnalysis(string code)
            {
                throw new NotImplementedException();
            }
        }
    }
}