CREATE SCHEMA IF NOT EXISTS public;

CREATE TABLE public.limits
(
	user_id    integer NOT NULL,
	sc_limit   integer NOT NULL,
	updated_at date    NOT NULL
);

ALTER TABLE public.limits
	OWNER TO postgres;

ALTER TABLE ONLY public.limits
	ADD CONSTRAINT limits_pk PRIMARY KEY (user_id);

CREATE UNIQUE INDEX limits_user_id_sc_limit_uindex ON public.limits USING btree (user_id, sc_limit);
CREATE UNIQUE INDEX limits_user_id_uindex ON public.limits USING btree (user_id);

CREATE TABLE public.user_social_rating_chat
(
	id         integer                     NOT NULL,
	username   character varying           NOT NULL,
	rating     integer                     NOT NULL,
	created_at timestamp without time zone NOT NULL,
	updated_at timestamp without time zone NOT NULL,
	chat_id    bigint                      NOT NULL
);

ALTER TABLE public.user_social_rating_chat
	OWNER TO postgres;

CREATE INDEX user_social_rating_chat_chat_id_idx ON public.user_social_rating_chat USING btree (chat_id);
CREATE UNIQUE INDEX user_social_rating_chat_user_id_chat_id_idx ON public.user_social_rating_chat USING btree (id, chat_id);
CREATE INDEX user_social_rating_chat_user_id_idx ON public.user_social_rating_chat USING btree (id);
