package captionr.common;

import java.math.BigInteger;

public class LanguageCaption {
    public BigInteger offset;
    public String language;
    public String text;

    public LanguageCaption(BigInteger offset, String language, String text) {
        this.offset = offset;
        this.language = language;
        this.text = text;
    }
}