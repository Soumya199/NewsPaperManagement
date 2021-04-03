create table Customer
(
CustomerName varchar(100) not null,
CustomerEmailId varchar(100) primary key not null ,
CustomerPhone varchar(100) not null,
CustomerAddress varchar(250) not null,
CustomerPssword varchar(100) not null
);

create table Admin
(
AdminName varchar(100) not null,
AdminEmailId Varchar(100) primary key not null,
AdminPhone Varchar(100) not null,
AdminPassword varchar(100) not null,
);

create table Advertisement
(
AdvertisementNo int identity(1000,1) primary key not null,
AdvertisementName varchar(100) not null,
CustomerEmailId varchar(100) foreign key references Customer(CustomerEmailId) not null,
AdvertisementDescription varchar(100) not null,
AdvertisementImage varchar(max) not null,
AdvertisementDimension varchar(100) not null,
AdvertisementPrice decimal not null,
AdvertisementDate date not null,
AdvertisementStatus varchar(100) default 'pending'
);

create table ReportProblem
(
ReportId int identity(1000,1) primary key not null,
AdvertisementNo int foreign key references Advertisement(AdvertisementNo) not null,
CustomerEmailId varchar(100) foreign key references Customer(CustomerEmailId) not null,
ReportType varchar(100) not null,
ReportDescription varchar(100) not null
);
delete from Advertisement
where AdvertisementNo=1003

insert into Customer
values('soumya','sardar','1111','zcsf','1234')
insert into Admin
values('Admin','Admin@gmail.com','7008118056','Password')
select*from Advertisement