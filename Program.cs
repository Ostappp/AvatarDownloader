using AvatarDownloader.Services;
using AvatarDownloader.SocialsAvatars;

namespace AvatarDownloader
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Program started...");
            using (StreamWriter logFile = new(Consts.LogFilePath, true))
            {
                TeeTextWriter teeWriter = new(Console.Out, logFile);
                Console.SetOut(teeWriter);
                Console.SetError(teeWriter);

                while (true)
                {
                    (string login, string pass) = await GetLogInData();
                    FBook fBook = new();
                    string avatarURL = await fBook.GetProfileLogoURL(login, pass);
                    await ImageDownloader.DownloadImageAsync(avatarURL);
                    Console.WriteLine("Press key 'q' to quit or any other key to start again");
                    ConsoleKeyInfo insertedKey = Console.ReadKey(); 
                    if(insertedKey.KeyChar == 'q')
                    {
                        Environment.Exit(0);
                    }
                        
                }
            }
        }
        static async Task<(string login, string pass)> GetLogInData()
        {
            string login = string.Empty;
            string pass = string.Empty;

            while (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(pass))
            {
                Console.WriteLine("Input your login to Facebook:");
                login = "john.j83449152@gmail.com";// Console.ReadLine();
                Console.WriteLine(login);

                Console.WriteLine("Input your pass to Facebook:");
                pass = "johnthetester";//Console.ReadLine();
                Console.WriteLine(pass);
            }



            return (login, pass);
        }
    }
}
