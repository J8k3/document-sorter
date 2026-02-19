using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace dcsrt
{
    public static class FileSystemUtil
    {
        public static IEnumerable<string> GetFilesBelowPath(string rootPath,
                                                            string searchPattern,
                                                            SearchOption searchOption)
        {
            List<string> filePaths = new List<string>();

            try
            {
                filePaths.AddRange(Directory.EnumerateFiles(rootPath, searchPattern, searchOption));
            }
            catch (Exception ex)
            {
                Program.Context.Logger.Log(LogLevel.Fatal, ex, "Failed to enumerate files in path [{0}].", rootPath);
                throw new AggregateException(ex);
            }

            return filePaths;
        }

        public static void MoveFile(FileInfo sourceFile, string destinationFilePath)
        {
            FileInfo destinationFile = new FileInfo(destinationFilePath);

            if (destinationFile.Directory.Exists == false)
            {
                Program.Context.Logger
                    .Log(LogLevel.Info,
                         "Destination directory [{0}] not found. Directory will be created.",
                         destinationFile.Directory.FullName);
                FileSystemUtil.RecursivelyCreateDirectory(destinationFile.Directory);
            }

            if (destinationFile.Exists)
            {
                Program.Context.Logger
                    .Log(LogLevel.Info, "File is already in the destination and does not need to move.");
            }
            else
            {
                sourceFile.MoveTo(destinationFile.FullName);
            }
        }

        public static void RecursivelyCreateDirectory(DirectoryInfo directory)
        {
            if ((directory != null) && (directory.Exists == false))
            {
                if ((directory.Parent != null) && (directory.Parent.Exists == false))
                {
                    FileSystemUtil.RecursivelyCreateDirectory(directory.Parent);
                }

                Program.Context.Logger.Log(LogLevel.Info, "Creating directory [{0}].", directory.FullName);
                directory.Create();
            }
        }
    }
}
