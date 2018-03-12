using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octoman_Shooter
{
    class CImageBase : IDisposable
    {
        private bool disposed = false;

        private Bitmap _bitmap;
        private int x;
        private int y;

        public int left { get { return x;} set { x = value; } }
        public int top { get { return y; } set { y = value; } }

        public CImageBase(Bitmap _resource)
        {
            _bitmap = new Bitmap(_resource);
        }

        public void DrawImage(Graphics gfx)
        {
            gfx.DrawImage(_bitmap, x, y);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                _bitmap.Dispose();
            }

            disposed = true;
        }

    }
}
