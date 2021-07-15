-- Database: OnePlatform

-- DROP DATABASE "OnePlatform";

CREATE DATABASE "OnePlatform"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_India.1252'
    LC_CTYPE = 'English_India.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;
________________________________________________	

-- SCHEMA: op

-- DROP SCHEMA op ;

CREATE SCHEMA op
    AUTHORIZATION postgres;

-----------------------------------------------

-- Table: op.customer

-- DROP TABLE op.customer;

CREATE TABLE IF NOT EXISTS op.customer
(
    name text COLLATE pg_catalog."default",
    email text COLLATE pg_catalog."default",
    phone text COLLATE pg_catalog."default",
    address text COLLATE pg_catalog."default",
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 )
)

TABLESPACE pg_default;

ALTER TABLE op.customer
    OWNER to postgres;


----------------------------------------

-- PROCEDURE: op.spcustomerinsert(text, text, text, text, integer)

-- DROP PROCEDURE op.spcustomerinsert(text, text, text, text, integer);

CREATE OR REPLACE PROCEDURE op.spcustomerinsert(
	name text,
	email text,
	phone text,
	address text,
	INOUT id integer)
LANGUAGE 'sql'
AS $BODY$
INSERT INTO op.customer(
	name, email, phone, address)
	VALUES (name, email, phone, address) RETURNING id;
$BODY$;


-----------------------------


-- PROCEDURE: op.spcustomerselect(integer)

-- DROP PROCEDURE op.spcustomerselect(integer);

CREATE OR REPLACE PROCEDURE op.spcustomerselect(
	id integer)
LANGUAGE 'sql'
AS $BODY$
SELECT id, name, email, phone, address FROM op.customer WHERE id = id;
$BODY$;
