using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary
{
    public interface IRequestHandler
    {

        Response HandleRequest(Request request);
        
    }
}
