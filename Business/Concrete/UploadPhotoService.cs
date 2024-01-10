using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Business.Concrete
{
    public class UploadPhotoService
    {
        private string ResizePicture(FileStream stream, string picturePath)
        {
            Bitmap image = new(stream);

            RotatePicture(image);

            double aspectRatio = (double)image.Width / (double)image.Height;
            double newHeight = aspectRatio * (double)400;
            Size size = new((int)newHeight, 400);
            Bitmap resizedImage = new Bitmap(image, size);
            var newPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img/profilePictures/", picturePath);
            resizedImage.Save(newPath);
            image.Dispose();
            resizedImage.Dispose();

            return $"/img/profilePictures/{picturePath}"; 
        }

		private void RotatePicture(Bitmap bmp)
		{
			PropertyItem pi = bmp.PropertyItems.Select(x => x).FirstOrDefault(x => x.Id == 0x0112);
			if (pi == null) return;

			byte o = pi.Value[0];

			if (o == 2) bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
			if (o == 3) bmp.RotateFlip(RotateFlipType.RotateNoneFlipXY);
			if (o == 4) bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			if (o == 5) bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
			if (o == 6) bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
			if (o == 7) bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
			if (o == 8) bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
		}

		public string UploadProfilePicture(IFormFile uploadedImage)
        {
            if (uploadedImage != null)
            {
                if (Path.GetExtension(uploadedImage.FileName).ToLower() == ".jpeg" || Path.GetExtension(uploadedImage.FileName).ToLower() == ".jpg" || Path.GetExtension(uploadedImage.FileName).ToLower() == ".png")
                {
                    var extension = Path.GetExtension(uploadedImage.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                   // string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/profilePictures/temp", newImageName);
                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/profilePictures/temp");
					string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/profilePictures/temp", newImageName);

					if (!Directory.Exists(directoryPath)) 
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var stream = new FileStream(filePath, FileMode.Create);
                    uploadedImage.CopyTo(stream);

                    string pathForProfilePic = ResizePicture(stream, newImageName);               

                    stream.Close();
                    FileInfo firstImage = new FileInfo(filePath);
                    firstImage.Delete();
                    return pathForProfilePic;
                }
            }
            return null;
        }

        public string UploadPostPicture(IFormFile uploadedImage)
        {
            return null;
        }
    }
}
