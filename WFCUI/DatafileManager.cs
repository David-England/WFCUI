using System;
using System.IO;

namespace WFCUI
{
    internal class DatafileManager : IDisposable
    {
        public string Filepath { get; init; }
        public string FilenameNoExtension { get; init; }
        public string Extension { get; init; }
        
        internal DatafileManager(string filepath)
        {
            Filepath = filepath;
            FilenameNoExtension = Path.GetFileNameWithoutExtension(filepath);
            Extension = Path.GetExtension(filepath);
            Directory.SetCurrentDirectory(Path.GetDirectoryName(filepath)); // possibly not thread safe

            PopulateSamplesDirectory();
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete("samples", recursive: true);
            }
            catch (IOException)
            {
                return;
            }
        }

        private void PopulateSamplesDirectory()
        {
            Directory.CreateDirectory("samples");
            File.Copy($"{FilenameNoExtension}{Extension}", $"samples\\{FilenameNoExtension}{Extension}", overwrite: true);
        }
    }
}
