namespace MCTGClassLibrary.Networking
{
    public interface IEndpointHandler
    {
        Response HandleRequest(Request request);
    }
}
