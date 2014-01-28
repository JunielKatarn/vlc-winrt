﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLC_WINRT.Common;
using VLC_WINRT.ViewModels;
using VLC_WINRT.ViewModels.MainPage;

namespace VLC_WINRT.Utility.Commands.MusicPlayer
{
    public class FavoriteTrackCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            if (parameter as MusicLibraryViewModel.TrackItem != null)
            {
                // searching the track in the trackcollection
                int i = Locator.MusicLibraryVM.Track.IndexOf(parameter as MusicLibraryViewModel.TrackItem);

                // If the track is favorite, then now it is not
                // if the track was not favorite, now it is
                // updating the Track collection
                Locator.MusicLibraryVM.Track[i].Favorite = !(parameter as MusicLibraryViewModel.TrackItem).Favorite;

                var trackFromArtistCollection = Locator.MusicLibraryVM.Artist.FirstOrDefault(
                    x =>
                    {
                        var trackItem = parameter as MusicLibraryViewModel.TrackItem;
                        return trackItem != null && x.Name == trackItem.ArtistName;
                    })
                    .Albums.FirstOrDefault(y =>
                    {
                        var item = parameter as MusicLibraryViewModel.TrackItem;
                        return item != null && y.Name == item.AlbumName;
                    })
                    .Tracks.FirstOrDefault(z => z == (parameter as MusicLibraryViewModel.TrackItem));
                
                if(trackFromArtistCollection != null)
                    trackFromArtistCollection.Favorite = Locator.MusicLibraryVM.Track[i].Favorite;


                // serializing and saving the new Artist collection with updated Favorite property
                Locator.MusicLibraryVM.SerializeArtistsDataBase();
            }
        }
    }
}