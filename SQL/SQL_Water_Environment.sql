CREATE DATABASE Water_Environment
GO
USE Water_Environment
GO
CREATE TABLE "Users"(
    "id" INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    "UserName" VARCHAR(255) NOT NULL,
    "PassWord" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "UserPermission" INT NOT NULL,
    "CreatedOn" DATETIME NOT NULL,
    "IsActive" BIT NOT NULL
);
CREATE UNIQUE INDEX "users_username_unique" ON
    "Users"("UserName");
CREATE TABLE "ActivitiesAndNews"(
    "id" INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    "Title" NVARCHAR(255) NOT NULL,
    "IsActive" BIT NOT NULL,
    "CreateBy" INT NOT NULL,
    "CreateOn" DATETIME NOT NULL,
    "Content" NVARCHAR(255) NOT NULL,
    "CategoryId" INT NOT NULL
);
CREATE UNIQUE INDEX "activitiesandnews_title_unique" ON
    "ActivitiesAndNews"("Title");
CREATE TABLE "Category"(
    "id" INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    "Name" NVARCHAR(255) NOT NULL,
    "IsActive" BIT NOT NULL,
    "CreatedOn" DATETIME NOT NULL,
    "CreateBy" INT NOT NULL
);
CREATE UNIQUE INDEX "category_name_unique" ON
    "Category"("Name");
CREATE TABLE "Permission"(
    "id" INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    "Name" NVARCHAR(255) NOT NULL,
    "Code" VARCHAR(255) NOT NULL
);
CREATE UNIQUE INDEX "permission_code_unique" ON
    "Permission"("Code");
CREATE TABLE "Comment"(
    "id" INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    "UserComment" INT NOT NULL,
    "CreateOn" DATETIME NOT NULL,
    "Content" NVARCHAR(255) NOT NULL,
    "PostId" INT NOT NULL
);
ALTER TABLE
    "Users" ADD CONSTRAINT "users_userpermission_foreign" FOREIGN KEY("UserPermission") REFERENCES "Permission"("id");
ALTER TABLE
    "ActivitiesAndNews" ADD CONSTRAINT "activitiesandnews_createby_foreign" FOREIGN KEY("CreateBy") REFERENCES "Users"("id");
ALTER TABLE
    "ActivitiesAndNews" ADD CONSTRAINT "activitiesandnews_categoryid_foreign" FOREIGN KEY("CategoryId") REFERENCES "Category"("id");
ALTER TABLE
    "Category" ADD CONSTRAINT "category_createby_foreign" FOREIGN KEY("CreateBy") REFERENCES "Users"("id");
ALTER TABLE
    "Comment" ADD CONSTRAINT "comment_usercomment_foreign" FOREIGN KEY("UserComment") REFERENCES "Users"("id");
ALTER TABLE
    "Comment" ADD CONSTRAINT "comment_postid_foreign" FOREIGN KEY("PostId") REFERENCES "ActivitiesAndNews"("id");