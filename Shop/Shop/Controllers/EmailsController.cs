using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Models.Emails;

namespace Shop.Controllers
{
    public class EmailsController : Controller
    {
        private readonly IEmailServices _emailServices;

        public EmailsController(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel vm, List<IFormFile> Attachments)
        {
            var dto = new EmailDto()
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                AttachmentPaths = new List<string>() // Või AttachmentDto list, kui failide sisu salvestamine on vajalik
            };

            if (Attachments != null && Attachments.Count > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var file in Attachments)
                {
                    var filePath = Path.Combine(uploadPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    dto.AttachmentPaths.Add(filePath);
                }
            }

            _emailServices.SendEmail(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
