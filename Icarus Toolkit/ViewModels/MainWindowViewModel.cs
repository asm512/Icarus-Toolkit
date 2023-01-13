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

namespace Icarus_Toolkit.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public string gamePath = Path.Combine(Directory.GetCurrentDirectory(), "sandbox");

        [ObservableProperty]
        public bool validGamePath = true;

        [ObservableProperty]
        public bool editMode = false;

        [ObservableProperty]
        public List<Icarus.Character> characterList;

        private int selectedCharacterIndex;
        public int SelectedCharacterIndex
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
        public Icarus.Character selectedCharacter;

        [ObservableProperty]
        public int selectedCharacterLevel;

        [ObservableProperty]
        public string loadText = "Load Character Data";

        [ObservableProperty]
        public bool isCharacterLoaded = false;

        private int currentLoadedCharacterIndex;
        public int CurrentLoadedCharacterIndex
        {
            get => currentLoadedCharacterIndex;
            set
            {
                currentLoadedCharacterIndex = value;
                OnPropertyChanged();
                LoadText = "Load Character Data";
            }
        }

        [ObservableProperty]
        public string characterDisplayName;

        [ObservableProperty]
        public Profile playerProfile;

        public void ConfirmPath()
        {
            var gameData = new Icarus.GameData(gamePath);
            CharacterList = gameData.GetCharacters().Characters;
            PlayerProfile = gameData.GetProfile();

            SelectedCharacterIndex = 0;

            ValidGamePath= true;
        }

        public void LoadSelectedCharacter()
        {
            SelectedCharacter = characterList[SelectedCharacterIndex];
            SelectedCharacterLevel = Icarus.Core.GetPlayerLevel(selectedCharacter.XP);
            CharacterDisplayName = $"{selectedCharacter.CharacterName} (Level {SelectedCharacterLevel})";
            CurrentLoadedCharacterIndex = SelectedCharacterIndex;

            IsCharacterLoaded = true;
        }
    }
}
