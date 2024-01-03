namespace HelpingHands_V2.Services
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using dotenv.net;
    using NuGet.Protocol;

    public class CloudinaryService
    {
        private Cloudinary _cloudinary;
        public CloudinaryService()
        {

            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;
            _cloudinary = cloudinary;
        }
        public async Task<UploadResult> UploadToCloudinary(IFormFile file, string public_id)
        {
            using var stream = file.OpenReadStream();
            ImageUploadParams uploadParamas = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "Helping Hands",
                PublicId = public_id,

            };
            try
            {
                _cloudinary.Api.Secure = true;
                UploadResult uploadResult = await _cloudinary.UploadAsync(uploadParamas);
                return uploadResult;
            }
            catch
            {
                throw new Exception("System could not save image to the cloud");
            }
        }

        public async Task<DeletionResult> RemoveFromCloudinary(string public_id)
        {
            DeletionParams deletionParams = new DeletionParams(public_id)
            {
                PublicId = public_id,
                ResourceType = ResourceType.Image,
            };
            try
            {
                _cloudinary.Api.Secure = true;
                DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);
                return deletionResult;
            }
            catch
            {
                throw new Exception("System could not delete image from the cloud");
            }

        }
    }
}
