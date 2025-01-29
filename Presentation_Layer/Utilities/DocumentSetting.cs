using System.Drawing;

namespace Presentation_Layer.Utilities
{
    public static class DocumentSetting
    {
        public async static Task<string> uploadFile(IFormFile file, string foldername)
        {
            string Folderpath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files",foldername);

            string Filename = $"{Guid.NewGuid()}-{file.FileName}";

            string FilePath= Path.Combine(Folderpath, Filename);

            using var FileStream = new FileStream(FilePath, FileMode.Create);

            await file.CopyToAsync(FileStream);

            return Filename;


        }

        public static void DeleteFile(string foldername, string filename)
        {
            string Filepath=Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot\Files",foldername, filename);

            if (File.Exists(Filepath))
            {
                File.Delete(Filepath);
            }


        }




    }
}
