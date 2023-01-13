using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Icarus;
using System.Net.Http.Headers;

namespace Icarus
{
    public class GameData
    {
        public readonly string GameDataPath;
        public readonly string CharactersPath;
        public readonly string ProfilePath;

        internal const string CharactersFileName = "Characters.json";
        internal const string ProfileFileName = "Profile.json";

        public GameData(string gameDataPath)
        {
            if (!ValidateGamePath(gameDataPath)) { throw new Exception("Directory was not a valid game data path"); }
            GameDataPath = gameDataPath;
            CharactersPath = Path.Combine(GameDataPath, CharactersFileName);
            ProfilePath = Path.Combine(GameDataPath, ProfileFileName);
        }

        public CharacterExplorer GetCharacters()
        {
            return new CharacterExplorer(File.ReadAllText(CharactersPath));
        }

        public Profile GetProfile()
        {
            return new ProfileExplorer(File.ReadAllText(ProfilePath)).PlayerProfile;
        }

        private bool ValidateGamePath(string gamePath)
        {
            if (Directory.GetFiles(gamePath).Contains(CharactersFileName) && Directory.GetFiles(gamePath).Contains(ProfileFileName)) { return true; } return true;
        }
    }
}
