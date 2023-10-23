USE [stepdb1]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[stepsp_common_AddNewBook]'))
BEGIN
	DROP PROCEDURE stepsp_common_AddNewBook
	PRINT '<<<DROPPED PROCEDURE stepsp_common_AddNewBook >>>'
END 

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO 

/*************************************************************************
* TYPE:			STORED PROCEDURE 
* NAME:			stepsp_common_AddNewBook
* PURPOSE		To add a new book into the book table 
*
*
*
*
* AUTHOR		DATE			VER		DESCRIPTION 
* ------		----			---		-----------
* Stephen		10/03/2023		1.0		Initial Version
**************************************************************************/

CREATE PROCEDURE stepsp_common_AddNewBook
(
	 @Title				VARCHAR(200)
	,@Author			VARCHAR(200) 
	,@PublicationYear	INT
)

AS
BEGIN
	BEGIN TRY 
		INSERT INTO Book (Title, Author, PublicationYear)
		VALUES 
		(
			 @Title			
			,@Author		
			,@PublicationYear
		)
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
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[stepsp_common_AddNewBook]'))
BEGIN
	PRINT '<<<CREATED PROCEDURE stepsp_common_AddNewBook >>>'
END 
