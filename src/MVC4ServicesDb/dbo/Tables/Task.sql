CREATE TABLE [dbo].[Task] (
    [TaskId]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Subject]       NVARCHAR (100) NOT NULL,
    [StartDate]     DATETIME2 (7)  NULL,
    [DueDate]       DATETIME2 (7)  NULL,
    [DateCompleted] DATETIME2 (7)  NULL,
    [StatusId]      BIGINT         NOT NULL,
    [PriorityId]    BIGINT         NOT NULL,
    [CreatedDate]   DATETIME2 (7)  NOT NULL,
    [ts]            rowversion     NOT NULL,
    CONSTRAINT [PK__Task__7C6949B149D1FB5F] PRIMARY KEY CLUSTERED ([TaskId] ASC),
    CONSTRAINT [FK_Task_Priority] FOREIGN KEY ([PriorityId]) REFERENCES [dbo].[Priority] ([PriorityId]),
    CONSTRAINT [FK_Task_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([StatusId])
);

