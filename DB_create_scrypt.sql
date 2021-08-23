DROP DATABASE IF EXISTS EFCoreMySQL;
Create DATABASE EFCoreMySQL;

alter table EFCoreMySQL.Messages 
   drop foreign key FK_Messages_Sessions_Session_Id;


alter table EFCoreMySQL.Messages 
   drop foreign key FK_Messages_Sessions_Session_Id;

drop table if exists EFCoreMySQL.Messages;

drop table if exists EFCoreMySQL.Sessions;

/*==============================================================*/
/* User: EFCoreMySQL                                            */
/*==============================================================*/
create user EFCoreMySQL;

/*==============================================================*/
/* Table: Messages                                              */
/*==============================================================*/
create table EFCoreMySQL.Messages
(
   Id                   int(11) not null auto_increment  comment '',
   Session_Id           int(11) not null  comment '',
   MC1_timestamp        datetime(6) not null  comment '',
   MC2_timestamp        datetime(6) not null  comment '',
   MC3_timestamp        datetime(6) not null  comment '',
   End_timestamp        datetime(6) not null  comment '',
   primary key (Id)
);

/*==============================================================*/
/* Table: Sessions                                              */
/*==============================================================*/
create table EFCoreMySQL.Sessions
(
   Id                   int(11) not null auto_increment  comment '',
   primary key (Id)
);

alter table EFCoreMySQL.Messages add constraint FK_Messages_Sessions_Session_Id foreign key (Session_Id)
      references EFCoreMySQL.Sessions (Id);
