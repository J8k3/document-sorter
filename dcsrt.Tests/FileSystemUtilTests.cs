using dcsrt;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(FileSystemUtil))]
    public class FileSystemUtilTests
    {
        [Test()]
        public void GetFilesBelowPathTest()
        {
            string path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources");
            IEnumerable<string> files = FileSystemUtil.GetFilesBelowPath(path, "*.pdf", SearchOption.TopDirectoryOnly);
            Assert.That(files.Count(), Is.EqualTo(1));
            Assert.That(File.Exists(files.First()), Is.True);
        }

        [Test()]
        public void GetFilesBelowPath_AllDirectories()
        {
            string path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources");
            IEnumerable<string> files = FileSystemUtil.GetFilesBelowPath(path, "*.pdf", SearchOption.AllDirectories);
            Assert.That(files, Is.Not.Null);
            Assert.That(files.Count(), Is.GreaterThanOrEqualTo(1));
        }

        [Test()]
        public void GetFilesBelowPath_InvalidPath_ThrowsException()
        {
            string path = "C:\\NonExistentPath123456789";
            Assert.Throws<AggregateException>(() => FileSystemUtil.GetFilesBelowPath(path, "*.pdf", SearchOption.TopDirectoryOnly));
        }

        [Test()]
        public void MoveFile_CreatesDestinationDirectory()
        {
            string testDir = Path.Combine(Path.GetTempPath(), "dcsrt_test_" + Path.GetRandomFileName());
            string sourceFile = Path.Combine(testDir, "source.txt");
            string destFile = Path.Combine(testDir, "subdir", "dest.txt");
            try
            {
                Directory.CreateDirectory(testDir);
                File.WriteAllText(sourceFile, "test content");
                FileSystemUtil.MoveFile(new FileInfo(sourceFile), destFile);
                Assert.That(File.Exists(destFile), Is.True);
                Assert.That(File.Exists(sourceFile), Is.False);
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Test()]
        public void MoveFile_FileAlreadyExists_DoesNotMove()
        {
            string testDir = Path.Combine(Path.GetTempPath(), "dcsrt_test_" + Path.GetRandomFileName());
            string sourceFile = Path.Combine(testDir, "source.txt");
            string destFile = Path.Combine(testDir, "dest.txt");
            try
            {
                Directory.CreateDirectory(testDir);
                File.WriteAllText(sourceFile, "source content");
                File.WriteAllText(destFile, "dest content");
                FileSystemUtil.MoveFile(new FileInfo(sourceFile), destFile);
                Assert.That(File.Exists(sourceFile), Is.True);
                Assert.That(File.ReadAllText(destFile), Is.EqualTo("dest content"));
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Test()]
        public void RecursivelyCreateDirectory_CreatesNestedDirectories()
        {
            string testDir = Path.Combine(Path.GetTempPath(), "dcsrt_test_" + Path.GetRandomFileName(), "level1", "level2", "level3");
            try
            {
                DirectoryInfo dir = new DirectoryInfo(testDir);
                FileSystemUtil.RecursivelyCreateDirectory(dir);
                Assert.That(Directory.Exists(testDir), Is.True);
            }
            finally
            {
                string rootDir = Path.Combine(Path.GetTempPath(), Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(testDir)))));
                if (Directory.Exists(rootDir))
                {
                    Directory.Delete(rootDir, true);
                }
            }
        }

        [Test()]
        public void RecursivelyCreateDirectory_ExistingDirectory_DoesNotThrow()
        {
            string testDir = Path.Combine(Path.GetTempPath(), "dcsrt_test_" + Path.GetRandomFileName());
            try
            {
                Directory.CreateDirectory(testDir);
                DirectoryInfo dir = new DirectoryInfo(testDir);
                Assert.DoesNotThrow(() => FileSystemUtil.RecursivelyCreateDirectory(dir));
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }
    }
}
