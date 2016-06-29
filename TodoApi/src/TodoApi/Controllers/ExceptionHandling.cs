using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.RussianPost;

namespace TodoApi.Controllers
{
    static class ExceptionHandling
    {
        internal static IActionResult Handle(Controller controller, Exception exception)
        {
            var rpce = exception as RussianPostCalculateException;
            if (rpce != null)
            {
                var error = new { rpce.Error, rpce.Messages, rpce.ApiUrl };
                return controller.BadRequest(error);
            }
            return controller.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}