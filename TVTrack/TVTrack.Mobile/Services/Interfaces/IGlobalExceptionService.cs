using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TVTrack.Mobile.Services.Interfaces
{
    public interface IGlobalExceptionService
    {
        void HandleException(Exception exception, [CallerMemberName] string? source = null);
    }
}
