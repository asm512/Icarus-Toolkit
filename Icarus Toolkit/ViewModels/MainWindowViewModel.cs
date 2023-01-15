using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using Icarus;
using Avalonia.Threading;
using Avalonia.Controls;
using Icarus_Toolkit.Views;
using System.Threading.Tasks;

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

            GameData gameData = new(GamePath);

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
                IsWorking = false;
                InformationString = "Selected path was not a valid game data dir";
                SelectGameFolderButtonClicked();
            }
        }

        private void BackupData()
        {
            gameData.BackupData();
            InformationString = "Game Data Backed Up";
        }

        private void LoadSelectedCharacter()
        {
            IsWorking= true;
            SelectedCharacter = characterList[SelectedCharacterIndex];
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
            IsWorking= false;
        }

        private void ExportData()
        {
            IsProgressVisible = true;
            isWorking = true;
            SetCharacterValues();
            characterExplorerHandle.ExportCharacters(CharacterList);
            Progress = 40;
            SetProfileVales();
            profileExplorerHandle.ExportProfile(PlayerProfile);
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
            CharacterList[selectedCharacterIndex] = SelectedCharacter;
            InformationString = "Character values saved";
        }

        private void SetProfileVales()
        {
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
