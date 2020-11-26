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

            return ResponseManager.InternalServerError();
            
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
