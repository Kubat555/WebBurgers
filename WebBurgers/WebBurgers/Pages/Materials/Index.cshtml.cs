﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebBurgers.DataBase;
using WebBurgers.Repository;
using WebBurgers.Repository.Models;

namespace WebBurgers.Pages.Materials
{
    public class IndexModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext db;

        public IndexModel(WebBurgers.Repository.BurgerContext context)
        {
            db = context;
        }


        public SqlDataReader reader;
        public SelectList UnitList { get; set; }

        public async Task OnGetAsync()
        {
            UnitList = new SelectList(await db.Units.ToListAsync(), "Id", "Name");

            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("ShowMaterial", connection);
            command.CommandType = CommandType.StoredProcedure;
            reader = command.ExecuteReader();
        }

        public IActionResult OnPost(int id)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_deleteMaterial", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            reader = command.ExecuteReader();
            reader.Close();
            return RedirectToPage();
        }
    }
}
