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

            if (endpointHandler.IsNull())
                return new Response("400", "Bad Request", $"Endpoint {request.Endpoint} does not exist");

            //OnRequestHandled(new RequestEventArgs());

            try
            {
                Response resp = endpointHandler.HandleRequest(request);
                return resp;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in RequestHandler from {ex.Source}:" + ex.Message);
                Console.WriteLine($"Stack call {ex.StackTrace}");
            }

            return new Response("500", "Internal Server Error");
            
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
