using System;
using System.Linq;
using System.IO;
using Serilog;

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
            Utils.InitLog();
            CharactersPath = Path.Combine(gameDataPath, CharactersFileName);
            ProfilePath = Path.Combine(gameDataPath, ProfileFileName);
            if (!ValidateGamePath(gameDataPath)) { ValidGamePath = false; Log.Warning($"{gameDataPath} was not a valid game data path"); return; }
            ValidGamePath = true;
            GameDataPath = gameDataPath;
            Log.Information($"Game data path set to {GameDataPath}");
            CharactersPath = Path.Combine(GameDataPath, CharactersFileName);
            Log.Information($"Character file set to {CharactersPath}");
            ProfilePath = Path.Combine(GameDataPath, ProfileFileName);
            Log.Information($"Profile file set to {ProfilePath}");
            BackupPath = Path.Combine(Directory.GetCurrentDirectory(), BackupFolder);
            Log.Information($"Backup folder set to {BackupPath}");
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

        public bool BackupData()
        {
            try
            {
                if(Directory.Exists(BackupPath))
                {
                    Directory.Delete(BackupPath);
                }
                Directory.CreateDirectory(BackupPath);

                foreach (var file in Directory.GetFiles(GameDataPath))
                {
                    File.Copy(file, Path.Combine(BackupPath, Path.GetFileName(file)));
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return false;
            }
        }
    }
}
