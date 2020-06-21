IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AppSettingEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Key] nvarchar(max) NOT NULL,
    [Value] nvarchar(max) NOT NULL,
    [IsReadOnly] bit NOT NULL,
    CONSTRAINT [PK_AppSettingEntity] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CustomerEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Avatar] nvarchar(max) NOT NULL,
    [Leaders] nvarchar(max) NULL,
    CONSTRAINT [PK_CustomerEntity] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [UserEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Avatar] nvarchar(max) NOT NULL,
    [SlackMember] nvarchar(max) NULL,
    CONSTRAINT [PK_UserEntity] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ProductEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Avatar] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Leaders] nvarchar(max) NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_ProductEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductEntity_CustomerEntity_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [CustomerEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [SquadEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Avatar] nvarchar(max) NULL,
    [Leaders] nvarchar(max) NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_SquadEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SquadEntity_CustomerEntity_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [CustomerEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AnchorEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Target] datetime2 NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_AnchorEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnchorEntity_ProductEntity_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [FeatureEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Avatar] nvarchar(max) NOT NULL,
    [ProductId] int NOT NULL,
    [ServiceMapId] int NOT NULL,
    CONSTRAINT [PK_FeatureEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FeatureEntity_ProductEntity_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [IncidentEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Key] nvarchar(max) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    [Tags] nvarchar(max) NULL,
    [TTD] int NOT NULL,
    [TTE] int NOT NULL,
    [TTF] int NOT NULL,
    [End] datetime2 NOT NULL,
    [Affected] int NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_IncidentEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IncidentEntity_ProductEntity_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [ServiceEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NULL,
    [AvailabilitySlo] decimal(5,3) NOT NULL,
    [ExperienceSlo] decimal(5,3) NOT NULL,
    [LatencySlo] decimal(12,3) NOT NULL,
    [Avatar] nvarchar(max) NOT NULL,
    [Leaders] nvarchar(max) NULL,
    [Group] nvarchar(max) NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_ServiceEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ServiceEntity_ProductEntity_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [SourceEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Tags] nvarchar(max) NULL,
    [GoodDefinition] nvarchar(max) NOT NULL,
    [TotalDefinition] nvarchar(max) NOT NULL,
    [Avatar] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Kind] int NOT NULL,
    [Group] int NOT NULL,
    [Percentile] decimal(18,2) NOT NULL,
    [ProductId] int NOT NULL,
    [Latency] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_SourceEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SourceEntity_ProductEntity_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [MemberEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [SquadId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_MemberEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MemberEntity_SquadEntity_SquadId] FOREIGN KEY ([SquadId]) REFERENCES [SquadEntity] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MemberEntity_UserEntity_UserId] FOREIGN KEY ([UserId]) REFERENCES [UserEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [SquadFeatureEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [FeatureId] int NOT NULL,
    [SquadId] int NOT NULL,
    CONSTRAINT [PK_SquadFeatureEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SquadFeatureEntity_FeatureEntity_FeatureId] FOREIGN KEY ([FeatureId]) REFERENCES [FeatureEntity] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SquadFeatureEntity_SquadEntity_SquadId] FOREIGN KEY ([SquadId]) REFERENCES [SquadEntity] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [IncidentMapEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [FeatureId] int NOT NULL,
    [IncidentId] int NOT NULL,
    CONSTRAINT [PK_IncidentMapEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IncidentMapEntity_FeatureEntity_FeatureId] FOREIGN KEY ([FeatureId]) REFERENCES [FeatureEntity] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_IncidentMapEntity_IncidentEntity_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [IncidentEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [ServiceMapEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [ServiceId] int NOT NULL,
    [FeatureId] int NOT NULL,
    CONSTRAINT [PK_ServiceMapEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ServiceMapEntity_FeatureEntity_FeatureId] FOREIGN KEY ([FeatureId]) REFERENCES [FeatureEntity] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ServiceMapEntity_ServiceEntity_ServiceId] FOREIGN KEY ([ServiceId]) REFERENCES [ServiceEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [IndicatorEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [SourceId] int NOT NULL,
    [FeatureId] int NOT NULL,
    [Avatar] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_IndicatorEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IndicatorEntity_FeatureEntity_FeatureId] FOREIGN KEY ([FeatureId]) REFERENCES [FeatureEntity] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_IndicatorEntity_SourceEntity_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [SourceEntity] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [SourceItemEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [SourceId] int NOT NULL,
    [Good] int NULL,
    [Total] int NULL,
    [Target] datetime2 NOT NULL,
    [Measure] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_SourceItemEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SourceItemEntity_SourceEntity_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [SourceEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [ClueEntity] (
    [Id] int NOT NULL IDENTITY,
    [CreatedOn] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [ModifiedOn] datetime2 NOT NULL,
    [ModifiedBy] nvarchar(max) NOT NULL,
    [SourceItemId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_ClueEntity] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ClueEntity_SourceItemEntity_SourceItemId] FOREIGN KEY ([SourceItemId]) REFERENCES [SourceItemEntity] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AnchorEntity_ProductId] ON [AnchorEntity] ([ProductId]);

GO

CREATE INDEX [IX_ClueEntity_SourceItemId] ON [ClueEntity] ([SourceItemId]);

GO

CREATE UNIQUE INDEX [IX_CustomerEntity_Name] ON [CustomerEntity] ([Name]);

GO

CREATE UNIQUE INDEX [IX_FeatureEntity_ProductId_Name] ON [FeatureEntity] ([ProductId], [Name]);

GO

CREATE INDEX [IX_IncidentEntity_ProductId] ON [IncidentEntity] ([ProductId]);

GO

CREATE INDEX [IX_IncidentMapEntity_FeatureId] ON [IncidentMapEntity] ([FeatureId]);

GO

CREATE INDEX [IX_IncidentMapEntity_IncidentId] ON [IncidentMapEntity] ([IncidentId]);

GO

CREATE INDEX [IX_IndicatorEntity_SourceId] ON [IndicatorEntity] ([SourceId]);

GO

CREATE UNIQUE INDEX [IX_IndicatorEntity_FeatureId_SourceId] ON [IndicatorEntity] ([FeatureId], [SourceId]);

GO

CREATE INDEX [IX_MemberEntity_SquadId] ON [MemberEntity] ([SquadId]);

GO

CREATE INDEX [IX_MemberEntity_UserId] ON [MemberEntity] ([UserId]);

GO

CREATE UNIQUE INDEX [IX_ProductEntity_CustomerId_Name] ON [ProductEntity] ([CustomerId], [Name]);

GO

CREATE UNIQUE INDEX [IX_ServiceEntity_ProductId_Name] ON [ServiceEntity] ([ProductId], [Name]);

GO

CREATE INDEX [IX_ServiceMapEntity_FeatureId] ON [ServiceMapEntity] ([FeatureId]);

GO

CREATE UNIQUE INDEX [IX_ServiceMapEntity_ServiceId_FeatureId] ON [ServiceMapEntity] ([ServiceId], [FeatureId]);

GO

CREATE UNIQUE INDEX [IX_SourceEntity_ProductId_Name] ON [SourceEntity] ([ProductId], [Name]);

GO

CREATE INDEX [IX_SourceItemEntity_SourceId] ON [SourceItemEntity] ([SourceId]);

GO

CREATE INDEX [IX_SourceItemEntity_Target] ON [SourceItemEntity] ([Target]);

GO

CREATE UNIQUE INDEX [IX_SquadEntity_CustomerId_Name] ON [SquadEntity] ([CustomerId], [Name]);

GO

CREATE INDEX [IX_SquadFeatureEntity_FeatureId] ON [SquadFeatureEntity] ([FeatureId]);

GO

CREATE INDEX [IX_SquadFeatureEntity_SquadId] ON [SquadFeatureEntity] ([SquadId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200617193330_InitialCreate', N'3.1.0');

GO

