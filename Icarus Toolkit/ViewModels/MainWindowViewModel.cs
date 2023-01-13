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

namespace Icarus_Toolkit.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public string gamePath = Path.Combine(Directory.GetCurrentDirectory(), "sandbox");

        [ObservableProperty]
        public List<Icarus.Character> characterList;

        [ObservableProperty]
        public int selectedCharacterIndex = 0;

        [ObservableProperty]
        public Icarus.Character selectedCharacter;

        public void ConfirmPath()
        {
            var gamedata = new Icarus.GameData(gamePath);
            CharacterList = gamedata.GetCharacters().Characters;
        }

        public void LoadSelectedCharacter()
        {
            SelectedCharacter = characterList[SelectedCharacterIndex];
        }
    }
}
