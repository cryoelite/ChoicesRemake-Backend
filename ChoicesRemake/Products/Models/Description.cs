using Microsoft.AspNetCore.Mvc;

namespace Products.Models
{
    public class Description
    {
        public Description(string LongDescription, string Title)
        {
            this.LongDescription = LongDescription;
            this.Title = Title;
        }

        public Description()
        { }

        [BindProperty(Name = "LongDescription")]
        public string LongDescription { get; set; } = string.Empty;

        [BindProperty(Name = "Title")]
        public string Title { get; set; } = string.Empty;
    }
}