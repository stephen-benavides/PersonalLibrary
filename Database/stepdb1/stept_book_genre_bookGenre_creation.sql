USE[stepdb1]
DROP TABLE Genre 
DROP TABLE BookGenre
DROP TABLE Book 

/* GENRE */
-- Create Genre table
CREATE TABLE Genre (
    GenreId INT IDENTITY(1,1) PRIMARY KEY,
    GenreName VARCHAR(255) NOT NULL
);

/* BOOK */
-- Create Book table
CREATE TABLE Book (
    BookId INT IDENTITY(1,1) PRIMARY KEY NOT NULL , --Identity creates an increment starting from 1 and adding 1 every timefor each new row 
    Title VARCHAR(255) NOT NULL,
    Author VARCHAR(255) NOT NULL,
    PublicationYear INT
	--Creating a constraint in the book table to reference the GenreId 
	--CONSTRAINT FK_Book_Genre FOREIGN KEY (GenreId) REFERENCES Genre(GenreId)
);
/*NO NEED FOR THIS IF IDENTITY IS SET DURING THE TABLE CREATION*/
/*Creating Sequence To Increment the BookId by 1 automatically*/
CREATE SEQUENCE BookIdSequence 
	START WITH 1 
	INCREMENT BY 1 
	MINVALUE 1 
	NO MAXVALUE 
	CACHE 20; -- max CACHE is 50 (default) 

ALTER TABLE Book ADD BookIdIncrement INT DEFAULT  NEXT VALUE FOR BookIdSequence PRIMARY KEY;


/* BOOKGENRE */
-- Create a junction table to represent the many-to-many relationship
CREATE TABLE BookGenre (
    BookId INT,
    GenreId INT,
    PRIMARY KEY (BookId, GenreId),
    FOREIGN KEY (BookId) REFERENCES Book(BookId),
    FOREIGN KEY (GenreId) REFERENCES Genre(GenreId)
);

--So, when removing a book, the entire thing will be deleted from the BookGenre table automatically 
ALTER TABLE BookGenre
ADD CONSTRAINT FK_BookGenre_BookId
FOREIGN KEY (BookId)
REFERENCES Book(BookId)
ON DELETE CASCADE;



/*
	Initial Insertion 
*/
INSERT INTO Genre (GenreName)
VALUES
    ('Science Fiction'),
    ('Mystery'),
    ('Fantasy'),
    ('Romance');

-- Insert books with corresponding genre IDs
INSERT INTO Book (Title, Author, PublicationYear)
VALUES
    ('Dune', 'Frank Herbert', 1965),
    ('The Hitchhiker''s Guide to the Galaxy', 'Douglas Adams', 1979),
    ('The Da Vinci Code', 'Dan Brown', 2003),
    ('The Lord of the Rings', 'J.R.R. Tolkien', 1954),
    ('Pride and Prejudice', 'Jane Austen', 1813);


-- Associate books with genres
INSERT INTO BookGenre (BookId, GenreId)
VALUES
    (1,2),
	(1,3),
	(1,4)


