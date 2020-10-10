using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCTGClassLibrary
{
    public class RequestHandler
    {
        private RequestHandler(){}

        public static Response HandleRequest(Request request)
        {
            Response response = new Response("200", "OK");

            response.AddHeader("Content-Type", "text");
            response.AddHeader("Server", "my shitty laptop");
            response.AddHeader("Date", DateTime.Today.ToString());



            response.AddPayload("hello world\n");


            OnRequestHandled(new RequestEventArgs());

            return response;
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
