CREATE SCHEMA library;

CREATE TABLE library.Users (
    id SERIAL PRIMARY KEY,
    login VARCHAR(255) NOT NULL UNIQUE,
    passwordHash TEXT NOT NULL,
    role TEXT NOT NULL CHECK (role IN ('Teacher', 'Student')),
    fullName TEXT NOT NULL,
    sessionId VARCHAR(64) -- Абсолютно от балды взятое число
);

CREATE TABLE library.Tests (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    description TEXT,
    authorId INTEGER NOT NULL,
    createdAt TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (authorId) REFERENCES library.Users(id) ON DELETE CASCADE
);

CREATE TABLE library.Questions (
    id SERIAL PRIMARY KEY,
    testId INTEGER NOT NULL,
    text TEXT NOT NULL,
    correctAnswer TEXT NOT NULL,
    FOREIGN KEY (testId) REFERENCES library.Tests(id) ON DELETE CASCADE
);

CREATE TABLE library.Answers (
    id SERIAL PRIMARY KEY,
    questionId INTEGER NOT NULL,
    studentId INTEGER NOT NULL,
    givenAnswer TEXT NOT NULL,
    isCorrect BOOLEAN NOT NULL,
    FOREIGN KEY (questionId) REFERENCES library.Questions(id) ON DELETE CASCADE,
    FOREIGN KEY (studentId) REFERENCES library.Users(id) ON DELETE CASCADE
);

CREATE TABLE library.Attempts (
    id SERIAL PRIMARY KEY,
    testId INTEGER NOT NULL,
    studentId INTEGER NOT NULL,
    score INTEGER NOT NULL CHECK (score >= 0),
    dateUtc TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (testId) REFERENCES library.Tests(id) ON DELETE CASCADE,
    FOREIGN KEY (studentId) REFERENCES library.Users(id) ON DELETE CASCADE
);