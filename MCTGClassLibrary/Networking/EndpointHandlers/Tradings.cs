using MCTGClassLibrary.Networking.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Database.Repositories;
using System.Text.Json;
using MCTGClassLibrary.Enums;
using MCTGClassLibrary.Cards;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Tradings : EndpointHandlerBase
    {
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest($"No authorization header found");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            TradeDeal[] deals = new TradeDealsRepository().GetDeals();
            return ResponseManager.OK( JsonSerializer.Serialize<TradeDeal[]>(deals) );
        }

        protected override Response DeleteHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest($"No authorization header found");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            // token 2 is deal id => index 1
            string[] routeTokens = request.Route.Split("/").SkipWhile(el => el.IsNullOrWhiteSpace()).ToArray();

            if (routeTokens.Length != 2)
                return ResponseManager.BadRequest("invalid request");

            TradeDealsRepository dealer = new TradeDealsRepository();

            if (!dealer.DealExists(routeTokens[1]))
                return ResponseManager.NotFound($"Deal {routeTokens[1]} not found");

            string username = Session.GetUsername(ExtractAuthorizationToken(request.Authorization));
            if (!dealer.HasDeal(username, routeTokens[1]))
                return ResponseManager.BadRequest($"Deal {routeTokens[1]} is not yours!");

            dealer.RemoveDeal(routeTokens[1]);
            return ResponseManager.OK($"Deal {routeTokens[1]} successfully deleted");
        }
        protected override Response PostHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest($"No authorization header found");

            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest($"No payload found");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized();

            string[] routeTokens = request.Route.Split("/").SkipWhile(el => el.IsNullOrWhiteSpace()).ToArray();
            
            switch(routeTokens.Length)
            {
                case 1: return CreateTradeDeal(request);
                case 2: return HandleTradeDeal(request, routeTokens[1]); 
            } 

            return ResponseManager.InternalServerError();
        }

        private Response HandleTradeDeal(Request request, string dealId)
        {
            var dealer = new TradeDealsRepository();
            string username = Session.GetUsername(ExtractAuthorizationToken(request.Authorization));

            if (!dealer.DealExists(dealId))
                return ResponseManager.NotFound($"deal {dealId} does not exist");

            var deal = dealer.GetDeal(dealId);
            var users = new UsersRepository();

            if (deal.OwnerId == users.GetUserID(username))
                return ResponseManager.BadRequest("you can't trade with yourself");

            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("no payload");

            string offeredCardId = request.Payload.Replace('\"', ' ').Trim();
            var cards = new CardsRepository();

            if (!cards.CardExists(offeredCardId))
                return ResponseManager.BadRequest($"the offered card {offeredCardId} does not exist");

            if (!cards.InStack(username, offeredCardId))
                return ResponseManager.BadRequest($"user {username} is the not the owner of offered card {offeredCardId}");

            var offeredCard = cards.GetCard(offeredCardId);

            if (new DecksRepository().HasCardInDeck(username, offeredCardId))
                return ResponseManager.BadRequest($"card {deal.CardId} is in the deck for {username}. You can't trade cards in the deck");

            if (OfferMeatsDealRequirements(offeredCard, deal))
            {
                cards.TransferOwnership(deal.OwnerId, offeredCardId);
                cards.TransferOwnership(username, deal.CardId);
                dealer.RemoveDeal(deal.Id);

                return ResponseManager.Created($"trade deal successfully closed.");
            }

            return ResponseManager.BadRequest($"Deal requirements not met!");
        }

        private bool OfferMeatsDealRequirements(CardData offeredCard, TradeDeal deal)
        {
            // check constraints
            if (offeredCard.Damage < deal.MinimumDamage) return false;
            if (offeredCard.Weakness > deal.MaximumWeakness) return false;
            if (!ElementTypeForTradeIsOk(offeredCard.Name, deal.ElementType)) return false;
            if (!CardTypeForTradeIsOk(offeredCard.Name, deal.CardType)) return false;

            return true;
        }

        private bool CardTypeForTradeIsOk(string offeredCardName, string requiredCardType)
        {
            if (requiredCardType.IsNullOrWhiteSpace() || requiredCardType.ToLower() == "any")
                return true;

            CardType offeredCardType = CardsManager.ExtractCardType(offeredCardName);
            if (offeredCardType.ToString().ToLower() == requiredCardType.ToLower())
                return true;

            if (requiredCardType.ToLower() == "monster")
                return offeredCardType == CardType.Monster;

            MonsterType monsterType = CardsManager.ExtractMonsterType(offeredCardName);
            return requiredCardType.ToLower() == monsterType.ToString().ToLower();
        }

        private bool ElementTypeForTradeIsOk(string offeredCardName, string requiredElementType)
        {
            if (requiredElementType.IsNullOrWhiteSpace() || requiredElementType.ToLower() == "any")
                return true;

            ElementType offeredCardElementType = CardsManager.ExtractElementType(offeredCardName);
            ElementType requiredElement = CardsManager.ExtractElementType(requiredElementType);

            return offeredCardElementType == requiredElement;
        }

        private Response CreateTradeDeal(Request request)
        {
            TradeDeal deal = JsonSerializer.Deserialize<TradeDeal>(request.Payload);
            string username = Session.GetUsername(ExtractAuthorizationToken(request.Authorization));

            CardsRepository cardsRepo = new CardsRepository();
            UsersRepository usersRepo = new UsersRepository();
            TradeDealsRepository dealer = new TradeDealsRepository();
            DecksRepository decksRepo = new DecksRepository();

            if (!deal.Validate())
                return ResponseManager.BadRequest("invalid format for trade deal json object");

            if (dealer.DealExists(deal.Id))
                return ResponseManager.BadRequest($"trade deal with id {deal.Id} allready exists");

            if (username.IsNull())
                return ResponseManager.Unauthorized("No session found");

            if (!cardsRepo.CardExists(deal.CardId))
                return ResponseManager.NotFound($"card {deal.CardId} doesn't exist");

            if (!cardsRepo.InStack(username, deal.CardId))
                return ResponseManager.NotFound($"you don't own card {deal.CardId}");

            if (decksRepo.HasCardInDeck(username, deal.CardId))
                return ResponseManager.BadRequest($"card {deal.CardId} is in the deck for {username}. You can't trade cards in the deck");

            deal.OwnerId = usersRepo.GetUserID(username);
            dealer.AddDeal(deal);

            return ResponseManager.Created($"trade deal {deal.Id} created");
        }
    }
}
