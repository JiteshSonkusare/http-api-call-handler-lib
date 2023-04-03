using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientConsole.Service.Azure
{
    internal interface IAzureService
    {
        IList<Models.Sample> GetAzureData();
    }
}
