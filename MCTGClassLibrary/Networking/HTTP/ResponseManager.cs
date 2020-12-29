namespace MCTGClassLibrary.Networking.HTTP
{
    public class ResponseManager
    {
        private ResponseManager() { }


        // https://de.wikipedia.org/wiki/HTTP-Statuscode

        // 2xx
        static public Response OK(string payload = "")          => new Response("200", "OK", payload);
        static public Response Created(string payload = "")     => new Response("201", "Created", payload);
        static public Response Accepted(string payload = "")    => new Response("202", "Accepted", payload);



        // 4xx
        static public Response BadRequest(string payload = "")                      => new Response("400", "Bad Request", payload);
        static public Response Unauthorized(string payload = "Unauthorized")        => new Response("401", "Unauthorized", payload);
        static public Response Forbidden(string payload = "")                       => new Response("403", "Forbidden", payload);
        static public Response NotFound(string payload = "")                        => new Response("404", "Not Found", payload);
        static public Response MethodNotAllowed(string payload = "")                => new Response("405", "Method Not Allowed", payload);


        // 5xx
        static public Response InternalServerError(string payload = "Internal Servor Error") => new Response("500", "Internal Server Error", payload);
        static public Response NotImplemented(string payload = "")      => new Response("501", "Not Implemented", payload);
    }
}
