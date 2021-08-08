using DataAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace MTApi.Services.Artist
{
    public class ArtistService: IArtistService
    {
        private readonly IConfiguration configuration;
        private readonly string framework;
        public ArtistService(IConfiguration configuration)
        {
            this.configuration = configuration;
            framework = Assembly
                    .GetEntryAssembly()?
                    .GetCustomAttribute<TargetFrameworkAttribute>()?
                    .FrameworkName;
        }

        public void AddArtist(ArtistVM artistVM)
        {
            var sql = new SQL(configuration.GetConnectionString("admin"), framework);
            sql.OpenConnection();
            try
            {
                sql.BeginTransaction();

                string query = "INSERT INTO dbo.Artist (dateCreation, title, biography, imageUrl, heroUrl) VALUES (@dateCreation, @title, @biography, @imageUrl, @heroUrl)";

                sql.Parameters.Add("@dateCreation", DateTime.UtcNow);
                sql.Parameters.Add("@title", artistVM.ArtistTitle);
                sql.Parameters.Add("@biography", artistVM.Bio);
                sql.Parameters.Add("@imageUrl", artistVM.ImageHyperLink);
                sql.Parameters.Add("@heroUrl", artistVM.heroHyperLink);

                sql.Execute(query, true);
                sql.Commit();
            } 
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                sql.Rollback();
                throw;
            }
            finally {
                sql.CloseConnection();
            }
        }

        

        public PagedResult<ArtistListVM> SearchArtist(string name, int page, int size)
        {
            var sql = new SQL(configuration.GetConnectionString("admin"), framework);
            sql.OpenConnection();
            try
            {

                string query = @"SELECT artistId, title, dateCreation FROM dbo.Artist WHERE title LIKE '%' + @searchname + '%'
                                ORDER BY dateCreation OFFSET @start ROWS FETCH NEXT @size ROWS ONLY";

                sql.Parameters.Add("@searchname", name);
                sql.Parameters.Add("@start", size * (page > 0 ? page - 1 : 0));
                sql.Parameters.Add("@size", size);

                var dt = sql.ExecuteDT(query);
                var total = sql.ExecuteScalar<int>("SELECT COUNT(artistId) FROM dbo.Artist WHERE title LIKE '%' + @searchname + '%'");

                var artists = (from DataRow rw in dt.Rows
                             select new ArtistListVM()
                             {
                                 id = Convert.ToInt32(rw["artistId"]),
                                 Title = rw["title"].ToString(),
                                 DateCreated = Convert.ToDateTime(rw["dateCreation"])
                             }).ToList();

                return new PagedResult<ArtistListVM>()
                {
                    List = artists,
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
