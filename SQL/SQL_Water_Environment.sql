CREATE DATABASE Water_Environment
GO
USE Water_Environment
GO
CREATE TABLE "Users"(
    "id" INT NOT NULL,
    "UserName" VARCHAR(255) NOT NULL,
    "PassWord" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "UserPermission" INT NOT NULL,
    "CreatedOn" DATETIME NOT NULL,
    "IsActive" BIT NOT NULL
);
ALTER TABLE
    "Users" ADD CONSTRAINT "users_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "users_username_unique" ON
    "Users"("UserName");
CREATE TABLE "ActivitiesAndNews"(
    "id" INT NOT NULL,
    "Title" NVARCHAR(255) NOT NULL,
    "IsActive" BIT NOT NULL,
    "CreateBy" INT NOT NULL,
    "CreateOn" DATETIME NOT NULL,
    "Content" NVARCHAR(255) NOT NULL,
    "CategoryId" INT NOT NULL
);
ALTER TABLE
    "ActivitiesAndNews" ADD CONSTRAINT "activitiesandnews_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "activitiesandnews_title_unique" ON
    "ActivitiesAndNews"("Title");
CREATE TABLE "Category"(
    "id" INT NOT NULL,
    "Name" NVARCHAR(255) NOT NULL,
    "IsActive" BIT NOT NULL,
    "CreatedOn" DATETIME NOT NULL,
    "CreateBy" INT NOT NULL
);
ALTER TABLE
    "Category" ADD CONSTRAINT "category_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "category_name_unique" ON
    "Category"("Name");
CREATE TABLE "Permission"(
    "id" INT NOT NULL,
    "Name" NVARCHAR(255) NOT NULL,
    "Code" VARCHAR(255) NOT NULL
);
ALTER TABLE
    "Permission" ADD CONSTRAINT "permission_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "permission_code_unique" ON
    "Permission"("Code");
CREATE TABLE "Comment"(
    "id" INT NOT NULL,
    "UserComment" INT NOT NULL,
    "CreateOn" DATETIME NOT NULL,
    "Content" NVARCHAR(255) NOT NULL,
    "PostId" INT NOT NULL
);
ALTER TABLE
    "Comment" ADD CONSTRAINT "comment_id_primary" PRIMARY KEY("id");
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