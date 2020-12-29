--
-- PostgreSQL database dump
--

-- Dumped from database version 13.0
-- Dumped by pg_dump version 13.0

-- Started on 2020-12-29 11:51:00

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 200 (class 1259 OID 17788)
-- Name: battle; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.battle (
    id integer NOT NULL,
    player1_id integer,
    player2_id integer,
    date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    log text,
    winner_id integer,
    rounds bigint
);


ALTER TABLE public.battle OWNER TO "MCTGAdmin";

--
-- TOC entry 201 (class 1259 OID 17795)
-- Name: battle_id_seq; Type: SEQUENCE; Schema: public; Owner: MCTGAdmin
--

CREATE SEQUENCE public.battle_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.battle_id_seq OWNER TO "MCTGAdmin";

--
-- TOC entry 3070 (class 0 OID 0)
-- Dependencies: 201
-- Name: battle_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: MCTGAdmin
--

ALTER SEQUENCE public.battle_id_seq OWNED BY public.battle.id;


--
-- TOC entry 202 (class 1259 OID 17797)
-- Name: card; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.card (
    id character varying NOT NULL,
    name character varying,
    damage double precision,
    weakness double precision DEFAULT 0,
    owner_id integer,
    element character varying,
    card_type character varying,
    package_id integer,
    CONSTRAINT card_card_type_check CHECK (((card_type)::text = ANY (ARRAY[('spell'::character varying)::text, ('dragon'::character varying)::text, ('fireelf'::character varying)::text, ('goblin'::character varying)::text, ('knight'::character varying)::text, ('kraken'::character varying)::text, ('ork'::character varying)::text, ('wizzard'::character varying)::text]))),
    CONSTRAINT card_element_check CHECK (((element)::text = ANY (ARRAY[('normal'::character varying)::text, ('fire'::character varying)::text, ('water'::character varying)::text])))
);


ALTER TABLE public.card OWNER TO "MCTGAdmin";

--
-- TOC entry 203 (class 1259 OID 17806)
-- Name: deck; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.deck (
    user_id integer NOT NULL,
    card_id character varying NOT NULL
);


ALTER TABLE public.deck OWNER TO "MCTGAdmin";

--
-- TOC entry 204 (class 1259 OID 17812)
-- Name: package; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.package (
    id integer NOT NULL,
    creation_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    available integer DEFAULT 1 NOT NULL
);


ALTER TABLE public.package OWNER TO "MCTGAdmin";

--
-- TOC entry 205 (class 1259 OID 17817)
-- Name: package_id_seq; Type: SEQUENCE; Schema: public; Owner: MCTGAdmin
--

CREATE SEQUENCE public.package_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.package_id_seq OWNER TO "MCTGAdmin";

--
-- TOC entry 3071 (class 0 OID 0)
-- Dependencies: 205
-- Name: package_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: MCTGAdmin
--

ALTER SEQUENCE public.package_id_seq OWNED BY public.package.id;


--
-- TOC entry 206 (class 1259 OID 17819)
-- Name: scoreboard; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.scoreboard (
    user_id integer NOT NULL,
    points integer DEFAULT 100,
    battles integer DEFAULT 0,
    won_battles bigint DEFAULT 0,
    lost_battles bigint DEFAULT 0
);


ALTER TABLE public.scoreboard OWNER TO "MCTGAdmin";

--
-- TOC entry 209 (class 1259 OID 25727)
-- Name: trade_deal; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.trade_deal (
    id character varying NOT NULL,
    owner_id integer NOT NULL,
    card_id character varying,
    min_damage double precision,
    max_weakness double precision,
    element character varying,
    card_type character varying,
    CONSTRAINT trade_deal_card_type_check CHECK (((card_type)::text = ANY ((ARRAY['spell'::character varying, 'dragon'::character varying, 'fireelf'::character varying, 'goblin'::character varying, 'knight'::character varying, 'kraken'::character varying, 'ork'::character varying, 'wizzard'::character varying, 'monster'::character varying, 'any'::character varying])::text[]))),
    CONSTRAINT trade_deal_element_check CHECK (((element)::text = ANY ((ARRAY['normal'::character varying, 'fire'::character varying, 'water'::character varying, 'any'::character varying])::text[])))
);


ALTER TABLE public.trade_deal OWNER TO "MCTGAdmin";

--
-- TOC entry 207 (class 1259 OID 17826)
-- Name: user; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    username character varying,
    password character varying,
    bio text,
    image character varying,
    coins integer DEFAULT 20,
    name character varying
);


ALTER TABLE public."user" OWNER TO "MCTGAdmin";

--
-- TOC entry 208 (class 1259 OID 17833)
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: MCTGAdmin
--

CREATE SEQUENCE public.user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_id_seq OWNER TO "MCTGAdmin";

--
-- TOC entry 3072 (class 0 OID 0)
-- Dependencies: 208
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: MCTGAdmin
--

ALTER SEQUENCE public.user_id_seq OWNED BY public."user".id;


--
-- TOC entry 2884 (class 2604 OID 17835)
-- Name: battle id; Type: DEFAULT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle ALTER COLUMN id SET DEFAULT nextval('public.battle_id_seq'::regclass);


--
-- TOC entry 2890 (class 2604 OID 17836)
-- Name: package id; Type: DEFAULT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.package ALTER COLUMN id SET DEFAULT nextval('public.package_id_seq'::regclass);


--
-- TOC entry 2896 (class 2604 OID 17837)
-- Name: user id; Type: DEFAULT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public."user" ALTER COLUMN id SET DEFAULT nextval('public.user_id_seq'::regclass);


