using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.Exceptions
{
            public class ArtWorkNotFoundException : ApplicationException
        {
            ArtWorkNotFoundException() { }
            public ArtWorkNotFoundException(string message) : base(message)
            {

            }
        }
    
}
