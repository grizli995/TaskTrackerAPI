using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.API.Helpers
{
    public class ExceptionHelper
    {
        public ExceptionHelper()
        {

        }

        public HttpResponseMessage CreateErrorResponse(Exception ex, System.Net.HttpStatusCode statusCode, string message)
        {
            var result = new HttpResponseMessage(statusCode);
            var innerExceptionMessage = ex.InnerException?.Message ?? string.Empty;
            result.Content = new StringContent($"{ex.Message} {innerExceptionMessage}");
            return result;
        }
    }
}
