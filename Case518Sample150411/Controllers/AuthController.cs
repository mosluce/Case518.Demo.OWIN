using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Case518Sample150411.Models;
using Case518Sample150411.Models.ViewModels;
using Case518Sample150411.Utils;
using CSharpVitamins;

namespace Case518Sample150411.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (!Session["ValidationCode"].Equals(model.ValidationCode)) return Json(Result.CreateFailure("驗證碼輸入錯誤"));

            using (var db = new SampleDbContext())
            {
                var user = db.Users.FirstOrDefault(c => c.Email == model.Email);
                
                if (user == null) return Json(Result.CreateFailure("帳號錯誤"));
                if (user.Password != model.Password) return Json(Result.CreateFailure("密碼錯誤"));
                if (!user.Activated) return Json(Result.CreateFailure("尚未開通"));

                //OWIN登入
                var ctx = Request.GetOwinContext();
                var auth = ctx.Authentication;
                var claim = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email) 
                }, "ApplicationCookie");
                
                auth.SignIn(claim);

                return Json(Result.CreateSuccess(user));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ValidationCode()
        {
            var vc = ValidationCodeGenerator.GenValidationCode();
            Session["ValidationCode"] = vc;

            var bmp = ValidationCodeGenerator.ConvertTextToImage(vc, "Microsoft JhengHei", 24);
            FileContentResult result;

            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                result = File(stream.GetBuffer(), "image/png");
            }

            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            using (var db = new SampleDbContext())
            {
                if (!Session["ValidationCode"].Equals(model.ValidationCode)) return Json(Result.CreateFailure("驗證碼輸入錯誤"));

                var any = db.Users.Any(c => c.Email == model.Email);
                if (any) return Json(Result.CreateFailure("電子郵件已被註冊過"));

                ShortGuid code = Guid.NewGuid();

                var user = db.Users.Add(new AppUser
                {
                    Email = model.Email,
                    Gender = model.Gender,
                    Name = model.Name,
                    Password = model.Password,
                    Subscribe = model.Subscribe,
                    Activated = false,
                    ActivationCode = code,
                    RegisterDate = DateTime.Now
                });

                try
                {
                    db.SaveChanges();
                    var content = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority,
                        Url.Content("~/auth/activate?code=" + code));
                    await Email.SendValidationMail(model.Email, content);
                    return Json(Result.CreateSuccess(user));
                }
                catch (Exception ex)
                {
                    return Json(Result.CreateFailure(ex.Message));
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Activate(string code)
        {
            using (var db = new SampleDbContext())
            {
                var user = db.Users.FirstOrDefault(c => c.ActivationCode == code);

                if (user != null)
                {
                    //不重複啟動
                    if (!user.Activated) { 
                        user.Activated = true;
                        user.ActivatedDate = DateTime.Now;
                        db.SaveChanges();
                        return View(true);
                    }
                }
            }
            return View(false);
        }
    }
}