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
    public class ExamIndexModel : PageModel
    {
        private readonly ILogger<ExamIndexModel> _logger;

        private readonly UploadfileContext _context;

        public ExamIndexModel(ILogger<ExamIndexModel> logger, UploadfileContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<Exams> Exams { get; set; }

        public void OnGet(string searchby, string search)
        {
            if (searchby == "Reference")
            {
                IEnumerable<Exams> objList = _context.Exams;
                Exams = _context.Exams.Where(x => x.Reference == search || search == null).ToList();

            }

            else
            {
                IEnumerable<Exams> objList = _context.Exams;
                Exams = _context.Exams.Where(x => x.Name == search || search == null).ToList();

            }

        }

        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            var myInv = await _context.Exams.FirstOrDefaultAsync(m => m.Id == id);
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
                    FileDownloadName = $"Exams {myInv.Number}.pdf"
                };
            }

        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            var myInv = await _context.Exams.FirstOrDefaultAsync(m => m.Id == id);
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

            Exams = await _context.Exams.ToListAsync();
            return Page();
        }
    }
}
