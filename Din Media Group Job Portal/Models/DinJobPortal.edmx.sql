
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/29/2017 20:00:09
-- Generated from EDMX file: C:\Users\Dr Hasnain Ayoub\documents\visual studio 2013\Projects\Din Media Group Job Portal\Din Media Group Job Portal\Models\DinJobPortal.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DinJobPortal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_tb_employee_registration_data_tb_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tb_employee_registration_data] DROP CONSTRAINT [FK_tb_employee_registration_data_tb_user];
GO
IF OBJECT_ID(N'[dbo].[FK_tb_employer_registration_data_tb_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tb_employer_registration_data] DROP CONSTRAINT [FK_tb_employer_registration_data_tb_user];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[tb_employee_registration_data]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tb_employee_registration_data];
GO
IF OBJECT_ID(N'[dbo].[tb_employer_registration_data]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tb_employer_registration_data];
GO
IF OBJECT_ID(N'[dbo].[tb_exception]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tb_exception];
GO
IF OBJECT_ID(N'[dbo].[tb_user]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tb_user];
GO
IF OBJECT_ID(N'[dbo].[tb_verification_code]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tb_verification_code];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tb_employee_registration_data'
CREATE TABLE [dbo].[tb_employee_registration_data] (
    [id] int IDENTITY(1,1) NOT NULL,
    [email] varchar(100)  NOT NULL,
    [first_name] varchar(100)  NOT NULL,
    [last_name] varchar(100)  NOT NULL,
    [gender] varchar(100)  NOT NULL,
    [location] varchar(100)  NOT NULL,
    [job_catagory_field] varchar(100)  NOT NULL,
    [job_title] varchar(max)  NOT NULL,
    [last_login] datetime  NULL,
    [job_interest] varchar(max)  NOT NULL,
    [user_id] int  NOT NULL
);
GO

-- Creating table 'tb_employer_registration_data'
CREATE TABLE [dbo].[tb_employer_registration_data] (
    [id] int IDENTITY(1,1) NOT NULL,
    [email] varchar(100)  NOT NULL,
    [first_name] varchar(100)  NOT NULL,
    [last_name] varchar(100)  NOT NULL,
    [company_name] varchar(100)  NOT NULL,
    [mobile] varchar(20)  NOT NULL,
    [cnic] varchar(20)  NULL,
    [last_login] datetime  NULL,
    [user_id] int  NOT NULL
);
GO

-- Creating table 'tb_user'
CREATE TABLE [dbo].[tb_user] (
    [id] int IDENTITY(1,1) NOT NULL,
    [email] varchar(50)  NOT NULL,
    [password] varchar(100)  NOT NULL,
    [user_type] varchar(50)  NULL,
    [is_active] bit  NOT NULL,
    [is_verified] bit  NOT NULL,
    [last_change_password] varchar(100)  NULL
);
GO

-- Creating table 'tb_exception'
CREATE TABLE [dbo].[tb_exception] (
    [id] int IDENTITY(1,1) NOT NULL,
    [exception_message] varchar(max)  NULL,
    [exception_stack_trace] varchar(max)  NULL,
    [exception_number] varchar(max)  NULL,
    [date] datetime  NULL
);
GO

-- Creating table 'tb_verification_code'
CREATE TABLE [dbo].[tb_verification_code] (
    [id] int  NOT NULL,
    [email] varchar(50)  NOT NULL,
    [verification_code] decimal(18,0)  NOT NULL,
    [status] varchar(50)  NOT NULL,
    [date] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id], [user_id] in table 'tb_employee_registration_data'
ALTER TABLE [dbo].[tb_employee_registration_data]
ADD CONSTRAINT [PK_tb_employee_registration_data]
    PRIMARY KEY CLUSTERED ([id], [user_id] ASC);
GO

-- Creating primary key on [id], [user_id] in table 'tb_employer_registration_data'
ALTER TABLE [dbo].[tb_employer_registration_data]
ADD CONSTRAINT [PK_tb_employer_registration_data]
    PRIMARY KEY CLUSTERED ([id], [user_id] ASC);
GO

-- Creating primary key on [id] in table 'tb_user'
ALTER TABLE [dbo].[tb_user]
ADD CONSTRAINT [PK_tb_user]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tb_exception'
ALTER TABLE [dbo].[tb_exception]
ADD CONSTRAINT [PK_tb_exception]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tb_verification_code'
ALTER TABLE [dbo].[tb_verification_code]
ADD CONSTRAINT [PK_tb_verification_code]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [user_id] in table 'tb_employee_registration_data'
ALTER TABLE [dbo].[tb_employee_registration_data]
ADD CONSTRAINT [FK_tb_employee_registration_data_tb_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[tb_user]
        ([id])
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Creating non-clustered index for FOREIGN KEY 'FK_tb_employee_registration_data_tb_user'
CREATE INDEX [IX_FK_tb_employee_registration_data_tb_user]
ON [dbo].[tb_employee_registration_data]
    ([user_id]);
GO

-- Creating foreign key on [user_id] in table 'tb_employer_registration_data'
ALTER TABLE [dbo].[tb_employer_registration_data]
ADD CONSTRAINT [FK_tb_employer_registration_data_tb_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[tb_user]
        ([id])
    ON DELETE CASCADE ON UPDATE CASCADE;

-- Creating non-clustered index for FOREIGN KEY 'FK_tb_employer_registration_data_tb_user'
CREATE INDEX [IX_FK_tb_employer_registration_data_tb_user]
ON [dbo].[tb_employer_registration_data]
    ([user_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------