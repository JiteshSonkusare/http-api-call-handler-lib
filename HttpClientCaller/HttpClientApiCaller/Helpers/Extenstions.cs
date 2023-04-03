using Newtonsoft.Json;

namespace HttpClientApiCaller.Helpers
{
    public static class Extenstions
    {
        private static readonly JsonSerializer serializer;

        static Extenstions()
        {
            serializer = JsonSerializer.CreateDefault();
            serializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public static string ToJson(this object obj)
        {
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }
    }
}