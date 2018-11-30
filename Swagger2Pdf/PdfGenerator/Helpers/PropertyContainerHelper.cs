using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Swagger2Pdf.PdfGenerator.Helpers
{
    public static class PropertyContainerHelper
    {
        public static TElement Right<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {
            return element.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
        }
        
        public static TElement Left<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {
            return element.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
        }
        
        public static TElement Centered<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {
            return element.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
        }

        public static TElement Bold<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {
            element.SetBold();
            return element;
        }

        public static TElement AsFixedCharLength<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {            
            element.SetFont(FixedCharLengthFont);
            element.SetFontSize(9);
            return element;
        }

        public static TElement AsHeader<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {
            element.SetFont(NormalLengthFont);
            element.SetFontSize(13);
            element.SetBold();
            return element;
        }

        public static TElement AsTitle<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {
            element.SetFont(NormalLengthFont);
            element.SetFontSize(20);
            element.SetBold();
            return element;
        }

        public static TElement AsSubHeader<TElement>(this TElement element) where TElement : ElementPropertyContainer<TElement>
        {
            element.SetFontSize(11);
            return element;
        }

        public static PdfFont FixedCharLengthFont => PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.COURIER);
        public static PdfFont NormalLengthFont => PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN);
    }
}
