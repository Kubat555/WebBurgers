﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBurgers.Repository;
using WebBurgers.Repository.Models;

namespace WebBurgers.Pages.Sale
{
    public class CreateModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public CreateModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Id");
        ViewData["Product"] = new SelectList(_context.Products, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public SaleOfProduct SaleOfProduct { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.SaleOfProducts == null || SaleOfProduct == null)
            {
                return Page();
            }

            _context.SaleOfProducts.Add(SaleOfProduct);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
