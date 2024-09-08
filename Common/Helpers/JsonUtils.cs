using Newtonsoft.Json;
using System.IO;

namespace JehreeDevTools.Common
{
    internal class JsonUtils
    {
        public static T GetDataFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string CreateJsonFromData<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static string GetPath(string fileName, string subDirectory)
        {
            string fileDirectory = Path.Combine(Path.GetDirectoryName(Plugin.AssemblyPath), subDirectory);
            string filePath = Path.Combine(fileDirectory, fileName);
            return filePath;
        }
    }
}
