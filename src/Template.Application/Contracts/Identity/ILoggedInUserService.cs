using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Application.Contrats
{
    public interface ILoggedInUserService
    {
        public string UserId { get; set; }
    }
}
