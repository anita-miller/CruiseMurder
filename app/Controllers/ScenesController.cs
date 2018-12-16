﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScenesController : ControllerBase
    {
        string databaseName = "CruiseMurderDB";
        SqlConnection con;
        SqlDataAdapter da;
        DataSet ds;

     

        // GET api/scenes/id
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            con = new SqlConnection(@"data source=cruisemurderdb.cat4spyvo2xb.ap-southeast-2.rds.amazonaws.com;" +
                "initial catalog=" + databaseName + ";" + 
                "User Id=sa;" +
                "Password=12345678");
            con.Open();
            string qry = "getScene " + id;
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            SceneItem item = new SceneItem
            {
                SceneContent = (String)dataReader.GetValue(0),
                SceneImage = (String)dataReader.GetValue(1),
                SceneId = (int)dataReader.GetValue(2),
                EndingType = (String)dataReader.GetValue(3),
                SceneLocation = (String)dataReader.GetValue(4)
            };

            dataReader.Close();
            cmd.Dispose();

            qry = "getChoicesFromScene " + id;
            cmd = new SqlCommand(qry, con);
            dataReader = cmd.ExecuteReader();
            List<ChoiceItem> choices = new List<ChoiceItem>();
            while (dataReader.Read())
            {
                ChoiceItem choice = new ChoiceItem();
                choice.Consequent = (int)dataReader.GetValue(0);
                choice.Text = (String)dataReader.GetValue(1);
                choices.Add(choice);
            }
            item.Choices = choices;

            dataReader.Close();
            cmd.Dispose();
            con.Close();

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            return jsonString;
        }
    }
}
