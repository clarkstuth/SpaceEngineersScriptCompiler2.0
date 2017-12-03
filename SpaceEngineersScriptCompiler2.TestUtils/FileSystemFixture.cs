using System;
using System.IO;

namespace SpaceEngineersScriptCompiler2.TestUtils
{
    public class FileSystemFixture
    {
        private readonly DirectoryInfo testDirectory =
            new DirectoryInfo(Path.GetTempPath() + "SpaceEngineersScriptCompilerTests" + DateTime.Now.ToBinary());

        /// <summary>
        /// Create a new test directory with a random name, then return a reference to it.
        /// </summary>
        /// <returns></returns>
        public DirectoryInfo CreateTestDirectory()
        {
            testDirectory.Create();
            return testDirectory;
        }

        /// <summary>
        /// Deletes the test directory and any files and folders underneath
        /// </summary>
        public void RemoveTestDirectory()
        {
            CleanDir(testDirectory);
        }

        private static void CleanDir(DirectoryInfo dir)
        {
            foreach (var directoryInfo in dir.GetDirectories())
                CleanDir(directoryInfo);

            foreach (var fileInfo in dir.GetFiles())
                fileInfo.Delete();

            dir.Delete();
        }

        /// <summary>
        /// Create the given FileInfo and write the provided content to it.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="content"></param>
        public FileInfo WriteTestFile(string filepath, string content)
        {
            var file = new FileInfo(testDirectory.FullName + Path.DirectorySeparatorChar + filepath);

            using (var stream = file.Create())
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(content);
            }

            return file;
        }

        /// <summary>
        /// Generates a FileInfo from a provided file builder.
        /// </summary>
        /// <param name="fileBuilder"></param>
        /// <returns></returns>
        public FileInfo WriteTestFile(FileBuilder fileBuilder)
        {
            return WriteTestFile(fileBuilder.FileName, fileBuilder.GetContents());
        }


    }
}