using DataAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace MTApi.Services.Song
{
    public class SongService: ISongService
    {
        private readonly IConfiguration configuration;
        private readonly string framework;
        public SongService(IConfiguration configuration) 
        {
            this.configuration = configuration;
            framework = Assembly
                    .GetEntryAssembly()?
                    .GetCustomAttribute<TargetFrameworkAttribute>()?
                    .FrameworkName;
        }

        public PagedResult<SongsListVM> ListSongs(int size, int page)
        {

            var sql = new SQL(configuration.GetConnectionString("admin"), framework);
            sql.OpenConnection();
            try
            {

                string query = @"select t1.songID, t1.title as songTitle, t1.dateCreation, t2.title as artistTitle from Song as t1
                                LEFT JOIN
                                Artist as t2
                                on t1.artistID = t2.artistID ORDER BY t1.dateCreation OFFSET @start ROWS FETCH NEXT @size ROWS ONLY";

                sql.Parameters.Add("@start", size * (page > 0 ? page - 1 : 0));
                sql.Parameters.Add("@size", size);
                var total = sql.ExecuteScalar<int>("SELECT COUNT(songId) AS total FROM dbo.Song");

                var dt = sql.ExecuteDT(query, true);

                var songs = (from DataRow rw in dt.Rows
                        select new SongsListVM()
                        {
                            id = Convert.ToInt32(rw["songID"]),
                            Title = rw["songTitle"].ToString(),
                            Artist = rw["artistTitle"].ToString(),
                            DateCreated = Convert.ToDateTime(rw["dateCreation"])
                        }).ToList();

                return new PagedResult<SongsListVM>()
                {
                    List = songs,
                    Info = new PagingInfo()
                    {
                        Page = page,
                        PageSize = size,
                        TotalItems = total
                    }
                };
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                throw;
            }
            finally
            {
                sql.CloseConnection();
            }
        }
    }
}
