create table Account
(
	id int identity(1,1) primary key,
	name varchar(20) not null unique,
	password varchar(20) not null,
	score int not null,
)

