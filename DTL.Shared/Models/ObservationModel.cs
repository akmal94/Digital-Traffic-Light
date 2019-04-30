using System;
using System.Collections.Generic;
using System.Text;

namespace DTL.Shared.Models
{
    public class ObservationModel
    {
        public string Sequence { get; set; }
        public Colors Color { get; set; }
        public string[] Numbers { get; set; }
    }

    public enum Colors
    {
        green,
        red
    }
}
