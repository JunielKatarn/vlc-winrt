﻿/**********************************************************************
 * VLC for WinRT
 **********************************************************************
 * Copyright © 2013-2014 VideoLAN and Authors
 *
 * Licensed under GPLv2+ and MPLv2
 * Refer to COPYING file of the official project for license
 **********************************************************************/

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using VLC_WINRT_APP.Model;
using VLC_WINRT_APP.ViewModels;
using WinRTXamlToolkit.Imaging;

namespace VLC_WINRT_APP.Helpers
{
    public class UpdateTileHelper
    {
        public static void UpdateMediumTileWithMusicInfo()
        {
            const TileTemplateType template = TileTemplateType.TileSquare150x150PeekImageAndText02;
            var tileXml = TileUpdateManager.GetTemplateContent(template);

            var tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = "playing";
            if (Locator.MusicPlayerVM.CurrentTrack != null)
            {
                tileTextAttributes[1].InnerText = Locator.MusicPlayerVM.CurrentTrack.Name + " - " + Locator.MusicPlayerVM.CurrentTrack.ArtistName;

                var tileImgAttribues = tileXml.GetElementsByTagName("image");
                if (Locator.MusicPlayerVM.CurrentAlbum != null)
                    tileImgAttribues[0].Attributes[1].NodeValue = Locator.MusicPlayerVM.CurrentAlbum.Picture;
            }

            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            UpdateBigTileWithMusicInfo();
        }

        public static void UpdateBigTileWithMusicInfo()
        {
            const TileTemplateType template = TileTemplateType.TileWide310x150PeekImage05;
            var tileXml = TileUpdateManager.GetTemplateContent(template);

            var tileTextAttributes = tileXml.GetElementsByTagName("text");
#if WINDOWS_APP
            tileTextAttributes[0].InnerText = "Now playing";
#endif
            if (Locator.MusicPlayerVM.CurrentTrack != null)
            {
#if WINDOWS_APP
                tileTextAttributes[1].InnerText = Locator.MusicPlayerVM.CurrentTrack.Name + " - " + Locator.MusicPlayerVM.CurrentTrack.ArtistName;
#else
                tileTextAttributes[0].InnerText = Locator.MusicPlayerVM.CurrentTrack.Name ?? "";
                tileTextAttributes[1].InnerText = Locator.MusicPlayerVM.CurrentTrack.AlbumName;
                tileTextAttributes[1].InnerText = Locator.MusicPlayerVM.CurrentTrack.ArtistName;
#endif
                var tileImgAttribues = tileXml.GetElementsByTagName("image");
#if WINDOWS_APP
                if (Locator.MusicPlayerVM.CurrentArtist != null)
                    tileImgAttribues[0].Attributes[1].NodeValue = Locator.MusicPlayerVM.CurrentArtist.Picture;
                
                if (Locator.MusicPlayerVM.CurrentAlbum != null)
                    tileImgAttribues[1].Attributes[1].NodeValue = Locator.MusicPlayerVM.CurrentAlbum.Picture;
#else
                if (Locator.MusicPlayerVM.CurrentAlbum != null)
                    tileImgAttribues[0].Attributes[1].NodeValue = Locator.MusicPlayerVM.CurrentAlbum.Picture;
#endif
            }

            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public static void UpdateMediumTileWithVideoInfo()
        {
            LogHelper.Log("PLAYVIDEO: Updating Live Tile");
            const TileTemplateType template = TileTemplateType.TileSquare150x150PeekImageAndText02;
            var tileXml = TileUpdateManager.GetTemplateContent(template);
            var tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = "playing";
            if (Locator.VideoVm.CurrentVideo != null)
            {
                tileTextAttributes[1].InnerText = Locator.VideoVm.CurrentVideo.Title;

                var tileImgAttribues = tileXml.GetElementsByTagName("image");
                if (Locator.VideoVm.CurrentVideo != null)
                    tileImgAttribues[0].Attributes[1].NodeValue = "ms-appdata:///local/videoPic/" + Locator.VideoVm.CurrentVideo.Id + ".jpg";
            }
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            UpdateBigTileWithVideoInfo();
        }

        public static void UpdateBigTileWithVideoInfo()
        {
            const TileTemplateType template = TileTemplateType.TileWide310x150PeekImage05;
            var tileXml = TileUpdateManager.GetTemplateContent(template);
            var tileTextAttributes = tileXml.GetElementsByTagName("text");
#if WINDOWS_APP
            tileTextAttributes[0].InnerText = "Now playing";
#endif
            if (Locator.VideoVm.CurrentVideo != null)
            {
                tileTextAttributes[0].InnerText = Locator.VideoVm.CurrentVideo.Title;
                var tileImgAttribues = tileXml.GetElementsByTagName("image");
                if (Locator.VideoVm.CurrentVideo != null)
                    tileImgAttribues[0].Attributes[1].NodeValue = "ms-appdata:///local/videoPic/" + Locator.VideoVm.CurrentVideo.Id + ".jpg";
            }
            var tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public static async void CreateOrReplaceSecondaryTile(VLCItemType type, int id, string title)
        {
            string tileId = "SecondaryTile-" + type.ToString() + "-" + id;
            if (!SecondaryTile.Exists(tileId))
            {
                var tileData = new SecondaryTile()
                {
                    TileId = tileId,
                    DisplayName = title,
                    Arguments = tileId
                };
                string subfolder = null;
                switch (type)
                {
                    case VLCItemType.Album:
                        subfolder = "albumPic";
                        break;
                    case VLCItemType.Artist:
                        subfolder = "artistPic";
                        break;
                }
                tileData.VisualElements.ShowNameOnSquare150x150Logo = true;
                tileData.DisplayName = title;
                tileData.VisualElements.Square150x150Logo =
                    new Uri("ms-appdata:///local/" + subfolder + "/" + id + ".jpg");
                await tileData.RequestCreateAsync();
            }
        }
    }
}
