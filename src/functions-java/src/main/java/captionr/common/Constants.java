package captionr.common;

import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public class Constants {
    public static List<String> languages;
    public static List<String> languageCodes;

    static {
        languages = Arrays.asList("en-US", "fr-FR", "es-ES", "ko-KO", "ja-JP", "de-DE");
        languageCodes = languages.stream().map(l -> l.substring(0, 2)).collect(Collectors.toList());
    }
}