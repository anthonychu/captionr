using System.Collections.Generic;
using System.Linq;

namespace CaptionR.Common
{
    public static class Constants
    {
        public static readonly IReadOnlyList<string> LANGUAGES = 
            new List<string> { "en-US", "fr-FR", "es-ES", "ko-KO", "ja-JP", "de-DE" };
        public static readonly IReadOnlyList<string> LANGUAGE_CODES = 
            LANGUAGES.Select(l => l.Substring(0, 2)).ToList();
    }
}