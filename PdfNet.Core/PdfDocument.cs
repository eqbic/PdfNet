using System.Collections.Generic;
using PDFiumCore;

namespace PdfNet.Core
{
    public class PdfDocument<T>
    {
        private FpdfDocumentT _document;
        private readonly Dictionary<int, PdfPage<T>> _pages;

        public PdfPage<T> GetPage(int pageNumber) => _pages[pageNumber];

        public int PageCount { get; }

        public PdfDocument(string path)
        {
            PdfLibrary.EnsureLoaded();
            _document = fpdfview.FPDF_LoadDocument(path, "");
            PageCount = fpdfview.FPDF_GetPageCount(_document);
            _pages = new Dictionary<int, PdfPage<T>>();
            CachePages(_document);
        }

        public void Render(PdfViewport<T> viewport)
        {
            // each page has same size as first one
            _pages[0].UpdatePageSize(viewport);
            var firstPageSize = _pages[0].Rectangle.Height;
        
            var startPageNumber = (int)(viewport.Position.Y / firstPageSize);
            var endPageNumber = (int)((viewport.Position.Y + viewport.Size.Y) / firstPageSize);

            var startPage = GetPage(startPageNumber);
            var endPage = GetPage(endPageNumber);
        
            startPage.UpdatePageSize(viewport);
            startPage.Render(viewport);
            if (startPageNumber != endPageNumber)
            {
                endPage.UpdatePageSize(viewport);
                endPage.Render(viewport);
            }
        }

        private void CachePages(FpdfDocumentT document)
        {
            for (int i = 0; i < PageCount; i++)
            {
                var page = new PdfPage<T>(fpdfview.FPDF_LoadPage(document, i), i);
                _pages[i] = page;
            }
        }
    }
}