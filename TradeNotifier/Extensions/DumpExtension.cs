using Newtonsoft.Json;

namespace TradeNotifier.Extensions
{
    public static class DumpExtension
    {
        public static string Dump(this object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }
    }
}
