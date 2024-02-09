using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.Model
{
    public class ArtworkGallery
    {
        private int artworkID;
        private int galleryID;

        public int ArtworkID
        {
            get { return artworkID; }
            set { artworkID = value; }
        }
        public int GalleryID
        {
            get { return galleryID; }
            set { galleryID = value; }
        }

        public ArtworkGallery() { }
        public ArtworkGallery(int artworkID, int galleryID)
        {
            ArtworkID = artworkID   ;
            GalleryID = galleryID;

        }
    }
}
