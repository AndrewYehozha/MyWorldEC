using System;

public static class JsonResults
{
    public static object Success()
    {
        return new { success = true };
    }

    public static object Success(object data)
    {
        return new { success = true, data };
    }

    public static object Error(int errorNum = 0, string errorMessage = "")
    {
        return new { success = false, errorNum, errorMessage };
    }
}
