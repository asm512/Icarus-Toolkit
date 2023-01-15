using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Icarus_Toolkit.Views;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using System.IO;
using Icarus;
using Avalonia.Threading;

namespace Icarus_Toolkit.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region Core & UI
        [ObservableProperty]
        public string gamePath = Path.Combine(Directory.GetCurrentDirectory(), "sandbox");

        private GameData gameData;

        [ObservableProperty]
        private bool validGamePath = true;

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

        private int xP;
        public int XP
        {
            get => xP;
            set
            {
                xP = value;
                OnPropertyChanged();
                XPLevel = Core.GetPlayerLevel(xP);
            }
        }
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


        public void ConfirmPath()
        {
            ReloadGameData();
        }

        public void ReloadGameData()
        {
            gameData = new GameData(gamePath);

            characterExplorerHandle = gameData.GetCharacters();
            CharacterList = characterExplorerHandle.Characters;

            profileExplorerHandle = gameData.GetProfile();
            PlayerProfile = gameData.GetProfile().PlayerProfile;

            SelectedCharacterIndex = 0;
            ValidGamePath = true;
            InformationString = "Game data loaded";

        }

        private void LoadSelectedCharacter()
        {
            IsWorking= true;
            SelectedCharacter = characterList[SelectedCharacterIndex];
            SelectedCharacterLevel = Core.GetPlayerLevel(selectedCharacter.XP);
            CharacterDisplayName = $"{selectedCharacter.CharacterName} (Level {SelectedCharacterLevel})";

            #region Character
            XP = selectedCharacter.XP;
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
            InformationString = $"User {PlayerProfile.UserID} was saved";

            Progress = 100;
            WasCharacterExported = true;
            isWorking= false;
            IsProgressVisible= false;
        }

        private void SetCharacterValues()
        {
            SelectedCharacter.XP = XP;
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
