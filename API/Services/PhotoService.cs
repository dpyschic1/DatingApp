using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
            (     //Create an account object with our cloudinary configrations as parameters
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            
            _cloudinary = new Cloudinary(acc); // set the cloudinary private field with the Cloudinary object using account object as parameter
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if(file.Length > 0) //check if file is empty or not
            {
                using var stream = file.OpenReadStream(); // use the using statement so that the object is disposed of after use and memory is freed
                var uploadParams = new ImageUploadParams // create the parameters for image upload
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"), //Transform the image so it is a square and cropped centered on the face
                    Folder = "da-net7"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams); //upload the files using above parameters
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}