USE [DMA-CSD-S241_10632087];
GO

-------------------------
-- COUNTRY
-------------------------
INSERT INTO dbo.Country (Country)
VALUES  ('Denmark'),
        ('Sweden'),
        ('Norway'),
        ('Germany'),
        ('France');
GO

-------------------------
-- CITY
-------------------------
INSERT INTO dbo.City (City, FK_Country_ID)
VALUES  ('Copenhagen', 1),
        ('Aarhus', 1),
        ('Stockholm', 2),
        ('Gothenburg', 2),
        ('Oslo', 3),
        ('Bergen', 3),
        ('Berlin', 4),
        ('Hamburg', 4),
        ('Paris', 5),
        ('Lyon', 5);
GO

-------------------------
-- INTERESTS
-------------------------
INSERT INTO dbo.Interests (InterestName)
VALUES ('Football'),
       ('Cooking'),
       ('Reading'),
       ('Gaming'),
       ('Hiking'),
       ('Movies'),
       ('Music'),
       ('Travel'),
       ('Fitness'),
       ('Photography');
GO

-------------------------
-- PROFILE
-------------------------
INSERT INTO dbo.Profile
    (Email, HashPassword, Salt, ProfileType, FirstName, MiddleName, Surname, PhoneNo, Age, Relationship, Description, FK_City_ID, StreetName, StreetNumber, ZipCode)
VALUES
    ('anna@test.com', 'HASH1', 'SALT1', 'User', 'Anna', NULL, 'Jensen', '12345678', 25, 'Single', 'Love hiking and movies.', 1, 'Gaden', '1', '9000'),
    ('mikkel@test.com', 'HASH2', 'SALT2', 'User', 'Mikkel', 'B.', 'Hansen', '22334455', 31, 'Single', 'Gamer and reader.', 2, 'Gaden', '1', '9000'),
    ('sara@test.com', 'HASH3', 'SALT3', 'User', 'Sara', NULL, 'Olsen', '33445566', 28, 'Taken', 'Enjoys cooking and fitness.', 3, 'Gaden', '1', '9000'),
    ('jonas@test.com', 'HASH4', 'SALT4', 'User', 'Jonas', 'M.', 'Nielsen', '44556677', 35, 'Single', 'Music lover.', 4, 'Gaden', '1', '9000'),
    ('laura@test.com', 'HASH5', 'SALT5', 'User', 'Laura', NULL, 'Poulsen', '55667788', 22, 'Single', 'Fitness enthusiast.', 5, 'Gaden', '1', '9000'),
    ('emil@test.com', 'HASH6', 'SALT6', 'User', 'Emil', NULL, 'Thomsen', '66778899', 29, 'Taken', 'Avid traveler.', 6, 'Gaden', '1', '9000'),
    ('victor@test.com', 'HASH7', 'SALT7', 'User', 'Victor', 'A.', 'Kristensen', '77889900', 33, 'Single', 'Photography hobbyist.', 7, 'Gaden', '1', '9000'),
    ('ida@test.com', 'HASH8', 'SALT8', 'User', 'Ida', NULL, 'Andersen', '88990011', 27, 'Single', 'Football fan.', 8, 'Gaden', '1', '9000'),
    ('marcus@test.com', 'HASH9', 'SALT9', 'User', 'Marcus', NULL, 'Petersen', '99001122', 24, 'Single', 'Loves reading.', 9, 'Gaden', '1', '9000'),
    ('sofie@test.com', 'HASH10', 'SALT10', 'User', 'Sofie', 'M.', 'Holm', '10011002', 30, 'Taken', 'Cooking and movies.', 10, 'Gaden', '1', '9000');
GO

-------------------------
-- PROFILE INTERESTS
-------------------------
INSERT INTO dbo.ProfileInterests (ProfileID, InterestsID)
VALUES
    (1, 5), (1, 6),
    (2, 4), (2, 3),
    (3, 2), (3, 9),
    (4, 7),
    (5, 9), (5, 10),
    (6, 8),
    (7, 10), (7, 7),
    (8, 1),
    (9, 3),
    (10, 2), (10, 6);
GO
