using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;

namespace Icarus
{
    public class ProfileExplorer
    {
        public Profile PlayerProfile;
        private readonly string profilePath;

        public ProfileExplorer(string profilePath)
        {
            this.profilePath = profilePath;
            RefreshProfile();
        }

        public bool ExportProfile(Profile profile)
        {
            var serializerOptions = new JsonSerializerOptions() { WriteIndented = true };

            try
            {
                File.WriteAllText(profilePath, JsonSerializer.Serialize(profile, serializerOptions));
                Log.Information("Exported profile");
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return false;
            }
        }

        public void RefreshProfile()
        {
            PlayerProfile = JsonSerializer.Deserialize<Profile>(File.ReadAllText(profilePath));
        }
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

    public class MetaResource
    {
        public string MetaRow { get; set; }
        public int Count { get; set; }
    }

    public class Talent
    {
        public string RowName { get; set; }
        public int Rank { get; set; }
    }
}
