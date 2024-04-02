Use SocialMedia
GO
IF NOT EXISTS(SELECT*FROM sys.server_principals where name ='SMUser')
BEGIN
CREATE LOGIN SMUser WITH PASSWORD =N'SmPA$$06500', DEFAULT_DATABASE=SocialMedia
END

IF NOT EXISTS(SELECT*FROM sys.database_principals where name ='SMUser')
BEGIN
    exec sp_adduser 'SMUser', 'SMUser', 'db_owner';
END