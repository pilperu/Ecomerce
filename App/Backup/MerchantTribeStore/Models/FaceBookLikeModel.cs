using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Models
{
    public enum FaceBookVerb
    {
        Like = 0,
        Recommend = 1
    }
    public enum FaceBookLayout
    {
        Buttons = 0,
        Standard = 1,
        Box = 2
    }
    public enum FaceBookColorScheme
    {
        Light = 0,
        Dark = 1
    }

    public class FaceBookLikeModel
    {
        public FaceBookVerb Verb { get; set; }
        public FaceBookLayout Layout { get; set; }
        public FaceBookColorScheme Colors { get; set; }
        public bool IncludeSendButton { get; set; }
        public bool ShowFaces { get; set; }
        public int Width { get; set; }
        public string PageUrl { get; set; }
        
        public FaceBookLikeModel()
        {
            this.Verb = FaceBookVerb.Like;
            this.Layout = FaceBookLayout.Buttons;
            this.Colors = FaceBookColorScheme.Light;
            this.IncludeSendButton = true;
            this.ShowFaces = false;
            this.Width = 450;
            this.PageUrl = string.Empty;
        }
    }
}