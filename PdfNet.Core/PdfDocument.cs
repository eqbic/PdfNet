using System;
using System.Collections.Generic;
using PDFiumCore;

namespace PdfNet.Core
{
    public class PdfDocument
    {
        private readonly FpdfDocumentT _document;
        private Dictionary<int, PdfPage> _pages;

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
            try
            {
                unsafe
                {
                    fixed (byte* pointer = data)
                    {
                        IntPtr ptr = (IntPtr)pointer;
                        return fpdfview.FPDF_LoadMemDocument(ptr, data.Length, password);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void SetupDocument()
        {
            PageCount = fpdfview.FPDF_GetPageCount(_document);
            _pages = new Dictionary<int, PdfPage>();
            CachePages(_document);
        }

        public void Render(PdfViewport viewport)
        {
            // each page has same size as first one
            _pages[0].UpdatePageSize(viewport);
            var firstPageSize = _pages[0].Rectangle.Height;
        
            var startPageNumber = viewport.Position.Y / firstPageSize;
            var endPageNumber = (viewport.Position.Y + viewport.Size.Y) / firstPageSize;

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
                var page = new PdfPage(fpdfview.FPDF_LoadPage(document, i), i);
                _pages[i] = page;
            }
        }
    }
}