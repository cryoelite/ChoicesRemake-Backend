using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Products.Models
{
    [BindRequired]
    public class Mass
    {
        public Mass(double MassInKg)
        {
            this.MassInKg = MassInKg;
        }

        public Mass()
        { }

        public double MassInKg { get; set; } = 0;
    }
}