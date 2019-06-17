using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public static class JsonResults
    {
        public static object Success()
        {
            return new { Success = true };
        }

        public static object Success(object data)
        {
            return new { Success = true, data };
        }

        public static object Error(int errorNum = 0, string errorMessage = "")
        {
            return new { Success = false, ErrorNum = errorNum, ErrorMessages = errorMessage };
        }
    }
}