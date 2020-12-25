--
-- PostgreSQL database dump
--

-- Dumped from database version 13.0
-- Dumped by pg_dump version 13.0

-- Started on 2020-12-25 14:44:59

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
-- TOC entry 207 (class 1259 OID 17479)
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
-- TOC entry 206 (class 1259 OID 17477)
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
-- TOC entry 3058 (class 0 OID 0)
-- Dependencies: 206
-- Name: battle_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: MCTGAdmin
--

ALTER SEQUENCE public.battle_id_seq OWNED BY public.battle.id;


--
-- TOC entry 202 (class 1259 OID 17441)
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
    CONSTRAINT card_card_type_check CHECK (((card_type)::text = ANY ((ARRAY['spell'::character varying, 'dragon'::character varying, 'fireelf'::character varying, 'goblin'::character varying, 'knight'::character varying, 'kraken'::character varying, 'ork'::character varying, 'wizzard'::character varying])::text[]))),
    CONSTRAINT card_element_check CHECK (((element)::text = ANY ((ARRAY['normal'::character varying, 'fire'::character varying, 'water'::character varying])::text[])))
);


ALTER TABLE public.card OWNER TO "MCTGAdmin";

--
-- TOC entry 205 (class 1259 OID 17469)
-- Name: deck; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.deck (
    user_id integer NOT NULL,
    card_id character varying NOT NULL
);


ALTER TABLE public.deck OWNER TO "MCTGAdmin";

--
-- TOC entry 204 (class 1259 OID 17454)
-- Name: package; Type: TABLE; Schema: public; Owner: MCTGAdmin
--

CREATE TABLE public.package (
    id integer NOT NULL,
    creation_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    available integer DEFAULT 1 NOT NULL
);


ALTER TABLE public.package OWNER TO "MCTGAdmin";

--
-- TOC entry 203 (class 1259 OID 17452)
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
-- TOC entry 3059 (class 0 OID 0)
-- Dependencies: 203
-- Name: package_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: MCTGAdmin
--

ALTER SEQUENCE public.package_id_seq OWNED BY public.package.id;


--
-- TOC entry 208 (class 1259 OID 17544)
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
-- TOC entry 201 (class 1259 OID 17429)
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
-- TOC entry 200 (class 1259 OID 17427)
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
-- TOC entry 3060 (class 0 OID 0)
-- Dependencies: 200
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: MCTGAdmin
--

ALTER SEQUENCE public.user_id_seq OWNED BY public."user".id;


--
-- TOC entry 2886 (class 2604 OID 17482)
-- Name: battle id; Type: DEFAULT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle ALTER COLUMN id SET DEFAULT nextval('public.battle_id_seq'::regclass);


--
-- TOC entry 2883 (class 2604 OID 17457)
-- Name: package id; Type: DEFAULT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.package ALTER COLUMN id SET DEFAULT nextval('public.package_id_seq'::regclass);


--
-- TOC entry 2878 (class 2604 OID 17432)
-- Name: user id; Type: DEFAULT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public."user" ALTER COLUMN id SET DEFAULT nextval('public.user_id_seq'::regclass);


--
-- TOC entry 3051 (class 0 OID 17479)
-- Dependencies: 207
-- Data for Name: battle; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.battle (id, player1_id, player2_id, date, log, winner_id, rounds) FROM stdin;
\.


--
-- TOC entry 3046 (class 0 OID 17441)
-- Dependencies: 202
-- Data for Name: card; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.card (id, name, damage, weakness, owner_id, element, card_type, package_id) FROM stdin;
\.


--
-- TOC entry 3049 (class 0 OID 17469)
-- Dependencies: 205
-- Data for Name: deck; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.deck (user_id, card_id) FROM stdin;
\.


--
-- TOC entry 3048 (class 0 OID 17454)
-- Dependencies: 204
-- Data for Name: package; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.package (id, creation_date, available) FROM stdin;
\.


--
-- TOC entry 3052 (class 0 OID 17544)
-- Dependencies: 208
-- Data for Name: scoreboard; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public.scoreboard (user_id, points, battles, won_battles, lost_battles) FROM stdin;
\.


