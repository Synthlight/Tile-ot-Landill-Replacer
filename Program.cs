using System;
using System.IO;
using Ionic.Zlib;

namespace Blueprint_Editor {
    public class Program {
        private const string BP_FILE = "bp.txt";

        public static void Main(string[] args) {
            if (!File.Exists(BP_FILE)) return;

            using (var bpFile = new FileStream(BP_FILE, FileMode.Open)) {
                string base64BpString;

                using (var reader = new StreamReader(bpFile)) {
                    base64BpString = reader.ReadToEnd().Trim();
                }

                base64BpString = base64BpString.Substring(1);

                var json = BpStringToJson(base64BpString);

                json = json.Replace("stone-path", "landfill");
                json = json.Replace("refined-hazard-concrete", "landfill");
                json = json.Replace("refined-concrete", "landfill");
                json = json.Replace("hazard-concrete", "landfill");
                json = json.Replace("concrete", "landfill");

                base64BpString = "0" + JsonToBpString(json);

                File.WriteAllText(BP_FILE, base64BpString);
            }
        }

        private static string BpStringToJson(string base64BpString) {
            var zlibBpString = Convert.FromBase64String(base64BpString);
            var json = ZlibStream.UncompressString(zlibBpString);
            return json;
        }

        private static string JsonToBpString(string json) {
            var zlibBpString = ZlibStream.CompressString(json);
            var base64BpString = Convert.ToBase64String(zlibBpString);
            return base64BpString;
        }
    }
}