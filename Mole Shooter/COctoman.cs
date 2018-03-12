using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octoman_Shooter.Properties;

namespace Octoman_Shooter
{
    class COctoman :CImageBase
    {
        private Rectangle _octomanHotSpot = new Rectangle();

        public COctoman()
            : base(Resources.octoman)
        {
            _octomanHotSpot.X = left + 63;
            _octomanHotSpot.Y = top - 1;
            _octomanHotSpot.Width = 57;
            _octomanHotSpot.Height = 86;
        }

        public void Update(int x, int y)
        {
            left = x;
            top = y;
            _octomanHotSpot.X = left + 63;
            _octomanHotSpot.Y = top - 1;
        }

        public bool Hit(int x, int y)
        {
            Rectangle c = new Rectangle(x, y, 1, 1);

            if (_octomanHotSpot.Contains(c))
            {
                return true;
            }
            return false;
        }
    }
}
