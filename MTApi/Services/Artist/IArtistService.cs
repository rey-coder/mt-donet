using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTApi.Services.Artist
{
    public interface IArtistService
    {
        /// <summary>
        /// To add an artist
        /// </summary>
        /// <param name="artistVM"></param>
        void AddArtist(ArtistVM artistVM);
        /// <summary>
        /// To search an artist by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        PagedResult<ArtistListVM> SearchArtist(string name, int page, int size);
    }
}
