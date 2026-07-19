using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Settings
{
    public class FileStorageSettings
    {
        public int ImageSizeInMB { get; set; }

        public int VideoSizeInMB { get; set; }

        public int FileSizeInMB { get; set; }

        public string UploadsFolder { get; set; } = null!;

        public string ImagesFolder { get; set; } = null!;

        public string VideosFolder { get; set; } = null!;

        public string FilesFolder { get; set; } = null!;

      
        public string[] ImageExtensionAllowed { get; set; } = null!;

      
        public string[] FileExtensionAllowed { get; set; } = null!;

      
        public string[] VideoExtensionAllowed { get; set; } = null!;

      
        public int MaxFilesPerUpload { get; set; } = 5;
    }
}
