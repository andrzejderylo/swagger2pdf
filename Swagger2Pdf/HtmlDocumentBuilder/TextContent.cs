using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Swagger2Pdf.HtmlDocumentBuilder
{
    public sealed class TextContent : HtmlElement
    {
        private readonly List<string> _textContentLines;

        public TextContent(string text) : base("")
        {
            _textContentLines = new List<string> {text};
        }

        protected override void WriteStartTag(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        protected override void WriteEndTag(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        protected override void WriteAttributes(StringBuilder htmlStringBuilder)
        {
            // This is a raw text element so there is no need to implement this
        }

        protected override void WriteContent(StringBuilder htmlStringBuilder)
        {
            foreach (var text in _textContentLines)
            {
                foreach (var c in text)
                {
                    if (HtmlCodes.TryGetValue(c, out string replacement))
                    {
                        htmlStringBuilder.Append(replacement);
                        continue;
                    }

                    htmlStringBuilder.Append(c);
                }
            }
        }

        public TextContent LineBreak()
        {
            _textContentLines.Add("\u00A0");
            return this;
        }

        public TextContent AppendText(string text)
        {
            _textContentLines.Add(text);
            return this;
        }

        private readonly static IReadOnlyDictionary<char, string> HtmlCodes = new ReadOnlyDictionary<char, string>(new Dictionary<char, string>
        {
            ['\u00A0'] = "&nbsp;",
            ['\"'] = "&quot;",
            ['&'] = "&amp;",
            ['\''] = "&apos;",
            ['<'] = "&lt;",
            ['>'] = "&gt;",
            ['¡'] = "&iexcl;",
            ['¢'] = "&cent;",
            ['£'] = "&pound;",
            ['¤'] = "&curren;",
            ['¥'] = "&yen;",
            ['¦'] = "&brvbar;",
            ['§'] = "&sect;",
            ['¨'] = "&uml;",
            ['©'] = "&copy;",
            ['ª'] = "&ordf;",
            ['«'] = "&laquo;",
            ['¬'] = "&not;",
            ['&'] = "hy;",
            ['®'] = "&reg;",
            ['¯'] = "&macr;",
            ['°'] = "&deg;",
            ['±'] = "&plusmn;",
            ['²'] = "&sup2;",
            ['³'] = "&sup3;",
            ['´'] = "&acute;",
            ['µ'] = "&micro;",
            ['¶'] = "&para;",
            ['·'] = "&middot;",
            ['¸'] = "&cedil;",
            ['¹'] = "&sup1;",
            ['º'] = "&ordm;",
            ['»'] = "&raquo;",
            ['¼'] = "&frac14;",
            ['½'] = "&frac12;",
            ['¾'] = "&frac34;",
            ['¿'] = "&iquest;",
            ['À'] = "&Agrave;",
            ['Á'] = "&Aacute;",
            ['Â'] = "&Acirc;",
            ['Ã'] = "&Atilde;",
            ['Ä'] = "&Auml;",
            ['Å'] = "&Aring;",
            ['Æ'] = "&AElig;",
            ['Ç'] = "&Ccedil;",
            ['È'] = "&Egrave;",
            ['É'] = "&Eacute;",
            ['Ê'] = "&Ecirc;",
            ['Ë'] = "&Euml;",
            ['Ì'] = "&Igrave;",
            ['Í'] = "&Iacute;",
            ['Î'] = "&Icirc;",
            ['Ï'] = "&Iuml;",
            ['Ð'] = "&ETH;",
            ['Ñ'] = "&Ntilde;",
            ['Ò'] = "&Ograve;",
            ['Ó'] = "&Oacute;",
            ['Ô'] = "&Ocirc;",
            ['Õ'] = "&Otilde;",
            ['Ö'] = "&Ouml;",
            ['×'] = "&times;",
            ['Ø'] = "&Oslash;",
            ['Ù'] = "&Ugrave;",
            ['Ú'] = "&Uacute;",
            ['Û'] = "&Ucirc;",
            ['Ü'] = "&Uuml;",
            ['Ý'] = "&Yacute;",
            ['Þ'] = "&THORN;",
            ['ß'] = "&szlig;",
            ['à'] = "&agrave;",
            ['á'] = "&aacute;",
            ['â'] = "&;",
            ['ã'] = "&atilde;",
            ['ä'] = "&auml;",
            ['å'] = "&aring;",
            ['æ'] = "&aelig;",
            ['ç'] = "&ccedil;",
            ['è'] = "&egrave;",
            ['é'] = "&eacute;",
            ['ê'] = "&ecirc;",
            ['ë'] = "&euml;",
            ['ì'] = "&igrave;",
            ['í'] = "&iacute;",
            ['î'] = "&icirc;",
            ['ï'] = "&iuml;",
            ['ð'] = "&eth;",
            ['ñ'] = "&ntilde;",
            ['ò'] = "&ograve;",
            ['ó'] = "&oacute;",
            ['ô'] = "&ocirc;",
            ['õ'] = "&otilde;",
            ['ö'] = "&ouml;",
            ['÷'] = "&divide;",
            ['ø'] = "&oslash;",
            ['ù'] = "&ugrave;",
            ['ú'] = "&uacute;",
            ['û'] = "&ucirc;",
            ['ü'] = "&uuml;",
            ['ý'] = "&yacute;",
            ['þ'] = "&thorn;",
            ['ÿ'] = "&yuml;",
        });

        public HtmlElement SetColor(string red)
        {
            throw new System.NotImplementedException();
        }
    }
}