using System;
using System.Collections.Generic;
using System.Linq;
using TranslatorLibrary.Analyzer;

namespace TranslatorLibrary
{
    public static class Traductor
    {
        public static List<string[]> TRADUCTIONS;
        public static Language[] TYPED_LANGUAGES;

        private static int _index;
        private static int _tab;

        private static void Init()
        {
            _index = 0;
            _tab = 0;
        }

        public static string Translate(Node tree, List<string> words, List<string> lexems, Language original, Language traduction)
        {
            Init();

            switch (original)
            {
                case Language.PHP:
                case Language.CS:
                    throw new NotImplementedException("La traduction depuis ce language n'as pas encore été implémenté !");
                case Language.Java:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(original), original, null);
            }

            return traduction switch
            {
                Language.PHP or Language.CS or Language.Java =>
                    LocalTranslate(tree, words, lexems, original, traduction),
                _ => throw new ArgumentException("Ce language n'est pas encore compatible !", nameof(traduction))
            };
        }

        private static string LocalTranslate(Node tree, List<string> words, List<string> lexems, Language original, Language traduction)
        {
            string s = "";

            if (tree.Value.IsTerminal)
            {
                if (tree.Value.ToString() == "ε")
                    return "";

                string w = words[_index];
                string l = lexems[_index];
                bool FindTraduction(string[] t) => t[(int) original] == w;
                bool hasTraduction = TRADUCTIONS.Any(FindTraduction);

                if (!TYPED_LANGUAGES.Contains(traduction) && l == "id" && lexems[_index + 1] != "(" && !hasTraduction)
                    s += "$";

                if (hasTraduction)
                {
                    s += TRADUCTIONS.Find(FindTraduction)[(int) traduction];
                    if (l == "type" && TYPED_LANGUAGES.Contains(traduction))
                        s += " ";
                }
                else
                {
                    string[] spaces =
                    {
                        "+", "-", "*", "/",
                        "=", ">", "<"
                    };

                    s += spaces.Contains(w) ? " " + w + " " : w;
                }

                switch (w)
                {
                    case "{":
                        _tab++;
                        break;
                    case "}":
                        _tab--;
                        break;
                }

                string[] newLine = {";", "{", "}"};
                if (newLine.Contains(w) || words[_index + 1] == "{")
                {
                    s += "\n";
                    int localTab = _tab;
                    if (_index < words.Count - 1)
                    {
                        if (words[_index + 1] == "}")
                            localTab--;
                        for (int i = 0; i < localTab; i++)
                            s += "\t";
                    }
                }

                _index++;
            }
            else
            {
                foreach (Node child in tree.Children)
                    s += LocalTranslate(child, words, lexems, original, traduction);
            }

            return s;
        }

        #region Obsolete Methods

        [Obsolete("Utilisez la méthode avec l'énumération")]
        public static string Translate(Node tree, List<string> words, List<string> lexems, string original, string traduction)
        {
            Language lo = traduction.ToUpper() switch
            {
                "PHP" => Language.PHP,
                "CS" or "C#" => Language.CS,
                _ => throw new ArgumentException("Ce language n'est pas encore compatible !")
            };

            Language lt = traduction.ToUpper() switch
            {
                "PHP" => Language.PHP,
                "CS" or "C#" => Language.CS,
                _ => throw new ArgumentException("Ce language n'est pas encore compatible !")
            };

            return Translate(tree, words, lexems, lo, lt);
        }

        [Obsolete]
        private static string TranslatePHP(Node tree, List<string> words, List<string> lexems, Language orginal) =>
            LocalTranslate(tree, words, lexems, Language.Java, Language.PHP);

        [Obsolete]
        private static string TranslateCS(Node tree, List<string> words, List<string> lexems, Language original) =>
            LocalTranslate(tree, words, lexems, Language.PHP, Language.CS);

        #endregion

        /// <summary>
        /// The list of all the languages compatible.
        /// </summary>
        public enum Language
        {
            /// <summary>
            /// Java language
            /// </summary>
            Java = 0,
            /// <summary>
            /// PHP language
            /// </summary>
            PHP = 1,
            /// <summary>
            /// C# language
            /// </summary>
            CS = 2
        }
    }
}