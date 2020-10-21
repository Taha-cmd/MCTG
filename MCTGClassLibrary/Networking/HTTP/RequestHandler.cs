using MCTGClassLibrary.Networking;
using MCTGClassLibrary.Networking.EndpointHandlers;
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

            if (endpointHandler == null)
                return new Response("400", "Bad Request");

            //OnRequestHandled(new RequestEventArgs());

            return endpointHandler.HandleRequest(request);
        }

        //fire event every time a request is handled
        // gamehandler will subscribe to this event
        public static event EventHandler<RequestEventArgs> RequestHandled;
        protected static void OnRequestHandled(RequestEventArgs args)
        {
            RequestHandled?.Invoke(null, args);
            // cannot use this in a static method
            // send null instead
        }
    }
}
