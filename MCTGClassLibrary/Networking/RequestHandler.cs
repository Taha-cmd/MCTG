using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCTGClassLibrary
{
    public class RequestHandler
    {
        public RequestHandler(){}

        public Response HandleRequest(Request request)
        {
            Response response = new Response("200", "OK");

            /*IRequestHandler handler = GetRequestHandler(request);

            Response response = handler.HandleRequest(request); */


            response.AddPayload("hello world\n");
            OnRequestHandled(new RequestEventArgs());

            return response;
        }

        /*private static IRequestHandler GetRequestHandler(Request request)
        {
            //string Handlername = request
            switch(request)
            {

            }

            return new GameHandler();
        } */


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
