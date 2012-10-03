CREATE TABLE [dbo].[TaskCategory] (
    [TaskId]     BIGINT     NOT NULL,
    [CategoryId] BIGINT     NOT NULL,
    [ts]         rowversion NOT NULL,
    CONSTRAINT [pk_TaskCategory] PRIMARY KEY CLUSTERED ([TaskId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_TaskCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId]),
    CONSTRAINT [FK_TaskCategory_Task] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Task] ([TaskId])
);

