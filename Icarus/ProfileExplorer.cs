using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Icarus
{
    internal class ProfileExplorer
    {
        public List<Profile> ProfileList = new List<Profile>();

        public ProfileExplorer(string charactersJson)
        {
            
        }
    }

    public class MetaResource
    {
        public string MetaRow { get; set; }
        public int Count { get; set; }
    }

    public class Profile
    {
        public string UserID { get; set; }
        public List<MetaResource> MetaResources { get; set; }
        public List<object> UnlockedFlags { get; set; }
        public List<Talent> Talents { get; set; }
        public int NextChrSlot { get; set; }
        public int DataVersion { get; set; }
    }

    public class Talent
    {
        public string RowName { get; set; }
        public int Rank { get; set; }
    }
}
