using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.Exception
{
    public class GalleryNotFoundException : ApplicationException
    {
        GalleryNotFoundException() { }
        public GalleryNotFoundException(string message) : base(message)
        {

        }
    }
}