using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Icarus
{
    public class CharactersData
    {
        public List<Character> Characters = new List<Character>();

        public CharactersData(string charactersJson)
        {
            CharacterList CharacterList = JsonConvert.DeserializeObject<CharacterList>(charactersJson);

            foreach (var _char in CharacterList.CharactersStream)
            {
                Characters.Add(JsonConvert.DeserializeObject<Character>(_char));
            }
        }
    }


    public class CharacterList
    {
        [JsonProperty("Characters.json")]
        public List<string> CharactersStream { get; set; }
    }

    public class Character
    {
        public string CharacterName { get; set; }
        public int ChrSlot { get; set; }
        public int XP { get; set; }
        public int XP_Debt { get; set; }
        public bool IsDead { get; set; }
        public bool IsAbandoned { get; set; }
        public string LastProspectId { get; set; }
        public string Location { get; set; }
        public List<int> UnlockedFlags { get; set; }
        public List<object> MetaResources { get; set; }
        public CosmeticData Cosmetic { get; set; }
        public List<TalentData> Talents { get; set; }
    }

    public class CosmeticData
    {
        public int Customization_Head { get; set; }
        public int Customization_Hair { get; set; }
        public int Customization_HairColor { get; set; }
        public int Customization_Body { get; set; }
        public int Customization_BodyColor { get; set; }
        public int Customization_SkinTone { get; set; }
        public int Customization_HeadTattoo { get; set; }
        public int Customization_HeadScar { get; set; }
        public int Customization_HeadFacialHair { get; set; }
        public int Customization_CapLogo { get; set; }
        public bool IsMale { get; set; }
        public int Customization_Voice { get; set; }
        public int Customization_EyeColor { get; set; }
    }

    public class TalentData
    {
        public string RowName { get; set; }
        public int Rank { get; set; }
    }
}
