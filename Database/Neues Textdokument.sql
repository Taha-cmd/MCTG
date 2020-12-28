CREATE TABLE IF NOT EXISTS "trade_deal" (
  "id" varchar PRIMARY KEY,
  "owner_id" int NOT NULL,
  "card_id" varchar,
  "min_damage" double precision,
  "max_weakness" double precision DEFAULT NULL,
  "element" varchar,
  "card_type" varchar,
  CHECK ("element" IN ('normal', 'fire', 'water', 'any') ),
  CHECK ("card_type" IN ('spell', 'dragon', 'fireelf', 'goblin', 'knight', 'kraken', 'ork', 'wizzard', 'monster', 'any') )
);

ALTER TABLE ONLY public."trade_deal"
    ADD CONSTRAINT fk_owner_id FOREIGN KEY (owner_id) REFERENCES public."user"(id);
	
ALTER TABLE ONLY public."trade_deal"
    ADD CONSTRAINT fk_card_id_trade FOREIGN KEY (card_id) REFERENCES public."card"(id);