CREATE TABLE IF NOT EXISTS "user" (
  "id" SERIAL PRIMARY KEY,
  "username" varchar UNIQUE,
  "password" varchar,
  "bio" text DEFAULT null,
  "image" varchar DEFAULT null,
  "coins" int DEFAULT 20
  "name" varchar DEFAULT null
);

CREATE TABLE IF NOT EXISTS "card" (
  "id" varchar PRIMARY KEY,
  "name" varchar,
  "damage" double precision,
  "weakness" double precision DEFAULT 0,
  "owner_id" int DEFAULT null,
  "package_id" int,
  "element" varchar,
  "card_type" varchar,
  CHECK ("element" IN ('normal', 'fire', 'water') ),
  CHECK ("card_type" IN ('spell', 'dragon', 'fireelf', 'goblin', 'knight', 'kraken', 'ork', 'wizzard') )
);

CREATE TABLE IF NOT EXISTS "package" (
  "id" SERIAL PRIMARY KEY,
  "creation_date" timestamp DEFAULT 'now()'
  "available" int DEFAULT 0,
  CHECK ("available" IN (0,1))
);

CREATE TABLE IF NOT EXISTS "deck" (
  "user_id" int,
  "card_id" varchar NOT NULL,
  PRIMARY KEY ("user_id", "card_id")
);

CREATE TABLE IF NOT EXISTS "battle" (
  "id" SERIAL PRIMARY KEY,
  "winner_id" int,
  "loser_id" int,
  "date" timestamp DEFAULT 'now()',
  "log" text
);

ALTER TABLE "card" ADD FOREIGN KEY ("owner_id") REFERENCES "user" ("id");

ALTER TABLE "deck" ADD FOREIGN KEY ("user_id") REFERENCES "user" ("id");

ALTER TABLE "deck" ADD FOREIGN KEY ("card_id") REFERENCES "card" ("id");

ALTER TABLE "battle" ADD FOREIGN KEY ("winner_id") REFERENCES "user" ("id");

ALTER TABLE "battle" ADD FOREIGN KEY ("loser_id") REFERENCES "user" ("id");

ALTER TABLE "card" ADD FOREIGN KEY ("package_id") REFERENCES "package" ("id");

