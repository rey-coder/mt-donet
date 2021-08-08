using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTApi.Services.Song
{
    public interface ISongService
    {
        /// <summary>
        /// Gets paginated list of songs
        /// </summary>
        /// <param name="size"></param>
        /// <param name="page"></param>
        PagedResult<SongsListVM> ListSongs(int size, int page);
    }
}
