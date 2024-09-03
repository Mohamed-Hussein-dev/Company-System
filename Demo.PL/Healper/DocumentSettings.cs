namespace Demo.PL.Healper
{
    public static class DocumentSettings
    {
        public static string UplodeFile(IFormFile file , string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files", FolderName);
            string FileName = $"{Guid.NewGuid()}{file.FileName}";
            string FilePath = Path.Combine(FolderPath,FileName);
           using var FileStream = new FileStream(FilePath , FileMode.Create);
            file.CopyTo(FileStream);
            return FileName;
        }

        public static void DeleteFile(string fileName, string FolderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

   
}
