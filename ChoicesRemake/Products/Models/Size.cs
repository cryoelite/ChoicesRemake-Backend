using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Products.Models
{
    [BindRequired]
    public class Size
    {
        public Size(double HeightInMm, double LengthInMm, double WidthInMm)
        {
            this.WidthInMm = WidthInMm;
            this.HeightInMm = HeightInMm;
            this.LengthInMm = LengthInMm;
        }

        public Size()
        { }

        public double HeightInMm { get; set; } = 0;
        public double LengthInMm { get; set; } = 0;
        public double WidthInMm { get; set; } = 0;
    }
}