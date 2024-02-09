using VirtualArtGallery.Service;
namespace VirtualArtGallery
{
    internal class Program
    {
        static void Main(string[] args)
        {

            IVirtualArtGalleryService v = new VirtualArtGalleryService();





            while (true)
            {
                Console.WriteLine("Choose one of the Option\n1. Get Artwork By Id\n2. Add Artwork to Favourite\n3. Remove artwork from favourites\n4. Search Artwork By Artist" +
                    "5. Add Artwork\n6. Remove Artwork\n7. Get Artwork By Id\n8. Update Artwork\n9. Search Artwork By Artist\n10. Get User Favourite Artworks\n11.Exit.\n");
                    
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            v.GetArtworkById();
                            break;
                        case 2:
                            v.AddArtworkToFavourite();
                            break;
                        case 3:
                            v.RemoveArtworkToFavourite();
                            break;
                        case 4:
                            v.SearchArtworksByArtist();
                            break;

                        case 5:
                            v.AddArtwork();
                            break;
                        case 6:
                            v.RemoveArtwork();
                            break;
                        case 7:
                            v.GetArtworkById();
                            break;
                        case 8:
                            v.UpdateArtwork();
                            break;
                        case 9:
                            v.SearchArtworksByArtist();
                            break;
                        case 10:
                            v.GetUserFavouriteArtworks();
                            break;
                        case 11:
                            return;
                        default:
                            Console.WriteLine("Enter Vaild Input");
                            break;
                    }
                }
            }
        }
    }
}



            

            
                