using RestSharp;

namespace AutoApply
{

    public static class DetectLanguage
    {
        // To Use, get an API key from: http://ws.detectlanguage.com
        public static bool IsEnglish(string testMe)
        {
            RestClient client = new RestClient("http://ws.detectlanguage.com");
            RestRequest request = new RestRequest("/0.2/detect", Method.POST);

            request.AddParameter("key", Config.DetectLanguageApiKey); // replace "demo" with your API key
            request.AddParameter("q", testMe);

            IRestResponse response = client.Execute(request);

            RestSharp.Deserializers.JsonDeserializer deserializer = new RestSharp.Deserializers.JsonDeserializer();
            try
            {
                var result = deserializer.Deserialize<Result>(response);
                Detection detection = result.Data.Detections[0];
                return detection.Language == "en" && detection.IsReliable;
            }
            catch
            {
                return false;
            }
        }
    }
}
