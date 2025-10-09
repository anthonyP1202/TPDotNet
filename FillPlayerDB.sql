USE [StacktimDb]
GO

INSERT INTO [dbo].[Players]
           ([PSEUDO]
           ,[EMAIL]
           ,[Rank]
           ,[TotalScore])
     VALUES
           ('timmy', 'testemail@gmail.com', 'Bronze', 55),
		   ('uncreativeName', 'uncreative@email.com', 'Master', 452058),
		   ('stomp', 'oneSided@gmail.com', 'Master', 452158),
		   ('Abez', 'abez@xefi.com', 'Platinum', 2587),
		   ('PlatinumGod', 'dazz@orange.fr', 'Silver', 652),
		   ('outofidea', 'nothing@orange.fr', 'Gold', 1300)
GO




