using MCTGClassLibrary.Networking;
using MCTGClassLibrary.Networking.EndpointHandlers;
using MCTGClassLibrary.Networking.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCTGClassLibrary
{
    public class RequestHandler
    {
        public RequestHandler(){}

        public Response HandleRequest(Request request)
        {
            IEndpointHandler endpointHandler = EndpointHandlerManager.Get(request.Endpoint);

            if (endpointHandler.IsNull())
                return ResponseManager.BadRequest($"Endpoint {request.Endpoint} does not exist");

            try
            {
                Response resp = endpointHandler.HandleRequest(request);
                return resp;
            }
            catch(Exception ex)
            {
                ex.Log();
            }

            return ResponseManager.InternalServerError();
            
        }
    }
}
