using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarDownloader
{
    internal class Consts
    {
        public const string LogDataPath = "/app/logindata.json";
        public const string LogFilePath = "/app/out.log";
        public const string SaveImgDirPath = "/app/avatars";
        public readonly static TimeSpan WebRequestTimeout = new(0, 1, 0);// час на з'єднання - 1хв
        public readonly static Dictionary<Socials, string> SocialsURLs = new()
        {
            { Socials.Facebook, "https://www.facebook.com/" },
        };        
    }
    public enum Socials
    {
        None,
        Facebook,
    }
}
