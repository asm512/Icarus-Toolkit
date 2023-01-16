using System;
using System.Collections.Generic;
using Icarus;
using Avalonia.Threading;
using Avalonia.Controls;
using System.Threading.Tasks;
using Serilog;
using Icarus_Toolkit.Views;
using CommunityToolkit.Mvvm.ComponentModel;


namespace Icarus_Toolkit.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region Core & UI
        [ObservableProperty]
        private string gamePath;

        private GameData gameData;

        [ObservableProperty]
        private bool validGamePath = false;

        [ObservableProperty]
        private bool isWorking = false;

        private bool editMode;
        private bool EditMode
        {
            get => editMode;
            set
            {
                if (!WasCharacterExported)
                {
                    LoadSelectedCharacter();
                }
                editMode = value;
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private string loadText = "Load Character Data";

        private string informationString;
        private string InformationString
        {
            get => informationString;
            set
            {
                informationString = value;
                OnPropertyChanged();
                DisplayInformationString();
            }
        }

        [ObservableProperty]
        private bool isInformationStringVisible;

        [ObservableProperty]
        private DispatcherTimer informationStringTimer = new();

        [ObservableProperty]
        private int progress;

        [ObservableProperty]
        private bool isProgressVisible;

        #endregion

        #region Character

        private CharacterExplorer characterExplorerHandle;

        [ObservableProperty]
        private List<Character> characterList;

        private int selectedCharacterIndex;
        private int SelectedCharacterIndex
        {
            get => selectedCharacterIndex;
            set
            {
                selectedCharacterIndex = value;
                OnPropertyChanged();
                if (SelectedCharacterIndex != CurrentLoadedCharacterIndex) { LoadText = "Load Character Data*"; }
                else { LoadText = "Load Character Data"; }
            }
        }

        [ObservableProperty]
        private Character selectedCharacter;

        [ObservableProperty]
        private int selectedCharacterLevel;

        [ObservableProperty]
        private string characterDisplayName;

        [ObservableProperty]
        private bool isCharacterLoaded = false;

        [ObservableProperty]
        private bool wasCharacterExported;

        private int currentLoadedCharacterIndex;
        private int CurrentLoadedCharacterIndex
        {
            get => currentLoadedCharacterIndex;
            set
            {
                currentLoadedCharacterIndex = value;
                OnPropertyChanged();
                LoadText = "Load Character Data";
            }
        }

        public static int MaxXP => 5400001;

        [ObservableProperty]
        private int xPLevel;

        #region UserEdits

        private int editedXP;
        public int EditedXP
        {
            get => editedXP;
            set
            {
                editedXP = value;
                OnPropertyChanged();
                XPLevel = Core.GetPlayerLevel(editedXP);
            }
        }

        [ObservableProperty]
        private string editedName;
        #endregion
        #endregion

        #region Profile

        private ProfileExplorer profileExplorerHandle;

        [ObservableProperty]
        public Profile playerProfile;

        [ObservableProperty]
        private int maximumRen = 10000;

        [ObservableProperty]
        private int maximumExotic = 10000;

        #region UserEdits

        [ObservableProperty]
        private int ren;

        [ObservableProperty]
        private int exotic;
        #endregion
        #endregion

        public async Task<string> GetFolderFromUser(string title)
        {
            var dialog = new OpenFolderDialog()
            {
                Title = title
            };
            var result = await dialog.ShowAsync(MainWindow.MainWindowHandle);
            if (result != null)
            {
                return result;
            }
            return await GetFolderFromUser(title);
        }

        public async Task SelectGameFolder()
        {            
            ValidGamePath = false;
            var selectedPath = await GetFolderFromUser("Select Game Data Folder");
            GamePath = selectedPath;
            ReloadGameData();
        }

        public Task SelectGameFolderButtonClicked() => SelectGameFolder();

        public void ReloadGameData()
        {
            IsWorking = true;

            Log.Information($"Reloading game data from {GamePath}");
            gameData = new(GamePath);

            if(gameData.ValidGamePath)
            {
                characterExplorerHandle = gameData.GetCharacters();
                CharacterList = characterExplorerHandle.Characters;

                profileExplorerHandle = gameData.GetProfile();
                PlayerProfile = gameData.GetProfile().PlayerProfile;

                SelectedCharacterIndex = 0;
                ValidGamePath = true;
                InformationString = "Game data loaded";
                IsWorking = false;
            }
            else
            {
                Log.Error($"{GamePath} was not a valid game data path");
                IsWorking = false;
                InformationString = "Selected path was not a valid game data dir";
                SelectGameFolderButtonClicked();
            }
        }

        private void BackupData()
        {
            bool success = gameData.BackupData();
            if (success)
            {
                InformationString = "Game Data Backed Up";
                return;
            }
            else
            {
                InformationString = "Failed to backup data, check log for more details";
            }
        }

        private void LoadSelectedCharacter()
        {
            IsWorking= true;
            SelectedCharacter = characterList[SelectedCharacterIndex];
            Log.Information($"Reloading {SelectedCharacter.CharacterName}");
            SelectedCharacterLevel = Core.GetPlayerLevel(selectedCharacter.XP);
            CharacterDisplayName = $"{selectedCharacter.CharacterName} (Level {SelectedCharacterLevel})";

            #region Character
            EditedXP = selectedCharacter.XP;
            EditedName = selectedCharacter.CharacterName;
            #endregion
            #region Profile
            Ren = PlayerProfile.MetaResources[0].Count;
            Exotic = PlayerProfile.MetaResources[1].Count;
            #endregion
            CurrentLoadedCharacterIndex = SelectedCharacterIndex;
            InformationString = $"{CharacterDisplayName} Loaded";
            IsCharacterLoaded = true;
            Log.Information($"{SelectedCharacter.CharacterName} reloaded");
            IsWorking= false;
        }

        private void ExportData()
        {
            Log.Information("Exporting data");
            IsProgressVisible = true;
            isWorking = true;
            SetCharacterValues();
            bool successCharacter = characterExplorerHandle.ExportCharacters(characterList);
            if(!successCharacter)
            {
                InformationString = "Characters data failed to export";
            }
            Progress = 40;
            SetProfileVales();
            bool profileSuccess = profileExplorerHandle.ExportProfile(playerProfile);
            if(!profileSuccess)
            {
                InformationString = "Profile failed to export";
            }
            Progress = 80;
            ReloadCharacter();
            InformationString = $"User #{PlayerProfile.UserID} | {selectedCharacter.CharacterName} was saved";

            Progress = 100;
            WasCharacterExported = true;
            isWorking= false;
            IsProgressVisible= false;
        }

        private void SetCharacterValues()
        {
            selectedCharacter.CharacterName = editedName;
            SelectedCharacter.XP = EditedXP;
            Log.Information("Setting character values {@SelectedCharacter}", SelectedCharacter);
            CharacterList[selectedCharacterIndex] = SelectedCharacter;
            InformationString = "Character values saved";
        }

        private void SetProfileVales()
        {
            Log.Information("Setting profile values {@MetaResources}", PlayerProfile.MetaResources);
            PlayerProfile.MetaResources[0].Count = Ren;
            PlayerProfile.MetaResources[1].Count = Exotic;
            InformationString = "Ren & Exotic values saved";
        }

        private void ReloadCharacter() => LoadSelectedCharacter();

        private void DisplayInformationString()
        {
            int seconds = InformationString.Length / 3;
            informationStringTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            informationStringTimer.Tick += (s, e) =>
            {
                IsInformationStringVisible = false;
                informationStringTimer.Stop();
                OnPropertyChanged(nameof(InformationStringTimer));
            };
            IsInformationStringVisible = true;
            informationStringTimer.Start();
            OnPropertyChanged(nameof(InformationStringTimer));
        }
    }
}
