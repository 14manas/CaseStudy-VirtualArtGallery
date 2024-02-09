using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGallery.Exceptions;
using VirtualArtGallery.Model;
using VirtualArtGallery.Utility;

namespace VirtualArtGallery.Repository
{ 
        public class VirtualArtGalleryRepository : IVirtualArtGalleryRepository
        {
        public string connectionString;
        SqlConnection sqlconnection = null;
        SqlCommand cmd = null;
        public VirtualArtGalleryRepository()
        {
            sqlconnection = new SqlConnection(DBConnectionUtility.GetConnectionString());
            cmd = new SqlCommand();
        }


        public bool addArtwork(Artwork artwork)
            {
                try
                {
                    sqlconnection.Open();
                    cmd.CommandText = "Insert into Artwork values(@artworkID,@title,@description,@creationDate,@medium,@imageUrl,@artistID)";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@artworkID", artwork.ArtworkID);
                    cmd.Parameters.AddWithValue("@title", artwork.Title);
                    cmd.Parameters.AddWithValue("@description", artwork.Description);
                    cmd.Parameters.AddWithValue("@creationDate", artwork.CreationDate);
                    cmd.Parameters.AddWithValue("@medium", artwork.Medium);
                    cmd.Parameters.AddWithValue("@imageUrl", artwork.ImageUrl);
                    cmd.Parameters.AddWithValue("@artistID", artwork.ArtistID);
                    int addArtworkStatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (addArtworkStatus > 0) return true;
                    else return false;
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }
            public bool removeArtwork(int artworkID)
            {
                sqlconnection.Open();
                cmd.CommandText = "Delete from Artwork where ArtworkID = @artworkID";
                cmd.Connection = sqlconnection;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@artworkID", artworkID);
                int removeArtworkStatus = cmd.ExecuteNonQuery();
                sqlconnection.Close();
                if (removeArtworkStatus > 0)
                {
                    Console.WriteLine("Artwork Removed.");
                    return true;
                }
                else
                {
                    throw new ArtWorkNotFoundException($"Artwork with ArtworkId {artworkID} not found.");
                }

            }

            public Artwork getArtworkById(int artworkID)
            {
                sqlconnection.Open();
                cmd.CommandText = "select * from Artwork where ArtworkID = @artworkID";
                cmd.Connection = sqlconnection;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@artworkID", artworkID);
                Artwork artwork = new Artwork();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        artwork.ArtworkID = (int)reader["ArtworkID"];
                        artwork.Title = (string)reader["Title"];
                        artwork.Description = (string)reader["Description"];
                        artwork.CreationDate = (DateTime)reader["CreationDate"];
                        artwork.Medium = (string)reader["Medium"];
                        artwork.ImageUrl = (string)reader["ImageUrl"];
                        artwork.ArtistID = (int)reader["ArtistID"];
                        sqlconnection.Close();
                        return artwork;
                    }
                    else
                    {
                        sqlconnection.Close();
                        throw new ArtWorkNotFoundException($"Artwork with ArtworkID {artworkID} not found.");

                    }
                }

            }
            public bool addArtworkToFavourite(int userID, int artWorkID)
            {
                try
                {
                    sqlconnection.Open();

                    cmd.CommandText = "select FavoriteArtworks from Users where UserID = @users";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@users", userID);
                    string favourite_artwork = "";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            favourite_artwork = (string)reader["FavoriteArtworks"];
                        }
                        else
                        {
                            sqlconnection.Close();
                            throw new UserNotFoundException($"No User Found with user ID{userID}");
                        }
                    }
                    sqlconnection.Close();

                    string result = string.Concat(favourite_artwork, ",", artWorkID.ToString());

