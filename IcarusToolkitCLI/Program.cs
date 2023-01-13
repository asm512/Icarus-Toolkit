using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcarusToolkitCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gameData = new Icarus.GameData(Path.Combine(Directory.GetCurrentDirectory(), "sandbox"));
            var charactersData = gameData.GetCharacters();

            PrintProperties(charactersData.Characters, true);


            Console.WriteLine();

            var PlayerProfile = gameData.GetProfile();


            PrintProperties(PlayerProfile);
            PrintProperties(PlayerProfile.MetaResources, true);


            KeepOpen();
        }

        public static void PrintProperties(object obj, bool deconstruct = false)
        {
            if(deconstruct)
            {
                foreach (var o in obj as IEnumerable<object>)
                {
                    PrintProperties(o);
                }
            }
            else
            {
                foreach (var o in obj.GetType().GetProperties())
                {
                    Console.WriteLine($"{o.Name} = {o.GetValue(obj, null)}");
                }
            }
        }

        private static void KeepOpen()
        {
            while (true) { }
        }
    }
}
