namespace RicKit.MakeEditorTools
{
    public static class IOHelper
    {
        public static void MakeSurePath(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
        public static void OpenFolder(string path)
        {
            path = path.Replace("/", "\\");
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
        public static void SaveFile(string path, string json)
        {
            System.IO.File.WriteAllText(path, json);
        }

        public static bool IsFileExist(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static string ReadFile(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
    }
}