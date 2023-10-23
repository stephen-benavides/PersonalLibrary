IF EXISTS (SELECT * FROM sys.types WHERE name = 'BookGenreList')
BEGIN
	DROP TYPE BookGenreList
	PRINT '<<<DROPPED TYPE BookGenreList >>>'
END 

GO
CREATE TYPE BookGenreList AS TABLE
(
    BookId INT,
    GenreId INT
)

GO
IF EXISTS (SELECT * FROM sys.types WHERE name = 'BookGenreList')
BEGIN
	PRINT '<<<CREATED  TYPE BookGenreList >>>'
END 

