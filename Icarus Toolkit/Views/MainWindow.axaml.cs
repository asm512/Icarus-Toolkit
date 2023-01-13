using Avalonia.Controls;
using Avalonia.Media;
using Icarus_Toolkit.ViewModels;
using System.Collections.Generic;
using System.IO;

namespace Icarus_Toolkit.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            var gamedata = new Icarus.GameData(Path.Combine(Directory.GetCurrentDirectory(), "sandbox"));
            var charactersdata = gamedata.GetCharacters();

            CharacterSelectCombobox.Items = charactersdata.Characters;
            CharacterSelectCombobox.SelectedIndex = 0;

        }
    }
}
