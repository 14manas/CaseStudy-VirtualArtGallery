using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.Service
{
    internal interface IVirtualArtGalleryService
    {
        void AddArtwork();
        void RemoveArtwork();
        void GetArtworkById();

        void AddArtworkToFavourite();
        void RemoveArtworkToFavourite();
        void UpdateArtwork();

        void SearchArtworksByArtist();

        void GetUserFavouriteArtworks();
        void AddGallery();
        void UpdateGallery();
        void RemoveGallery();
        void GetGalleryById();

       




    }
}

