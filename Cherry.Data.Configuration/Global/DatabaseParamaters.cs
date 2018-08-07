namespace Cherry.Data.Configuration.Global
{
    public static class DatabaseParamaters
    {
        public static bool IsInDevelopment
        {
            get
            {
#if PROD
                return false;
#else
                return true;
#endif
            }
        }
    }
}
