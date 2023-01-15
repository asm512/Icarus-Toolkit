using System;
using System.Linq;
using System.IO;

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
            return new CharacterExplorer(CharactersPath);
        }

        public ProfileExplorer GetProfile()
        {
            return new ProfileExplorer(ProfilePath);
        }

        private bool ValidateGamePath(string gamePath)
        {
            if (Directory.GetFiles(gamePath).Contains(CharactersFileName) && Directory.GetFiles(gamePath).Contains(ProfileFileName)) { return true; } return true;
        }
    }
}
