using LibFrame.Confs;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class FileService
    {
        private readonly FileConfigModel _fileConfig;
        public FileService(FileConfigModel fileConfig)
        {
            _fileConfig=fileConfig;
        }
        public async Task<string> SaveFile(string filepath, string fileName, byte[] fileBytes)
        {
            //string getFilePath = Path.Combine(filepath.Split('/'));
            string PhysicalPath = Path.Combine(_fileConfig.FileRootPath, filepath);
            Directory.CreateDirectory(PhysicalPath);
            //fileName = $"{Path.GetRandomFileName()}{fileName}";
            PhysicalPath = Path.Combine(PhysicalPath, fileName);
            using Stream str = System.IO.File.Create(PhysicalPath);
            await str.WriteAsync(fileBytes);
            str.Flush();

            return Path.Combine(_fileConfig.FileLocation, filepath, fileName);
        }

        public List<string> GetAllFileByFolder(string folder)
        {
            List<string> allfiles = new List<string>();
            folder = Path.Combine(_fileConfig.FileRootPath, folder);
            if (Directory.Exists(folder))
            {
                DirectoryInfo info = new DirectoryInfo(folder);
                FileInfo[] files = info.GetFiles();
                allfiles.AddRange(files.Select(file => file.FullName.ReplaceFirst(_fileConfig.FileRootPath, _fileConfig.FileLocation)));
                DirectoryInfo[] dires = info.GetDirectories();
                foreach (DirectoryInfo dire in dires)
                {
                    allfiles.AddRange(GetAllFileByFolder(dire.FullName));
                };
            }
            
            return allfiles;
        }

        public bool DeleteUserFile(string file)
        {
            file = file.ReplaceFirst(_fileConfig.FileLocation, _fileConfig.FileRootPath);
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }
            return false;
        }
    }
}
