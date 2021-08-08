using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;

public partial class artistDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var sql = new SQL();

        try
        {
            if (Request.QueryString["artistId"] == null) throw new Exception("Please specify a valid artist Id");
            string query = "GetArtistDetails";
            var param = new SqlParameter("@id", Request.QueryString["artistId"]);
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.Int32;
            sql.Parameters.Add(param);
            var dr = sql.ExecuteStoredProcedureDataReader(query);

            while(dr.Read())
            {

                title.Text = dr["title"].ToString();
                banner.ImageUrl = dr["heroUrl"].ToString();
                artistImg.ImageUrl = dr["imageUrl"].ToString();
                bio.Text = dr["biography"].ToString();
            }

            sql.CloseReader(dr);
        }
        catch(Exception ex)
        {
            Trace.Write(ex.Message);
        }
    }
}