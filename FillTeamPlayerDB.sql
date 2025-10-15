USE [StacktimDb]
GO

INSERT INTO [dbo].[TeamPlayers]
           ([TeamId]
           ,[PlayerId]
           ,[Role])
     VALUES
           ((select [dbo].[Teams].[ID] from [dbo].[Teams] where Name = 'molusque'),(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'timmy'),0),
		   ((select [dbo].[Teams].[ID] from [dbo].[Teams] where Name = 'molusque'),(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'Abez'),1),
		   ((select [dbo].[Teams].[ID] from [dbo].[Teams] where Name = 'stomp'),(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'stomp'),0),
		   ((select [dbo].[Teams].[ID] from [dbo].[Teams] where Name = 'stomp'),(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'PlatinumGod'),1),
		   ((select [dbo].[Teams].[ID] from [dbo].[Teams] where Name = 'GenericTeamName'),(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'outofidea'),0),
		   ((select [dbo].[Teams].[ID] from [dbo].[Teams] where Name = 'GenericTeamName'),(select [dbo].[Players].[ID] from [dbo].[Players] where PSEUDO = 'uncreativeName'),1)

GO


