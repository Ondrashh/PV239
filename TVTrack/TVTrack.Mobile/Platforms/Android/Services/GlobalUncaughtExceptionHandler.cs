using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.Mobile.Services.Interfaces;

namespace TVTrack.Mobile.Platforms
{
    public class GlobalUncaughtExceptionHandler : Java.Lang.Object, Java.Lang.Thread.IUncaughtExceptionHandler
    {
        private readonly IGlobalExceptionService globalExceptionService;

        public GlobalUncaughtExceptionHandler(IGlobalExceptionService globalExceptionService)
        {
            this.globalExceptionService = globalExceptionService;
        }

        public void UncaughtException(Java.Lang.Thread t, Throwable e)
        {
            globalExceptionService.HandleException(e, nameof(GlobalUncaughtExceptionHandler));
        }
    }
}
