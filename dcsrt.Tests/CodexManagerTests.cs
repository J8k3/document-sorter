using dcsrt;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;

namespace dcsrt.Tests
{
    [TestFixture(TestOf = typeof(CodexManager))]
    public class CodexManagerTests
    {
        [Test()]
        public void GetDefaultCodexPath_ReturnsValidPath()
        {
            string path = CodexManager.GetDefaultCodexPath();
            Assert.That(path, Is.Not.Null);
            Assert.That(path, Does.EndWith("Codex.json"));
        }

        [Test()]
        public void Reload_InvalidPath_ThrowsFileNotFoundException()
        {
            string invalidPath = "C:\\NonExistent\\Codex.json";
            Assert.Throws<FileNotFoundException>(() => CodexManager.Reload(invalidPath));
        }

        [Test()]
        public void Reload_ValidPath_LoadsCodex()
        {
            string testFile = Path.Combine(Path.GetTempPath(), "test_codex_" + Path.GetRandomFileName() + ".json");
            try
            {
                CodexDictionary testCodex = new CodexDictionary
                {
                    RootPath = "C:\\Test",
                    Dictionary = new[]
                    {
                        new Codex { RuleId = "TestRule", Destination = "TestDest", Words = new[] { "test" } }
                    }
                };
                string json = JsonConvert.SerializeObject(testCodex);
                File.WriteAllText(testFile, json);
                CodexManager.Reload(testFile);
                Assert.That(CodexManager.Codex, Is.Not.Null);
                Assert.That(CodexManager.Codex.RootPath, Is.EqualTo("C:\\Test"));
                Assert.That(CodexManager.CodexFilePath, Is.EqualTo(testFile));
            }
            finally
            {
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Test()]
        public void Save_CreatesFile()
        {
            string testFile = Path.Combine(Path.GetTempPath(), "test_codex_save_" + Path.GetRandomFileName() + ".json");
            try
            {
                CodexDictionary testCodex = new CodexDictionary
                {
                    RootPath = "C:\\SaveTest",
                    Dictionary = new[]
                    {
                        new Codex { RuleId = "SaveRule", Destination = "SaveDest", Words = new[] { "save" } }
                    }
                };
                string json = JsonConvert.SerializeObject(testCodex);
                File.WriteAllText(testFile, json);
                CodexManager.Reload(testFile);
                CodexManager.Save();
                Assert.That(File.Exists(testFile), Is.True);
                string savedContent = File.ReadAllText(testFile);
                Assert.That(savedContent, Does.Contain("SaveTest"));
            }
            finally
            {
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }
    }
}
