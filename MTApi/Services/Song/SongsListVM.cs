using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTApi.Services.Song
{
    public class SongsListVM
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
