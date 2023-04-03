using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientConsole.Service.Samples
{
    internal interface ISampleService
    {
        IList<Models.Sample> GetSampleData();
    }
}
