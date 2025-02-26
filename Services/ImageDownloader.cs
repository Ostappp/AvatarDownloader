namespace AvatarDownloader.Services
{
    internal class ImageDownloader
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task DownloadImageAsync(string imageUrl, string saveDirectoryPath = Consts.SaveImgDirPath)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("Receiving picture data...");
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                Console.WriteLine("Creating directory if needed...");
                Directory.CreateDirectory(saveDirectoryPath);

                Console.WriteLine("Writing picture data into file...");
                await File.WriteAllBytesAsync($"{saveDirectoryPath}/{Directory.GetFiles(saveDirectoryPath).Length}", imageBytes);

                Console.WriteLine("The image has been successfully uploaded and saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
            }

        }
    }
}
