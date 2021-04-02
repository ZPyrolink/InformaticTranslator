using System.Collections.Generic;

namespace TranslatorLibrary.Analyzer
{
    public interface ILanguage
    {
        (List<string>, List<string>) LexicalAnalysis(string code);

    }
}