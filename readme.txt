github repository: https://github.com/Taha-cmd/MCTG

Basic Design:

Endpoint handling:
The three classes (HttpServer, HttpRequest, HttpResponse) were taken from the 1st project, but the the request handling is different.
The class RequestHandler does not handle the requests anymore, it just forwards the request to the proper endpoint and returns a response.
The Endpointhandling has the following structure: I have an interface (IEndpointhandler) and an EndpointHandlerBase which implements default behavior as virtual methods.
Each endpoint is a class that inherits from EndpointHandlerBase and overrides the behavior that it should implement. I used a similar approach to the factory pattern to know
when to contact the correct endpoint by using the static class EndpointHandlerManager. I tell the class which endpoint I would like to contact, it returns me the correct instance to 
handle the request.


Database:
I used a simple class (Database) which builds the connection string and returns it. The actual querying and manipulation of data takes place in the repoistory classes.
Here I have a RepoistoryBase which implements common behavior and 7 other Repoistory classes that inherits from the base class. Each repoistory class corresponds to a table in the database.
Most queries were to retrieve a single value (coins, username, user id, check if users exists etc..). I abstracted this behavior in the base class with parameterized generic methods,
making it possible to perform most queries with one line methods and overload most of the methods to use both username or user id.


Cards: 
An abstract class Card is at the top of heirarchy. Two classes inherits from it (SpellCard and MonsterCard). MonsterCard is in turn also an abstract class, which each monster type inherits from.
The fight logic is implemented in the cards. Common functionality in abstracted in the base class.


Battle:
A singleton class GameHandler implements the battle logic. It has a queue of players and automatically starts the battle when 2 unique players are enqueued. When a battle takes place,
the instance fires and event BattleEnded and sends the Battle Log. The Threads who enqueued the players wait for this event to return the response. (Wait and Notify Pattern)


Other design decisions:
-All 'magic numbers' are saved as constants in a static global class (Config), like the size of a package, amount of coints per player etc... this makes future modifications easy
-I wrote an api documentation in the file 'home.txt'. This file gets displayed when making a request to the root site.
-I used batch files to create, populate, drop or clean the database.
-I used a CardsManager class to create random cards for unit testing. Since cards are randomly generated for the tests, each test is repeated for 10000 times in a loop, to test all possible cases.
-I extended the curl script to test more edge cases. for example: no payload, invalid json, missing authorization header, wrong array size.
-I used extension methods and lambda syntax for one line methods to make the code more readable


Extra Features:
-No static tokens were used. Instead, pseudo random strings of length 64 were generated. I adapted the curl scripts the save the token returned in a variable to use it in the further requests.
 A similar approach to php was taken to implement the session logic, that is using a global object (here a static dictionary username => token) to keep track of logged in users. For 
 simplicity, no expiration date was added the sessions. But, you can terminate the session yourself (log out) by sending a delete request to /sessions with your token.

-Extended scoreboard and stats. User stats hold the following information: points, battles, won battles, lost battles, win ratio, lose ratio.
 For each win +5 points, for lose -3. Theese values are also saved a constants in the Config class.


Estimiated time:
Unfortunatley, I did not measure the time spent on this project. But fortunatley, the history of the projected is well documented with git. With a total of 43 commits, 
I approximatley spent 2.5 - 5 hours per commit, which makes it anywhere between 105 and 210 hours.


pitfalls, challanges, failures (no such thing as failures, I mean lessons learned):
- correctly reading the http request using stream reader: the problem here is the empty line between the headers and the payload. the Method ReadLine() blocks exactly at this line. 
 It took me 2 days to figure this out. Solution: Read the request character wise
- synchronise the battle. How to keep the battle requesting threads waiting for the battle and keep the connections open?. Solution: use events and ManuelResetEvent (wait and notify pattern).
 the battle fires an event,  the callback for that event grabs the battle log and set the ResetEvent, thus releasing the waiting threads.
- using linq where possible: linq is pretty awesome, but it can get out of hands sometimes and mess up the code if not used correctly. Sometimes a foreach is more than enough.
- how to manage such a big project? don't write spaghetti code! take the time to think about code before writing it, take the time to refactor ugly code. Use interfaces, inheritance, 
and the tools the language has to offer. You don't know them? google, stackoverflow, Microsoft. Use common patterns, use good variable names. Learning about extension methods was the best
thing in this project.
  




 