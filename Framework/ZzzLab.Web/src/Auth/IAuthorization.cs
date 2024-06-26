﻿using Microsoft.AspNetCore.Http;

namespace ZzzLab.Web.Auth
{
    public interface IAuthorization
    {
        bool IsAuth(HttpRequest request, out string message);

        bool IsAuth(HttpRequest request);
    }
}