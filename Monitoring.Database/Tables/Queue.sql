CREATE TABLE [dbo].[Queue]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Created] DATETIME2 NOT NULL DEFAULT GetDate(), 
    [Updated] DATETIME2 NOT NULL DEFAULT GetDate()
)

GO

CREATE TRIGGER [dbo].[Queue_Update]
    ON [dbo].[Queue]
    FOR UPDATE
    AS
    BEGIN
        SET NoCount ON

		UPDATE [Queue]
		SET [Queue].Updated = GETDATE()
		FROM [Queue]
		INNER JOIN inserted
			ON [Queue].Id = inserted.Id
    END