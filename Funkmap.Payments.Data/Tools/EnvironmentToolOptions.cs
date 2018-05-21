namespace Funkmap.Payments.Data.Tools
{
    public class EnvironmentToolOptions
    {
        public bool DeleteIfExists { get; set; }

        public bool NeedFeelStatic { get; set; }

        public static EnvironmentToolOptions Default()
        {
            return new EnvironmentToolOptions()
            {
                DeleteIfExists = false,
                NeedFeelStatic = false
            };
        }
    }
}