                    sqlconnection.Open();
                    cmd.CommandText = "Insert into UserFavouriteArtwork values(@userID,@artworkID);" +
                        "update Users set FavoriteArtworks = @result where UserID= @userID";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@userId", userID);
                    cmd.Parameters.AddWithValue("@artworkId", artWorkID);
                    cmd.Parameters.AddWithValue("@result", result);
                    int adduserfavartworkStatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (adduserfavartworkStatus > 0) return true;
                    else
                    {
                        throw new ArtWorkNotFoundException($"No Artwork Found with Artwork Id{artWorkID}");
                    }

                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }

            }
            public bool removeArtworkToFavourite(int userID, int artWorkID)
            {
                try
                {
                    sqlconnection.Open();

                    cmd.CommandText = "select FavoriteArtworks from Users where UserID = @user";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@user", userID);
                    string favourite_artwork = "";
                    string result = "";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            favourite_artwork = (string)reader["FavoriteArtworks"];
                        }
                        else
                        {
                            sqlconnection.Close();
                            throw new UserNotFoundException($"No User Found with user Id{userID}");
                        }
                    }
                    int[] favartworkids = favourite_artwork.Split(',').Select(int.Parse).ToArray();
                    foreach (int artworkid in favartworkids)
                    {
                        if (artworkid != artWorkID)
                        {
                            result = result + artworkid.ToString() + ",";
                        }
                    }
                    result = result.Remove(result.Length - 1);
                    cmd.CommandText = "delete from UserFavouriteArtwork where UserId=@userID and ArtworkID=@artworkID;" +
                        "update Users set FavoriteArtworks = @result where UserID= @userID";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@userId", userID);
                    cmd.Parameters.AddWithValue("@artworkId", artWorkID);
                    cmd.Parameters.AddWithValue("@result", result);
                    int adduserfavartworkStatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (adduserfavartworkStatus > 0) return true;
                    else
                    {
                        throw new ArtWorkNotFoundException($"No Artwork Found with Artwork Id{userID}");    
                    }

                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }

            }
            public bool updateArtwork(Artwork artwork, int artworkID)
            {
                sqlconnection.Open();
                cmd.CommandText = "update Artwork set Title = @title,Description = @description,CreationDate = @creationDate,Medium = @medium,ImageUrl = @imageUrl,ArtistID =@artistID where ArtworkID=@ArtworkID";
                cmd.Connection = sqlconnection;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@artworkID", artwork.ArtworkID);
                cmd.Parameters.AddWithValue("@title", artwork.Title);
                cmd.Parameters.AddWithValue("@description", artwork.Description);
                cmd.Parameters.AddWithValue("@creationDate", artwork.CreationDate);
                cmd.Parameters.AddWithValue("@medium", artwork.Medium);
                cmd.Parameters.AddWithValue("@imageUrl", artwork.ImageUrl);
                cmd.Parameters.AddWithValue("@artistID", artwork.ArtistID);
                int updateArtworkStatus = cmd.ExecuteNonQuery();
                sqlconnection.Close();
                if (updateArtworkStatus > 0) return true;
                else
                {
                    throw new ArtWorkNotFoundException($"No Artwork found with Artwork ID {artworkID}");
                }
            }
            public List<Artwork> searchArtworksbyArtist(string artist)
            {
                try
                {
                    List<Artwork> artworks = new List<Artwork>();
                    cmd.CommandText = "select * from Artwork join Artist on Artwork.ArtistID=Artist.ArtistID where Artist.Name = @name;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@name", artist);
                    cmd.Connection = sqlconnection;
                    sqlconnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        Artwork artwork = new Artwork();
                        artwork.ArtworkID = (int)reader["ArtworkID"];
                        artwork.Title = (string)reader["Title"];
                        artwork.Description = (string)reader["Description"];
                        artwork.CreationDate = (DateTime)reader["CreationDate"];
                        artwork.Medium = (string)reader["Medium"];
                        artwork.ImageUrl = (string)reader["ImageUrl"];
                        artwork.ArtistID = (int)reader["ArtistID"];
                        artworks.Add(artwork);
                        count++;
                    }
                    sqlconnection.Close();
                    if (count == 0)
                    {
                        throw new ArtWorkNotFoundException($"No Artwork found of Artist {artist}");
                    }
                    else return artworks;
                }
                catch (System.Exception ex)
                {

                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }

            public List<Artwork> getUserFavouriteArtworks(int userID)
            {
                try
                {
                    List<Artwork> artworks = new List<Artwork>();
                    cmd.CommandText = "select * from Artwork where artworkId in(select ArtworkId from UserFavouriteArtwork where UserID = @userID);";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Connection = sqlconnection;
                    sqlconnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        Artwork artwork = new Artwork();
                        artwork.ArtworkID = (int)reader["ArtworkID"];
                        artwork.Title = (string)reader["Title"];
                        artwork.Description = (string)reader["Description"];
                        artwork.CreationDate = (DateTime)reader["CreationDate"];
                        artwork.Medium = (string)reader["Medium"];
                        artwork.ImageUrl = (string)reader["ImageUrl"];
                        artwork.ArtistID = (int)reader["ArtistID"];
                        artworks.Add(artwork);
                        count++;
                    }
                    sqlconnection.Close();
                    if (count > 0)
                    {
                        return artworks;
                    }
                    else
                    {
                        throw new UserNotFoundException($"No User Found with user id {userID}");
                    }
                }
                catch (System.Exception ex)
                {

                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return null;
                }
            }

            public bool createGallery(Gallery gallery)
            {
                try
                {
                    sqlconnection.Open();
                    cmd.CommandText = "Insert into Gallery values(@galleryID,@name,@description,@location,@curator,@openinghours)";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@galleryID", gallery.GalleryID);
                    cmd.Parameters.AddWithValue("@name", gallery.Name);
                    cmd.Parameters.AddWithValue("@description", gallery.Description);
                    cmd.Parameters.AddWithValue("@location", gallery.Location);
                    cmd.Parameters.AddWithValue("@curator", gallery.Curator);
                    cmd.Parameters.AddWithValue("@openinghours", gallery.OpeningHours);

                    int addGalleryStatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (addGalleryStatus > 0) return true;
                    else return false;
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }
            public bool updateGallery(Gallery gallery, int galleryID)
            {
                try
                {
                    sqlconnection.Open();
                    cmd.CommandText = "update Gallery set Name = @name,Description = @description,Location = @location,Curator= @curator,OpeningHours= @openinghours where GalleryID=@galleryID";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@galleryID", gallery.GalleryID);
                    cmd.Parameters.AddWithValue("@name", gallery.Name);
                    cmd.Parameters.AddWithValue("@description", gallery.Description);
                    cmd.Parameters.AddWithValue("@location", gallery.Location);
                    cmd.Parameters.AddWithValue("@curator", gallery.Curator);
                    cmd.Parameters.AddWithValue("@openinghours", gallery.OpeningHours);
                    int updateGalleryStatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (updateGalleryStatus > 0) return true;

                    else
                    {
                        return false;
                    }


                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }

            public bool removeGallery(int galleryID)
            {
                try
                {
                    sqlconnection.Open();
                    cmd.CommandText = "Delete from Gallery where Gallery_id = @galleryID";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@galleryID", galleryID);
                    int removeGalleryStatus = cmd.ExecuteNonQuery();
                    sqlconnection.Close();
                    if (removeGalleryStatus > 0)
                    {
                        Console.WriteLine("Gallery Removed.");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                   
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }
            public Gallery getGalleryById(int galleryID)
            {

                
                sqlconnection.Open();
                cmd.CommandText = "select * from Gallery where Gallery_ID = @galleryID";
                cmd.Connection = sqlconnection;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@galleryID", galleryID);
                Gallery gallery = new Gallery();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        gallery.GalleryID = galleryID;
                        gallery.Name = (string)reader["Name"];
                        gallery.Description = (string)reader["description"];
                        gallery.Location = (string)reader["location"];
                        gallery.Curator = (int)reader["curator"];
                        gallery.OpeningHours = (string)reader["openingHours"];
                        sqlconnection.Close();
                        return gallery;
                    }
                    else
                    {
                        sqlconnection.Close();
                        throw new System.Exception($"An error occurred");

                    }
                }
            }
            
            
        }
}