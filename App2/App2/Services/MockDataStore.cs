using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App2.Models;
using App2.Views;
using Newtonsoft.Json;

namespace App2.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;
        //sample data
        public MockDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Title = "1,2,3,4", Description="Player 1 coordinates are 2,1. Player 2 coordinates are 4,3" },
                new Item { Id = Guid.NewGuid().ToString(), Title = "0,0,0,0", Description="Player 1 coordinates are 0,0. Player 2 coordinates are 0,0" },
                new Item { Id = Guid.NewGuid().ToString(), Title = "9,6,5,2", Description="Player 1 coordinates are 6,9. Player 2 coordinates are 2,5" },
                new Item { Id = Guid.NewGuid().ToString(), Title = "3,6,7,2", Description="Player 1 coordinates are 6,3. Player 2 coordinates are 2,7" },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }
        //delete methods not working
        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteItemAsync(Item item)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(item);

            return await Task.FromResult(true);
        }
        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
        //reading from json default data works,not using it because saving new data does not.
      /*  public static void SaveListData()
        {
            // need the path to the file
            string path = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, "TextFile1.txt");
            // use a stream writer to write the list
            using (var writer = new StreamWriter(filename, false))
            {
                // stringify equivalent
                //string jsonText = JsonConvert.SerializeObject("wellllllll");
                //writer.WriteLine(jsonText);
                string jsonText = JsonConvert.SerializeObject(items);
                writer.WriteLine(jsonText);
            }
        }*/
        //can not get the try to find the file, always reads default so I left it in but unused.
        public static ObservableCollection<Item> ReadListData()
        {
            ObservableCollection<Item> myList = new ObservableCollection<Item>();
            string jsonText;

            try  // reading the localApplicationFolder first
            {
                string path = Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, "SnakesAndLadders.txt");
                using (var reader = new StreamReader(filename))
                {
                    jsonText = reader.ReadToEnd();
                    // need json library
                }
            }
            catch // fallback is to read the default file
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(
                                                typeof(MainPage)).Assembly;
                // create the stream
                Stream stream = assembly.GetManifestResourceStream(
                                    "App2.Data.SnakesAndLadders.txt");
                using (var reader = new StreamReader(stream))
                {
                    jsonText = reader.ReadToEnd();
                    // include JSON library now
                }
            }

            myList = JsonConvert.DeserializeObject<ObservableCollection<Item>>(jsonText);

            return myList;
        }
    }

}
