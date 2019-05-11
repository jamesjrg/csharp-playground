using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace newtonsoft_test
{
    class Foo
    {
        public List<JObject> Bar;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var x = JsonConvert.DeserializeObject<Foo>("{}");
            Console.WriteLine("Hello World!");
            Console.WriteLine(x.Bar);
        }
    }
}
