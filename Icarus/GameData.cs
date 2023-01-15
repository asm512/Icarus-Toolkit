using System;
using System.Linq;
using System.IO;

namespace Icarus
{
    public class GameData
    {
        internal readonly string GameDataPath;
        internal readonly string CharactersPath;
        internal readonly string ProfilePath;
        internal readonly string BackupPath;

        internal const string CharactersFileName = "Characters.json";
        internal const string ProfileFileName = "Profile.json";
        internal const string BackupFolder = "backups";

        public readonly bool ValidGamePath = false;

        public GameData(string gameDataPath)
        {
            CharactersPath = Path.Combine(gameDataPath, CharactersFileName);
            ProfilePath = Path.Combine(gameDataPath, ProfileFileName);
            if (!ValidateGamePath(gameDataPath)) { ValidGamePath = false; return;/*throw new Exception("Directory was not a valid game data path");*/ }
            ValidGamePath = true;
            GameDataPath = gameDataPath;
            CharactersPath = Path.Combine(GameDataPath, CharactersFileName);
            ProfilePath = Path.Combine(GameDataPath, ProfileFileName);
            BackupPath = Path.Combine(Directory.GetCurrentDirectory(), BackupFolder);
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
            if (Directory.GetFiles(gamePath).Contains(CharactersPath) && Directory.GetFiles(gamePath).Contains(ProfilePath)) { return true; } return false;
        }

        public void BackupData()
        {
            if (Directory.Exists(BackupPath)) { Directory.Delete(BackupPath); }
            Directory.CreateDirectory(BackupPath);

            foreach (var file in Directory.GetFiles(GameDataPath))
            {
                File.Copy(file, Path.Combine(BackupPath, Path.GetFileName(file)));
            }
        }
    }
}
