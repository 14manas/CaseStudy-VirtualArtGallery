using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.Model
{
    public class UserFavouriteArtwork
    {
        private int userID;
        private int artworkID;

        public int ArtworkID
        {
            get { return userID; }
            set { userID = value; }
        }
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public UserFavouriteArtwork() { }
        public UserFavouriteArtwork(int userID, int artworkID)
        {
            UserID = userID;
            ArtworkID = artworkID;

        }
    }
}
