using Cinema.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.FileHelper
{
  public  class FileHelper
    {
        public static void JsonSerializationMovie(Movie movie)
        {
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter($"{movie.MovieName}.json"))
            {
                using (var jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Newtonsoft.Json.Formatting.Indented;
                    serializer.Serialize(jw, movie);
                }
            }
        }
        public static Movie JsonDeserializeMovie(string filename)
        {
            Movie movie = null;
            var serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader($"{filename}.json"))
            {
                using (var jr = new JsonTextReader(sr))
                {
                   movie = serializer.Deserialize<Movie>(jr);
                }
           
            }
            return movie;
        }

    }
    }