--
-- TOC entry 3045 (class 0 OID 17429)
-- Dependencies: 201
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: MCTGAdmin
--

COPY public."user" (id, username, password, bio, image, coins, name) FROM stdin;
\.


--
-- TOC entry 3061 (class 0 OID 0)
-- Dependencies: 206
-- Name: battle_id_seq; Type: SEQUENCE SET; Schema: public; Owner: MCTGAdmin
--

SELECT pg_catalog.setval('public.battle_id_seq', 65, true);


--
-- TOC entry 3062 (class 0 OID 0)
-- Dependencies: 203
-- Name: package_id_seq; Type: SEQUENCE SET; Schema: public; Owner: MCTGAdmin
--

SELECT pg_catalog.setval('public.package_id_seq', 100, true);


--
-- TOC entry 3063 (class 0 OID 0)
-- Dependencies: 200
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: MCTGAdmin
--

SELECT pg_catalog.setval('public.user_id_seq', 28, true);


--
-- TOC entry 2903 (class 2606 OID 17488)
-- Name: battle battle_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT battle_pkey PRIMARY KEY (id);


--
-- TOC entry 2897 (class 2606 OID 17451)
-- Name: card card_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.card
    ADD CONSTRAINT card_pkey PRIMARY KEY (id);


--
-- TOC entry 2901 (class 2606 OID 17476)
-- Name: deck deck_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.deck
    ADD CONSTRAINT deck_pkey PRIMARY KEY (user_id, card_id);


--
-- TOC entry 2899 (class 2606 OID 17460)
-- Name: package package_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.package
    ADD CONSTRAINT package_pkey PRIMARY KEY (id);


--
-- TOC entry 2905 (class 2606 OID 17550)
-- Name: scoreboard scoreboard_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.scoreboard
    ADD CONSTRAINT scoreboard_pkey PRIMARY KEY (user_id);


--
-- TOC entry 2893 (class 2606 OID 17438)
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 2895 (class 2606 OID 17440)
-- Name: user user_username_key; Type: CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_username_key UNIQUE (username);


--
-- TOC entry 2911 (class 2606 OID 17519)
-- Name: battle battle_loser_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT battle_loser_id_fkey FOREIGN KEY (player2_id) REFERENCES public."user"(id);


--
-- TOC entry 2910 (class 2606 OID 17514)
-- Name: battle battle_winner_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT battle_winner_id_fkey FOREIGN KEY (player1_id) REFERENCES public."user"(id);


--
-- TOC entry 2906 (class 2606 OID 17489)
-- Name: card card_owner_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.card
    ADD CONSTRAINT card_owner_id_fkey FOREIGN KEY (owner_id) REFERENCES public."user"(id);


--
-- TOC entry 2907 (class 2606 OID 17525)
-- Name: card card_package_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.card
    ADD CONSTRAINT card_package_id_fkey FOREIGN KEY (package_id) REFERENCES public.package(id);


--
-- TOC entry 2909 (class 2606 OID 17509)
-- Name: deck deck_card_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.deck
    ADD CONSTRAINT deck_card_id_fkey FOREIGN KEY (card_id) REFERENCES public.card(id);


--
-- TOC entry 2908 (class 2606 OID 17504)
-- Name: deck deck_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.deck
    ADD CONSTRAINT deck_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(id);


--
-- TOC entry 2912 (class 2606 OID 17530)
-- Name: battle fk_winnerid; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.battle
    ADD CONSTRAINT fk_winnerid FOREIGN KEY (winner_id) REFERENCES public."user"(id);


--
-- TOC entry 2913 (class 2606 OID 17551)
-- Name: scoreboard scoreboard_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: MCTGAdmin
--

ALTER TABLE ONLY public.scoreboard
    ADD CONSTRAINT scoreboard_user_id_fkey FOREIGN KEY (user_id) REFERENCES public."user"(id);


-- Completed on 2020-12-25 14:44:59

--
-- PostgreSQL database dump complete
--


