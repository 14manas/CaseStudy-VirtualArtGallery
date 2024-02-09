using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGallery.Model;

namespace VirtualArtGallery.Repository
{
  
        internal interface IVirtualArtGalleryRepository
        {
            public bool addArtwork(Artwork artwork);
            public bool removeArtwork(int artworkID);
            public Artwork getArtworkById(int artworkID);
            public bool addArtworkToFavourite(int userID, int artWorkID);

            public bool removeArtworkToFavourite(int userID, int artWorkID);

            public bool updateArtwork(Artwork artwork, int artworkID);

            public List<Artwork> searchArtworksbyArtist(string artist);

            public List<Artwork> getUserFavouriteArtworks(int userID);
            public bool createGallery(Gallery gallery);
            public bool updateGallery(Gallery gallery, int galleryID);
            public bool removeGallery(int galleryID);
            public Gallery getGalleryById(int galleryID);
            

        }
    
}
