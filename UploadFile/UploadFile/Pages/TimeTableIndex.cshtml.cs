using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UploadFile.Data;

namespace UploadFile.Pages
{
    public class TimeTableIndexModel : PageModel
    {
        private readonly ILogger<TimeTableIndexModel> _logger;

        private readonly UploadfileContext _context;

        public TimeTableIndexModel(ILogger<TimeTableIndexModel> logger, UploadfileContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<TimeTables> TimeTables { get; set; }

        public void OnGet(string searchby, string search)
        {
            if (searchby == "Reference")
            {
                IEnumerable<TimeTables> objList = _context.TimeTables;
                TimeTables = _context.TimeTables.Where(x => x.Reference == search || search == null).ToList();

            }

            else
            {
                IEnumerable<TimeTables> objList = _context.TimeTables;
                TimeTables = _context.TimeTables.Where(x => x.Name == search || search == null).ToList();

            }

        }

        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            var myInv = await _context.TimeTables.FirstOrDefaultAsync(m => m.Id == id);
            if (myInv == null)
            {
                return NotFound();
            }

            if (myInv.Attachment == null)
            {
                return Page();
            }
            else
            {
                byte[] byteArr = myInv.Attachment;
                string mimeType = "application/pdf";
                return new FileContentResult(byteArr, mimeType)
                {
                    FileDownloadName = $"TimeTables {myInv.Number}.pdf"
                };
            }

        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            var myInv = await _context.TimeTables.FirstOrDefaultAsync(m => m.Id == id);
            if (myInv == null)
            {
                return NotFound();
            }

            if (myInv.Attachment == null)
            {
                return Page();
            }
            else
            {
                myInv.Attachment = null;
                _context.Update(myInv);
                await _context.SaveChangesAsync();
            }

            TimeTables = await _context.TimeTables.ToListAsync();
            return Page();
        }
    }
}
