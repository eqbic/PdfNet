using System;
using System.Collections.Generic;
using System.Linq;
using PDFiumCore;
using PdfNet.Unsafe;

namespace PdfNet.Core
{
    public class PdfDocument : IDisposable
    {
        private FpdfDocumentT _document;
        private Dictionary<int, PdfPage> _pages;
        private List<PdfPage> _visiblePages = new List<PdfPage>();
        public PdfPage GetPage(int pageNumber) => _pages[pageNumber];

        public int PageCount { get; private set; }

        public PdfDocument(string path, string password = "")
        {
            PdfLibrary.EnsureLoaded();
            _document = fpdfview.FPDF_LoadDocument(path, password);
            SetupDocument();
        }

        public PdfDocument(byte[] data, string password = "")
        {
            PdfLibrary.EnsureLoaded();
            _document = LoadFromData(data, password);
            SetupDocument();
        }

        private FpdfDocumentT LoadFromData(byte[] data, string password = "")
        {
            return UnsafeUtils.LoadRaw(data, password);
        }

        private void SetupDocument()
        {
            PageCount = fpdfview.FPDF_GetPageCount(_document);
            _pages = new Dictionary<int, PdfPage>();
            CachePages(_document);
        }

        private List<PdfPage> GetPagesInViewport(PdfViewport viewport)
        {
            return _pages.Values.Where(page => page.Bottom > viewport.Top || page.Top < viewport.Bottom).ToList();
        }

        public void UpdatePageSizes(PdfViewport viewport)
        {
            var visiblePages = GetPagesInViewport(viewport);
            foreach (var page in visiblePages)
            {
                page.UpdatePageSize(viewport);
            }
        }

        public void Render(PdfViewport viewport, PdfTexture texture)
        {
            var visiblePages = GetPagesInViewport(viewport);
            foreach (var page in visiblePages)
            {
                page.Render(viewport, texture);
            }
        }

        private void CachePages(FpdfDocumentT document)
        {
            for (int i = 0; i < PageCount; i++)
            {
                var page = new PdfPage(fpdfview.FPDF_LoadPage(document, i), i);
                _pages[i] = page;
            }
        }

        public void Dispose()
        {
            foreach (var page in _pages.Values)
            {
                page.Dispose();
            }
            _pages.Clear();
            
            fpdfview.FPDF_CloseDocument(_document);
        }
    }
}