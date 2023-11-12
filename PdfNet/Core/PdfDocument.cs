using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
        public Vector2 DocumentSize { get; private set; }

        public int PageCount { get; private set; }

        public PdfDocument(string path, PdfViewport viewport, string password = "")
        {
            PdfLibrary.EnsureLoaded();
            _document = fpdfview.FPDF_LoadDocument(path, password);
            SetupDocument(viewport);
        }

        public PdfDocument(byte[] data, PdfViewport viewport, string password = "")
        {
            PdfLibrary.EnsureLoaded();
            _document = LoadFromData(data, password);
            SetupDocument(viewport);
        }

        private FpdfDocumentT LoadFromData(byte[] data, string password = "")
        {
            return UnsafeUtils.LoadRaw(data, password);
        }

        private void SetupDocument(PdfViewport viewport)
        {
            PageCount = fpdfview.FPDF_GetPageCount(_document);
            _pages = new Dictionary<int, PdfPage>();
            CachePages(_document);
            UpdatePageSizes(viewport);
            
        }

        private List<PdfPage> GetPagesInViewport(PdfViewport viewport)
        {
            return _pages.Values.Where(page => RectangleF.Intersect(viewport.Rectangle, page.Rectangle).Size != Size.Empty).ToList();
        }

        private void UpdatePageSizes(PdfViewport viewport)
        {
            var documentHeight = 0f;
            var documentWidth = 0f;
            foreach (var page in _pages.Values)
            {
                page.UpdatePageSize(viewport);
                documentHeight += page.Rectangle.Height;
                if (page.Rectangle.Width > documentWidth)
                {
                    documentWidth = page.Rectangle.Width;
                }
            }

            DocumentSize = new Vector2(documentWidth, documentHeight);
            viewport.DocumentHeight = documentHeight;
        }

        public PdfTexture Render(PdfViewport viewport, PdfTexture texture)
        {
            var visiblePages = GetPagesInViewport(viewport);
            foreach (var page in visiblePages)
            {
                page.Render(viewport, texture);
            }
            // _pages[0].Render(viewport, texture);
            return texture;
        }

        private void CachePages(FpdfDocumentT document)
        {
            for (int i = 0; i < PageCount; i++)
            {
                var nativePage = fpdfview.FPDF_LoadPage(document, i);
                var page = new PdfPage(nativePage, i);
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