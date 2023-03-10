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

        public static bool ValidateGamePath(string gamePath)
        {
            if (gamePath == "") { return false; }
            var files = from f in Directory.EnumerateFiles(gamePath, "*.json")
                        select Path.GetFileName(f);

            if(files.Contains(CharactersFileName) && files.Contains(ProfileFileName)) { return true;  } return false;   
        }

        public bool BackupData()
        {
            var currentBackupPath = Path.Combine(BackupPath, $"{DateTime.Now.ToString().Replace(":","-")}");
            try
            {
                if(Directory.Exists(currentBackupPath))
                {
                    Directory.Delete(currentBackupPath);
                }
                Directory.CreateDirectory(currentBackupPath);

                foreach (var file in Directory.GetFiles(GameDataPath))
                {
                    File.Copy(file, Path.Combine(currentBackupPath, Path.GetFileName(file)));
                }
                Log.Information($"Backup created at {currentBackupPath}");
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
