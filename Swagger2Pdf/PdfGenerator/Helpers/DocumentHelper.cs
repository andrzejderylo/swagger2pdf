﻿using System;
using iText.IO.Image;
using iText.Layout;
using iText.Layout.Element;

namespace Swagger2Pdf.PdfGenerator.Helpers
{
    public static class DocumentHelper
    {
        public static void AddParagraph(this Document document, Action<Paragraph> p = null)
        {
            var paragraph = new Paragraph();
            p?.Invoke(paragraph);
            document.Add(paragraph);
        }
        
        public static void AddParagraph(this Document document, string text, Action<Paragraph> p = null)
        {
            var paragraph = new Paragraph(text);
            p?.Invoke(paragraph);
            document.Add(paragraph);
        }

        public static void AddAreaBreak(this Document document)
        {
            document.Add(new AreaBreak());
        }

        public static void AddPageBreakableParagraph(this Document document, string text, Action<Paragraph> p = null)
        {
            document.AddParagraph(text, p);
        }

        public static Image AddImage(this Document document, string imagePath)
        {
            var imgData = ImageDataFactory.Create(new Uri(imagePath));
            var image = new Image(imgData);
            document.Add(image);
            return image;
        }
    }
}
