using Icarus;
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
            var PlayerProfile = gameData.GetProfile();

            //PrintProperties(charactersData.Characters, true);

            //PrintProperties(charactersData.Characters[0].Talents, true);

            //var SortedList = PlayerProfile.PlayerProfile.Talents.OrderBy(t => t.RowName).ToList();

            List<Talent> talentData = PlayerProfile.PlayerProfile.Talents.Where(x => x.RowName.ToLower().Contains("prospect")).Select(x => x).ToList();

            PrintProperties(talentData, true);


            Console.WriteLine();

            //PrintProperties(PlayerProfile.PlayerProfile);
            //PrintProperties(PlayerProfile.PlayerProfile.MetaResources, true);
            Console.WriteLine();

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
