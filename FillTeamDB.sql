USE [ StacktimDb]
GO

INSERT INTO [dbo].[Teams]
           ([Name]
           ,[Tag]
           ,[CaptainId])
     VALUES
           ('molusque','MOL',(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'timmy')),
		   ('stomp','STO',(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'stomp')),
		   ('GenericTeamName','GTN',(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'outofidea'))
GO


