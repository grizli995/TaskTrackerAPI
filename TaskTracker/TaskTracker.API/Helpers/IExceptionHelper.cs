using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TaskTracker.API.Helpers
{
    public interface IExceptionHelper
    {
        HttpResponseMessage CreateErrorResponse(Exception ex, System.Net.HttpStatusCode statusCode, string message);
    }
}
