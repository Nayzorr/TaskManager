namespace TaskManager.Api.Helpers
{
    public class CookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int expirationTime)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now.AddMinutes(expirationTime)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
        }

        public void RemoveCookie(string key)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key, options);
        }
    }
}
