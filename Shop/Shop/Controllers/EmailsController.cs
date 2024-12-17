﻿using Microsoft.AspNetCore.Mvc;
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
            var files = Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>();
            var dto = new EmailDto()
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attachment = files
            };         

            _emailServices.SendEmail(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
