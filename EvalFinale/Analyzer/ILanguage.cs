using System.Collections.Generic;

namespace EvalFinale.Analyzer
{
    public interface ILanguage
    {
        public (List<string>, List<string>) LexicalAnalysis(string code);

    }
}