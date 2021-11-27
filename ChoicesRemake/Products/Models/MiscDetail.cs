using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Products.Models
{
    [BindRequired]
    public class MiscDetail
    {
        public MiscDetail(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public MiscDetail()
        { }

        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}