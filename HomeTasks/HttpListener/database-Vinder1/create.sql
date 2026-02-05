-- Active: 1758114810365@@localhost@5432@oris
-- create database oris;
-- create schema holiday;

create table holiday.User (
	userId SERIAL PRIMARY KEY,
	login VARCHAR(30) NOT NULL UNIQUE,
	password_hash VARCHAR(64) NOT NULL,
	password_salt VARCHAR(64) NOT NULL,
	username VARCHAR(30),
	coins INT,
	role VARCHAR(30) NOT NULL DEFAULT 'user'
);

-- не очень оригинальное название
-- но да ладно
create table holiday.Holiday (
	holidayId SERIAL PRIMARY KEY,
	creator INT NOT NULL REFERENCES holiday.User(userId),
	celebDay DATE NOT NULL,
	name VARCHAR(30) NOT NULL
);

create table holiday.Present (
	presentId SERIAL PRIMARY KEY,
	fromId INT NOT NULL REFERENCES holiday.User(userId),
	toId INT NOT NULL REFERENCES holiday.User(userId),
	hId INT NOT NULL REFERENCES holiday.Holiday(holidayId),
	inside VARCHAR(100)
);