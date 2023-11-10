using System;
using PDFiumCore;

namespace PdfNet.Core
{
    public class PdfLibrary : IDisposable
    {
        private static readonly object _syncRoot = new object();
        private static PdfLibrary _library;
    
        public static void EnsureLoaded()
        {
            lock (_syncRoot)
            {
                if (_library == null)
                {
                    _library = new PdfLibrary();
                }
            }
        }

        private bool _disposed;

        private PdfLibrary()
        {
            fpdfview.FPDF_InitLibrary();
        }

        ~PdfLibrary()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                fpdfview.FPDF_DestroyLibrary();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}