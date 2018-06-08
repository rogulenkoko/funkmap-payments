using System;
using System.Collections.Generic;
using System.Text;

namespace Funkmap.Payments.Core.Models
{
    public class CommandResponse
    {
        public CommandResponse(bool succes)
        {
            Success = succes;
        }
        public bool Success { get; }

        public string Error { get; set; }
    }
}
