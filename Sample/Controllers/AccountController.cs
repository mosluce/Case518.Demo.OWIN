using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Sample.Models;
using Sample.Utils;

namespace Sample.Controllers
{
    [Authorize]
    public class AccountController : Controller
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
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!Session["Captcha"].Equals(model.Captcha))
            {
                ModelState.AddModelError(string.Empty, "驗證碼錯誤");
                return View();
            }

            //實作登入邏輯
            using (var db = new SampleDbContext())
            {
                var user = db.Users.FirstOrDefault(c => c.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "帳號錯誤");
                    return View();
                }

                if (user.Password != model.Password)
                {
                    ModelState.AddModelError(string.Empty, "密碼錯誤");
                    return View();
                }

                if (!user.Activated)
                {
                    ModelState.AddModelError(string.Empty, "尚未啟用");
                    return View();
                }

                //建立Cookie(using OWIN)
                var auth = Request.GetOwinContext().Authentication;
                var claim = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name) 
                }, DefaultAuthenticationTypes.ApplicationCookie);
                auth.SignIn(claim);
            }

            return Redirect("~/");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!Session["Captcha"].Equals(model.Captcha))
            {
                ModelState.AddModelError(string.Empty, "驗證碼錯誤");
                return View();
            }

            using (var db = new SampleDbContext())
            {
                var any = db.Users.Any(c => c.Email == model.Email);
                if (any)
                {
                    ModelState.AddModelError(string.Empty, "Email已被註冊");
                    return View();
                }
                
                var user = db.Users.Add(new ApplicationUser
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    Subscription = model.Subscription,
                    Gender = model.Gender,
                    RegisterTime = DateTime.Now,
                    Activated = false,
                    ActivatedTime = DateTime.Now,
                    ActivationCode = ((ShortGuid)Guid.NewGuid()).ToString(),
                    Birthday = model.Birthday
                });

                db.SaveChanges();

                await
                    EmailSender.Send(user.Email, "Sample啟動信",
                        string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority,
                            Url.Action("Activate", new {code = user.ActivationCode})));
            }

            return Redirect("~/");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Activate(string code)
        {
            using (var db = new SampleDbContext())
            {
                var user = db.Users.FirstOrDefault(c => c.ActivationCode == code && !c.Activated);

                if (user == null)
                {
                    ViewBag.Message = "啟動碼無效或已被啟動";
                    return View();
                }

                user.Activated = true;
                user.ActivatedTime = DateTime.Now;

                db.SaveChanges();
            }
            
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Forgot(string email)
        {
            if (email.IsEmpty())
            {
                @ViewBag.Message = "請先輸入Email";
                return View();
            }

            using (var db = new SampleDbContext())
            {
                var user = db.Users.FirstOrDefault(c => c.Email == email);

                if (user == null)
                {
                    @ViewBag.Message = "Email不存在";
                    return View();
                }

                await EmailSender.Send(email, "Sample密碼信", user.Password);
            }

            return Redirect("~/");
        }

        [HttpGet]
        public ActionResult MyProfile()
        {
            var authenticationManager = Request.GetOwinContext().Authentication;
            var claim = authenticationManager.User.Claims.FirstOrDefault(c => c.Type.Contains("authenticationmethod"));
            var emailClaim = authenticationManager.User.Claims.FirstOrDefault(c => c.Type.Contains("email"));
            
            @ViewBag.IsLoginByFacebook = claim.Value.Equals("Facebook");

            using (var db = new SampleDbContext())
            {
                var user = db.Users.Select(c => new ProfileViewModel
                {
                    Name = c.Name,
                    Birthday = c.Birthday ?? DateTime.Now,
                    Gender = c.Gender,
                    Email = c.Email,
                    Password = c.Password,
                    Subscription = c.Subscription
                }).FirstOrDefault(c => c.Email == emailClaim.Value);

                return View(user);
            }
        }

        [HttpPost]
        public ActionResult MyProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return MyProfile();
            }

            using (var db = new SampleDbContext())
            {
                var user = db.Users.FirstOrDefault(c => c.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "指定帳號不存在");
                    return View(model);
                }

                if (!model.Password.IsEmpty()) user.Password = model.Password;
                user.Birthday = model.Birthday;
                user.Name = model.Name;
                user.Gender = model.Gender;
                user.Subscription = model.Subscription;

                db.SaveChanges();
            }

            return MyProfile();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return Redirect("~/");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult FacebookLogin()
        {
            var provider = Request.GetOwinContext().Authentication.GetExternalAuthenticationTypes().FirstOrDefault().AuthenticationType;
            return new ChallengeResult(provider, Url.Action("FacebookLoginCallback", new { provider = provider}));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> FacebookLoginCallback(string provider)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            var extInfo =
                await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            //實作自動註冊
            using (var db = new SampleDbContext())
            {
                var any = db.Users.Any(c => c.Email == loginInfo.Email);
                if (!any)
                {
                    db.Users.Add(new ApplicationUser
                    {
                        Email = loginInfo.Email,
                        Name = loginInfo.DefaultUserName,
                        Activated = true,
                        Subscription = false,
                        RegisterTime = DateTime.Now,
                        ActivatedTime = DateTime.Now
                    });
                }
                else
                {
                    var user = db.Users.FirstOrDefault(c=>c.Email == loginInfo.Email);
                    user.Name = loginInfo.DefaultUserName;
                }

                db.SaveChanges();

                //轉換為內部Cookie
                var ctx = Request.GetOwinContext();
                var result = ctx.Authentication.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie).Result;
                ctx.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                var claims = result.Identity.Claims.ToList();
                claims.Add(new Claim(ClaimTypes.AuthenticationMethod, provider));

                var ci = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                ctx.Authentication.SignIn(ci);
            }

            return Redirect("~/");            
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Captcha()
        {
            var captcha = CaptchaGenerator.Create();

            Session["Captcha"] = captcha.Text;

            FileResult result;

            using (var stream = new MemoryStream())
            {
                captcha.Image.Save(stream, ImageFormat.Png);
                result = File(stream.ToArray(), "image/png");
            }

            return result;
        }

        #region Helper (擷取自MVC範本)
        // 新增外部登入時用來當做 XSRF 保護
        private const string XsrfKey = "dsjdkfsfwjWKLEFJLjdslkjfdl3724842hdjJjdw0";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}