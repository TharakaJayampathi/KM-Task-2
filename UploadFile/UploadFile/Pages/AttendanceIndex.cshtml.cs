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
    public class AttendanceIndexModel : PageModel
    {
        private readonly ILogger<AttendanceIndexModel> _logger;

        private readonly UploadfileContext _context;

        public AttendanceIndexModel(ILogger<AttendanceIndexModel> logger, UploadfileContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<Attendances> Attendances { get; set; }

        public void OnGet(string searchby, string search)
        {
            if (searchby == "Reference")
            {
                IEnumerable<Attendances> objList = _context.Attendances;
                Attendances = _context.Attendances.Where(x => x.Reference == search || search == null).ToList();

            }

            else
            {
                IEnumerable<Attendances> objList = _context.Attendances;
                Attendances = _context.Attendances.Where(x => x.Name == search || search == null).ToList();

            }

        }

        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            var myInv = await _context.Attendances.FirstOrDefaultAsync(m => m.Id == id);
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
                    FileDownloadName = $"Attendances {myInv.Number}.pdf"
                };
            }

        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            var myInv = await _context.Attendances.FirstOrDefaultAsync(m => m.Id == id);
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

            Attendances = await _context.Attendances.ToListAsync();
            return Page();
        }
    }
}