--
-- TOC entry 3055 (class 0 OID 17788)
-- Dependencies: 200
-- Data for Name: battle; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.battle (id, player1_id, player2_id, date, log, winner_id, rounds) FROM stdin;
\.


--
-- TOC entry 3057 (class 0 OID 17797)
-- Dependencies: 202
-- Data for Name: card; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.card (id, name, damage, weakness, owner_id, element, card_type, package_id) FROM stdin;
\.


--
-- TOC entry 3058 (class 0 OID 17806)
-- Dependencies: 203
-- Data for Name: deck; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.deck (user_id, card_id) FROM stdin;
\.


--
-- TOC entry 3059 (class 0 OID 17812)
-- Dependencies: 204
-- Data for Name: package; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.package (id, creation_date, available) FROM stdin;
\.


--
-- TOC entry 3061 (class 0 OID 17819)
-- Dependencies: 206
-- Data for Name: scoreboard; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.scoreboard (user_id, points, battles, won_battles, lost_battles) FROM stdin;
\.


--
-- TOC entry 3064 (class 0 OID 25727)
-- Dependencies: 209
-- Data for Name: trade_deal; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.trade_deal (id, owner_id, card_id, min_damage, max_weakness, element, card_type) FROM stdin;
\.


--
-- TOC entry 3062 (class 0 OID 17826)
-- Dependencies: 207
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public."user" (id, username, password, bio, image, coins, name) FROM stdin;
\.


--
-- TOC entry 3073 (class 0 OID 0)
-- Dependencies: 201
-- Name: battle_id_seq; Type: SEQUENCE SET; Schema: public; Owner: MCTGAdmin
--

SELECT pg_catalog.setval('public.battle_id_seq', 122, true);


--
-- TOC entry 3074 (class 0 OID 0)
-- Dependencies: 205
-- Name: package_id_seq; Type: SEQUENCE SET; Schema: public; Owner: MCTGAdmin
--

SELECT pg_catalog.setval('public.package_id_seq', 343, true);


--
-- TOC entry 3075 (class 0 OID 0)
-- Dependencies: 208
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: MCTGAdmin
--

SELECT pg_catalog.setval('public.user_id_seq', 109, true);


--
-- TOC entry 2900 (class 2606 OID 17839)
-- Name: battle battle_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT battle_pkey PRIMARY KEY (id);


--
-- TOC entry 2902 (class 2606 OID 17841)
-- Name: card card_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.card
    ADD CONSTRAINT card_pkey PRIMARY KEY (id);


--
-- TOC entry 2904 (class 2606 OID 17843)
-- Name: deck deck_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.deck
    ADD CONSTRAINT deck_pkey PRIMARY KEY (user_id, card_id);


--
-- TOC entry 2906 (class 2606 OID 17845)
-- Name: package package_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.package
    ADD CONSTRAINT package_pkey PRIMARY KEY (id);


--
-- TOC entry 2908 (class 2606 OID 17847)
-- Name: scoreboard scoreboard_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.scoreboard
    ADD CONSTRAINT scoreboard_pkey PRIMARY KEY (user_id);


--
-- TOC entry 2914 (class 2606 OID 25736)
-- Name: trade_deal trade_deal_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.trade_deal
    ADD CONSTRAINT trade_deal_pkey PRIMARY KEY (id);


--
-- TOC entry 2910 (class 2606 OID 17849)
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 2912 (class 2606 OID 17851)
-- Name: user user_username_key; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_username_key UNIQUE (username);


--
-- TOC entry 2915 (class 2606 OID 17852)
-- Name: battle battle_loser_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT battle_loser_id_fkey FOREIGN KEY (player2_id) REFERENCES public."user"(id);


--
-- TOC entry 2916 (class 2606 OID 17857)
-- Name: battle battle_winner_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT battle_winner_id_fkey FOREIGN KEY (player1_id) REFERENCES public."user"(id);


--
-- TOC entry 2918 (class 2606 OID 17862)
-- Name: card card_owner_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.card
    ADD CONSTRAINT card_owner_id_fkey FOREIGN KEY (owner_id) REFERENCES public."user"(id);


--
-- TOC entry 2919 (class 2606 OID 17867)
-- Name: card card_package_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.card
    ADD CONSTRAINT card_package_id_fkey FOREIGN KEY (package_id) REFERENCES public.package(id);


--
-- TOC entry 2920 (class 2606 OID 17872)
-- Name: deck deck_card_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.deck
    ADD CONSTRAINT deck_card_id_fkey FOREIGN KEY (card_id) REFERENCES public.card(id);


--
-- TOC entry 2921 (class 2606 OID 17877)
-- Name: deck deck_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.deck
    ADD CONSTRAINT deck_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(id);


--
-- TOC entry 2924 (class 2606 OID 25742)
-- Name: trade_deal fk_card_id_trade; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.trade_deal
    ADD CONSTRAINT fk_card_id_trade FOREIGN KEY (card_id) REFERENCES public.card(id);


--
-- TOC entry 2923 (class 2606 OID 25737)
-- Name: trade_deal fk_owner_id; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.trade_deal
    ADD CONSTRAINT fk_owner_id FOREIGN KEY (owner_id) REFERENCES public."user"(id);


--
-- TOC entry 2917 (class 2606 OID 17882)
-- Name: battle fk_winnerid; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT fk_winnerid FOREIGN KEY (winner_id) REFERENCES public."user"(id);


--
-- TOC entry 2922 (class 2606 OID 17887)
-- Name: scoreboard scoreboard_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.scoreboard
    ADD CONSTRAINT scoreboard_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(id);


-- Completed on 2020-12-29 11:51:00

--
-- PostgreSQL database dump complete
--

