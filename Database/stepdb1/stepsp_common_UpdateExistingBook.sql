USE [stepdb1]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[stepsp_common_UpdateExistingBook]'))
BEGIN
	DROP PROCEDURE stepsp_common_UpdateExistingBook
	PRINT '<<<DROPPED PROCEDURE stepsp_common_UpdateExistingBook >>>'
END 

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO 

/*************************************************************************
* TYPE:			STORED PROCEDURE 
* NAME:			stepsp_common_UpdateExistingBook
* PURPOSE		To update an existing book in the book table
*
*
*
*
* AUTHOR		DATE			VER		DESCRIPTION 
* ------		----			---		-----------
* Stephen		10/03/2023		1.0		Initial Version
**************************************************************************/

CREATE PROCEDURE stepsp_common_UpdateExistingBook
(
	 @BookId			INT
	,@Title				VARCHAR(200)
	,@Author			VARCHAR(200) 
	,@PublicationYear	INT
)
AS
BEGIN
	BEGIN TRY 
	
		UPDATE b
		SET 
			 b.Title = @Title
			,b.Author = @Author
			,b.PublicationYear = @PublicationYear
		FROM Book b
		WHERE b.BookId = @BookId

		SELECT * FROM Book where BookId = @BookId

	END TRY

	--TRY CATCH BLOCK 
	BEGIN CATCH 
	DECLARE 
		 @ErrorMessage		NVARCHAR(4000)
		,@ErrorSeverity		INT
		,@ErrorState		INT;
	SELECT 
		 @ErrorMessage		= ERROR_MESSAGE()
		,@ErrorSeverity		= ERROR_SEVERITY()
		,@ErrorState		= ERROR_STATE();
	RAISERROR 
	(
		 @ErrorMessage	
		,@ErrorSeverity	
		,@ErrorState	
	)
	END CATCH
END 


GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[stepsp_common_UpdateExistingBook]'))
BEGIN
	PRINT '<<<CREATED PROCEDURE stepsp_common_UpdateExistingBook >>>'
END 
