namespace TestDocker.Service
{
    public class FileService
    {
        public List<string> GetNewerFiles(string folderPath,string fileName)
        {
            string targetFilePath=Path.Combine(folderPath,fileName);
            if(!File.Exists(targetFilePath))
                throw new FileNotFoundException($"File {targetFilePath} does not exist");

            DateTime targetTime=File.GetLastWriteTime(targetFilePath).ToUniversalTime();
            var newerFiles=new List<string>();
            foreach(string file in Directory.GetFiles(folderPath,fileName))
            {
                DateTime fileTime=File.GetLastWriteTime(file).ToUniversalTime();
                if(file!=targetFilePath && fileTime > targetTime) { }
                    newerFiles.Add(file);
            }
            return newerFiles;
        }
    }
}
