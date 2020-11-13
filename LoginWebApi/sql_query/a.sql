create table [dbo].[Messages] (
    Id INT IDENTITY PRIMARY KEY,
    Sender NVARCHAR(50),
    ToUser NVARCHAR(50),
    Message NVARCHAR(300),
    Time DATETIME,
    IsRead BIT,
    IsSend BIT,
);