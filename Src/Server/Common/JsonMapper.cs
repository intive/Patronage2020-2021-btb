using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BTB.Server.Common
{
    public class JsonMapper
    {
        public readonly string CurrentFileAsString;

        public JsonMapper(string jsonTextFromFile)
        {
            CurrentFileAsString = jsonTextFromFile;
        }

        public T MapSection<T>() 
        {
            return MapSection<T>(typeof(T).Name);
        }

        private T MapSection<T>(string sectionName)
        {
            var jsonObject = JObject.Parse(CurrentFileAsString);
            var section = jsonObject[sectionName];

            T returnedObject = JsonConvert.DeserializeObject<T>(section.ToString());

            return returnedObject;
        }
    }
}
