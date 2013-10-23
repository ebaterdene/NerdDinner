using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NerdDinner.Models;

namespace NerdDinner.Infrastructure.Ninject
{
    public class GeneralModule : global::Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IDinnerRepository>().To<DinnerRepository>();
        }
    }
}