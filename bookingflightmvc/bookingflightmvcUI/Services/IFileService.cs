namespace bookingflightmvcUI.Services
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
        Task<string> SaveFile(IFormFile file, string[] allowedExtensions);

        void DeleteFile(string fileName);
    }

}
