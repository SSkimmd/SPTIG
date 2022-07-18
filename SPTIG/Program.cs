using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SPTIG {
    internal class Program {
        List<Item> Items = new List<Item>();

        static void Main(string[] args) {
            Program SPTIG = new Program();

            if(args.Length > 0) {
                SPTIG.GetItems(args[0]);
            } else {
                Console.WriteLine("Enter items.json path");
                string path = Console.ReadLine();
                SPTIG.GetItems(path);
            }
        }

        private void GetItems(string path) {
            string savepath = Directory.GetCurrentDirectory() + "/itemsout.json";
            if(!File.Exists(savepath)) { File.Create(savepath).Dispose(); }

            if(File.Exists(path)) {
                using (var sreader = new StreamReader(path))
                using (var jreader = new JsonTextReader(sreader)) {
                    JToken root = null;

                    try {
                        root = JToken.Load(jreader);
                    } catch (Exception e) {
                        Console.WriteLine(e);
                    }

                    int count = 0;
                    foreach(var obj in root.Children().Children()) {                        
                        if(obj["_props"].HasValues) {
                            count++;

                            string id = "" + obj["_id"];
                            string name = "" + obj["_props"]["Name"];

                            Items.Add(new Item(id, name));
                        }
                    }

                    Console.WriteLine("Got Data From: " + count + " Objects");
                    Console.WriteLine("Data Stored To: " + savepath.Replace("\\", "/"));
                }
            }

            File.WriteAllText(savepath, JsonConvert.SerializeObject(Items, Formatting.Indented));
            Console.Read();
        }
    }

    public class Item {
        public string ID { get; set; }
        public string Name { get; set; }

        public Item(string ID, string Name) {
            this.ID = ID;
            this.Name = Name;
        }
    }
}
