CREATE TABLE IF NOT EXISTS "user" (
  "id" SERIAL PRIMARY KEY,
  "username" varchar UNIQUE,
  "password" varchar,
  "coins" int DEFAULT 20
);

CREATE TABLE IF NOT EXISTS "card" (
  "id" varchar PRIMARY KEY,
  "name" varchar,
  "damage" double precision,
  "weakness" double precision DEFAULT 0,
  "owner_id" int DEFAULT null
);

CREATE TABLE IF NOT EXISTS "user_card" (
  "user_id" int,
  "card_id" varchar NOT NULL
);

CREATE TABLE IF NOT EXISTS "battle" (
  "id" SERIAL PRIMARY KEY,
  "player1_id" int,
  "player2_id" int,
  "winner_id" int,
  "log" text
);

ALTER TABLE "card" ADD FOREIGN KEY ("owner_id") REFERENCES "user" ("id");

ALTER TABLE "user_card" ADD FOREIGN KEY ("user_id") REFERENCES "user" ("id");

ALTER TABLE "user_card" ADD FOREIGN KEY ("card_id") REFERENCES "card" ("id");

ALTER TABLE "battle" ADD FOREIGN KEY ("player1_id") REFERENCES "user" ("id");

ALTER TABLE "battle" ADD FOREIGN KEY ("player2_id") REFERENCES "user" ("id");

ALTER TABLE "battle" ADD FOREIGN KEY ("winner_id") REFERENCES "user" ("id");
