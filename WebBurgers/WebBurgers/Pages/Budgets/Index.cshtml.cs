using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebBurgers.Repository;
using WebBurgers.Repository.Models;

namespace WebBurgers.Pages.Budgets
{
    public class IndexModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public IndexModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        public IList<Budget> Budget { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Budgets != null)
            {
                Budget = await _context.Budgets.ToListAsync();
            }
        }
    }
}
