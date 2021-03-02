
drop table [User];
create table [User](
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	Account	varchar(30) NOT NULL,
	Password	varchar(50) NOT NULL,
	Name	nvarchar(30) NOT NULL,
	Department	nvarchar(50),
	TelHome	varchar(30),
	Phone	varchar(50),
	PostCode varchar(5),
	City nvarchar(10),
	Town nvarchar(10),
	Address	nvarchar(100),
	Email	varchar(50),
	IsForgotPassword	bit,
	CompanyId	uniqueidentifier,
	CONSTRAINT PK_User PRIMARY KEY CLUSTERED (Id)
);

drop table UserToken;
create table [UserToken](
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
    UserId uniqueidentifier NOT NULL ,
	AccessToken	varchar(200) NOT NULL,
	RefreshToken	uniqueidentifier NOT NULL,
    ExpireAt	datetime2(7),
    IPAddress	varchar(30),
	CONSTRAINT FK_UserToken FOREIGN KEY (UserId) references [User] (Id),
	CONSTRAINT PK_UserToken PRIMARY KEY CLUSTERED (Id)
);


drop table Role;
create table Role(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	Name	nvarchar(30) NOT NULL,
	Code	varchar(20) NOT NULL,
	CONSTRAINT PK_Role PRIMARY KEY CLUSTERED (Id)
);

drop table UserRole;
create table UserRole(
	Id	uniqueidentifier NOT NULL default NEWID(),
	RoleId	uniqueidentifier NOT NULL,
	UserId	uniqueidentifier NOT NULL,
	CONSTRAINT FK_UserRole1 FOREIGN KEY (RoleId) references Role (Id),
	CONSTRAINT FK_UserRole2 FOREIGN KEY (UserId) references [User] (Id),
	CONSTRAINT PK_UserRole PRIMARY KEY CLUSTERED (Id)
);


CREATE TABLE [dbo].[repairSettingGroup](
	id uniqueidentifier NOT NULL default NEWID(),
	description nvarchar(20),
	CONSTRAINT pk_repairSettingGroup PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [dbo].[repairSetting](
	id uniqueidentifier NOT NULL default NEWID(),
	createdAt datetime2(7) NOT NULL default GetDate(),
	createdUser	uniqueidentifier,
	updatedAt datetime2(7) NOT NULL default GetDate(),
	updatedUser	uniqueidentifier,
	repairSettingGroupId uniqueidentifier,
	code varchar(5),
	description nvarchar(30),
	content nvarchar(50),
	periodType varchar(2),
	periodUnit numeric(10),
	CONSTRAINT pk_repairSetting PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT fk_repairSetting FOREIGN KEY (repairSettingGroupId) references repairSettingGroup (id)
);


CREATE TABLE [dbo].[repairSettingDetail](
	id uniqueidentifier NOT NULL default NEWID(),
	createdAt datetime2(7) NOT NULL default GetDate(),
	createdUser	uniqueidentifier,
	updatedAt datetime2(7) NOT NULL default GetDate(),
	updatedUser	uniqueidentifier,
	repairSettingId uniqueidentifier,
	changeType varchar(2),
	changeUnit numeric(10),
	CONSTRAINT pk_repairSettingsDetail PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT fk_repairSettingsDetail FOREIGN KEY (repairSettingId) references repairSetting (id)
);



drop table Client;
create table Client(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	Name	nvarchar(30) NOT NULL,
	IDNumber	varchar(10),
	PassportNo	varchar(10),
	EngName	varchar(30),
	ClientType	varchar(1) NOT NULL,
	CompanyId	uniqueidentifier NOT NULL,
	TaxIDNumber	varchar(10),
	Gender	varchar(1),
	Nation	nvarchar(20),
	TelHome	varchar(30),
	TelOffice	varchar(30),
	Phone	varchar(50),
	PostCode varchar(5),
	City nvarchar(10),
	Town nvarchar(10),
	Addr	nvarchar(100),
	DeliveryAddr	nvarchar(100),
	DeliveryPostCode varchar(5),
	DeliveryCity nvarchar(10),
	DeliveryTown nvarchar(10),
	Memo	nvarchar(200),
	CONSTRAINT FK_Client FOREIGN KEY (CompanyId) references Company (Id),
	CONSTRAINT PK_Client PRIMARY KEY CLUSTERED (Id)
);


create table CityTown(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	PostCode varchar(5),
	City nvarchar(10),
	Town nvarchar(10),
	CONSTRAINT PK_CityTown PRIMARY KEY CLUSTERED (Id)
);

drop table Quotation;
create table Quotation(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	QuotationNo	varchar(20),
	ClientId	uniqueidentifier,
	PostCode varchar(5),
	City nvarchar(10),
	Town nvarchar(10),
	Addr	nvarchar(100),
	Ping	numeric(7, 3),
	Notice	nvarchar(200),
	Surveyor	nvarchar(30),
	SurveyAt	datetime2(7),
	RobotInfoId	uniqueidentifier,
	Amount	numeric(3),
	TrialPrice	numeric(8),
	FinalPrice	numeric(8),
	DealPrice numeric(8),
	Version	numeric(3) not null,
	IsFinal	bit,
	QuotationType	varchar(2) NOT NULL,
	WarrantyEffDate	datetime2(7),
	WarrantyExpDate	datetime2(7),
	RentEffDate	datetime2(7),
	RentExpDate	datetime2(7),
	ServiceRsvDate datetime2(7),
	QuotationState varchar(3) default 'DRF',
	CONSTRAINT FK_Quotation1 FOREIGN KEY (RobotInfoId) references RobotInfo (Id),
	CONSTRAINT FK_Quotation2 FOREIGN KEY (ClientId) references Client (Id),
	CONSTRAINT PK_Quotation PRIMARY KEY CLUSTERED (Id)
);

drop table QuotationSurvey;
create table QuotationSurvey(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	QuotationId	uniqueidentifier NOT NULL,
	WorkAreaMap	nvarchar(100),
	Track	nvarchar(100),
	NavigationPoints	numeric(5),
	SurveyAt	datetime2(7),
	CONSTRAINT FK_QuotationSurvey FOREIGN KEY (QuotationId) references Quotation (Id),
	CONSTRAINT PK_QuotationSurvey PRIMARY KEY CLUSTERED (Id)
);

drop table QuotationSurveyPhoto;
create table QuotationSurveyPhoto(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	QuotationId	uniqueidentifier NOT NULL,
	Place	nvarchar(50),
	FileName	nvarchar(100),
	CONSTRAINT FK_QuotationSurveyPhoto FOREIGN KEY (QuotationId) references Quotation (Id),
	CONSTRAINT PK_QuotationSurveyPhoto PRIMARY KEY CLUSTERED (Id)
);

drop table Operator;
create table Operator(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	UserId	uniqueidentifier,
	OperationAdmit	numeric(1, 0) NOT NULL,
	OperationHour numeric(10, 2) default 0,
	CONSTRAINT FK_Operator FOREIGN KEY (UserId) references [User] (Id),
	CONSTRAINT PK_Operator PRIMARY KEY CLUSTERED (Id)
);

drop table OperatorAdmit;
create table OperatorAdmit(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	AdmitType	numeric(1, 0),
	Title 		nvarchar(50),
	AdmitAt	datetime2(7),
	ExpiryDate	datetime2(7),
	OperatorId	uniqueidentifier NOT NULL,
	CONSTRAINT FK_OperatorAdmit FOREIGN KEY (OperatorId) references [Operator] (Id),
	CONSTRAINT PK_OperatorAdmit PRIMARY KEY CLUSTERED (Id)
);

drop table RobotInfo;
create table RobotInfo(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	RegSN	varchar(30),
	ProperNo	varchar(30),
	RobotCatalogId	uniqueidentifier NOT NULL,
	UseState	numeric(1, 0) NOT NULL,
	WorkingHours	numeric(10, 0),
	KickOverAt	datetime2(7),
	Custodian	nvarchar(30),
	ClientId	uniqueidentifier,
	CONSTRAINT FK_RobotInfo FOREIGN KEY (RobotCatalogId) references RobotCatalog (Id),
	CONSTRAINT FK_RobotInfo2 FOREIGN KEY (ClientId) references Client (Id),
	CONSTRAINT PK_RobotInfo PRIMARY KEY CLUSTERED (Id)

);

drop table RobotCatalog;
create table RobotCatalog(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	M01Brand	nvarchar(50),
	M01SerialNumber	varchar(30) NOT NULL,
	M01NameC	nvarchar(50),
	M01NameE	varchar(50),
	M01NickName	nvarchar(20),
	LampHolderBrand	nvarchar(50),
	LampHolderSerialNumber	varchar(30) NOT NULL,
	LampHolderNameC	nvarchar(50),
	LampHolderNameE	varchar(50),
	LampHolderNickName	nvarchar(20),
	SalesStatus	numeric(1, 0) default 1,
	ProLink		nvarchar(100),
	LaunchAt	datetime2(7),
	RobotInfoId uniqueidentifier not null,
	CONSTRAINT PK_RobotCatalog PRIMARY KEY CLUSTERED (Id)
);

drop table RobotRepair;
create table RobotRepair(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	ReType	numeric(1, 0) NOT NULL,
	ReAt	datetime2(7) NOT NULL,
	ReDescription	nvarchar(200),
	ReNo	varchar(20),
	ReHours	numeric(10, 0) NOT NULL,
	ReState	numeric(1, 0) default 1 NOT NULL,
	TechMan	nvarchar(20) NOT NULL,
	RobotInfoId uniqueidentifier not null,
	CONSTRAINT FK_RobotRepair FOREIGN KEY (RobotInfoId) references RobotInfo (Id),
	CONSTRAINT PK_RobotRepair PRIMARY KEY CLUSTERED (Id)
);

drop table RobotUnitInfo;
create table RobotUnitInfo(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	Name	nvarchar(50) NOT NULL,
	Spec	nvarchar(50),
	Num	numeric(10, 0),
	ProductAt	datetime2(7) NOT NULL,
	WorkingHours	numeric(10, 0) NOT NULL,
	MaintType	numeric(1, 0) NOT NULL,
	MaintBy	numeric(20, 2) NOT NULL,
	MaintPeriod	numeric(1) NOT NULL,
	Memo	nvarchar(200),
	RobotInfoId uniqueidentifier not null,
	CONSTRAINT FK_RobotUnitInfo FOREIGN KEY (RobotInfoId) references RobotInfo (Id),
	CONSTRAINT PK_RobotUnitInfo PRIMARY KEY CLUSTERED (Id)
);

drop table JobPlan;
create table JobPlan(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	QuotationId	uniqueidentifier NOT NULL,
	Notice	nvarchar(200),
	PredictJobAt	datetime2(7),
	PredictJobEnd	datetime2(7),
	SimulationDiagram	nvarchar(50),
	IsFinal   bit NOT NULL,
	CONSTRAINT FK_JobPlan FOREIGN KEY (QuotationId) references Quotation (Id),
	CONSTRAINT PK_JobPlan PRIMARY KEY CLUSTERED (Id)
);

drop table JobPlanOperator;
create table JobPlanOperator(
	Id	uniqueidentifier NOT NULL default NEWID(),
	JobPlanId	uniqueidentifier NOT NULL,
	OperatorId	uniqueidentifier NOT NULL,
	CONSTRAINT FK_JobPlanOperator1 FOREIGN KEY (JobPlanId) references JobPlan (Id),
	CONSTRAINT FK_JobPlanOperator2 FOREIGN KEY (OperatorId) references Operator (Id),
	CONSTRAINT PK_JobPlanOperator PRIMARY KEY CLUSTERED (Id)
);

drop table JobPlanRobot;
create table JobPlanRobot(
	Id	uniqueidentifier NOT NULL default NEWID(),
	JobPlanId	uniqueidentifier NOT NULL,
	RobotInfoId	uniqueidentifier NOT NULL,
	CONSTRAINT FK_JobPlanRobot1 FOREIGN KEY (JobPlanId) references JobPlan (Id),
	CONSTRAINT FK_JobPlanRobot2 FOREIGN KEY (RobotInfoId) references RobotInfo (Id),
	CONSTRAINT PK_JobPlanRobot PRIMARY KEY CLUSTERED (Id)
);

drop table JobInfo;
create table JobInfo(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	JobPlanId	uniqueidentifier NOT NULL,
	PostCode varchar(5),
	City nvarchar(10),
	Town nvarchar(10),
	Addr	nvarchar(100),
	Ping	numeric(7, 3),
	Place	nvarchar(50),
	Notice	nvarchar(200),
	JobAt	datetime2(7),
	JobEnd	datetime2(7),
	ElapsedTime	numeric(5),
	NavigationPoints	numeric(5),
	Price	numeric(8),
	Track	nvarchar(100),
	SimulationDiagram	nvarchar(100),
	IsFinal   bit NOT NULL,
	JobReport	nvarchar(100),
	CONSTRAINT FK_JobInfo FOREIGN KEY (JobPlanId) references JobPlan (Id),
	CONSTRAINT PK_JobInfo PRIMARY KEY CLUSTERED (Id)
);

drop table JobInfoOperator;
create table JobInfoOperator(
	Id	uniqueidentifier NOT NULL default NEWID(),
	JobInfoId	uniqueidentifier NOT NULL,
	OperatorId	uniqueidentifier NOT NULL,
	CONSTRAINT FK_JobInfoOperator1 FOREIGN KEY (JobInfoId) references JobInfo (Id),
	CONSTRAINT FK_JobInfoOperator2 FOREIGN KEY (OperatorId) references Operator (Id),
	CONSTRAINT PK_JobInfoOperator PRIMARY KEY CLUSTERED (Id)
);


drop table JobInfoRobot;
create table JobInfoRobot(
	Id	uniqueidentifier NOT NULL default NEWID(),
	JobInfoId	uniqueidentifier NOT NULL,
	RobotInfoId	uniqueidentifier NOT NULL,
	CONSTRAINT FK_JobInfoRobot1 FOREIGN KEY (JobInfoId) references JobInfo (Id),
	CONSTRAINT FK_JobInfoRobot2 FOREIGN KEY (RobotInfoId) references RobotInfo (Id),
	CONSTRAINT PK_JobInfoRobot PRIMARY KEY CLUSTERED (Id)
);


drop table WorkAreaMap;
create table WorkAreaMap(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	JobInfoId	uniqueidentifier NOT NULL,
	FileName	nvarchar(50) NOT NULL,
	BuildAt	datetime2(7) NOT NULL,
	Notice	nvarchar(200),
	CONSTRAINT FK_WorkAreaMap FOREIGN KEY (JobInfoId) references JobInfo (Id),
	CONSTRAINT PK_WorkAreaMap PRIMARY KEY CLUSTERED (Id)
);

drop table RoutePlan;
create table RoutePlan(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	JobInfoId	uniqueidentifier NOT NULL,
	FileName	nvarchar(50) NOT NULL,
	BuildAt	datetime2(7) NOT NULL,
	Notice	nvarchar(200),
	CONSTRAINT FK_RoutePlan FOREIGN KEY (JobInfoId) references JobInfo (Id),
	CONSTRAINT PK_RoutePlan PRIMARY KEY CLUSTERED (Id)
);

drop table JobLog;
create table JobLog(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	JobInfoId	uniqueidentifier NOT NULL,
	Instruction	varchar(100) NOT NULL,
	ExecAt	datetime2(7) NOT NULL,
	Memo	nvarchar(200),
	CONSTRAINT FK_JobLog FOREIGN KEY (JobInfoId) references JobInfo (Id),
	CONSTRAINT PK_JobLog PRIMARY KEY CLUSTERED (Id)
);


CREATE SEQUENCE QuotationSequence AS INT
START WITH 1
INCREMENT BY 1;


CREATE SEQUENCE RobotRepairSequence AS INT
START WITH 1
INCREMENT BY 1;



CREATE SEQUENCE ServiceSearchSequence AS INT
START WITH 1
INCREMENT BY 1;


CREATE SEQUENCE ServiceRepairSequence AS INT
START WITH 1
INCREMENT BY 1;


CREATE SEQUENCE ServiceProvidingSequence AS INT
START WITH 1
INCREMENT BY 1;


drop table ServiceSearch;
create table ServiceSearch(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	ServiceNo	varchar(20),
	[Case]	nvarchar(30),
	ClientId	uniqueidentifier,
	ClientName  nvarchar(30),
	CompanyId	uniqueidentifier,
	CompanyName nvarchar(30),
	Phone	varchar(50) NOT NULL,
	Email	varchar(50) NOT NULL,
	ServiceType	varchar(2),
	ServicePerson	uniqueidentifier,
	ServiceInfo	nvarchar(100),
	SearchDetescription	nvarchar(200),
	ServiceDate	datetime2(7),
	ServiceStatus	numeric(1,0),
	CloseReasonDate	datetime2(7),
	Satisfaction	 numeric(10),
	Attachment	 nvarchar(100),
	CONSTRAINT PK_ServiceSearch PRIMARY KEY CLUSTERED (Id)
);





drop table ServiceRepair;
create table ServiceRepair(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	ServiceNo	varchar(20),
	RepairCase	nvarchar(20),
	ClientId	uniqueidentifier,
	ClientName  nvarchar(30),
	CompanyId	uniqueidentifier,
	CompanyName nvarchar(30),
	Phone	varchar(50),
	Email	varchar(50),
	RepairType	numeric(1,0),
	ServicePerson	uniqueidentifier,
	RepairInfo	nvarchar(100),
	RepairDescription	nvarchar(200),
	ServiceReNo	varchar(20),
	ServiceDate	datetime2(7),
	ServiceStatus	 numeric(1,0),
	CloseReasonDate	datetime2(7),
	Satisfaction	 numeric(10),
	Attachment	 nvarchar(100),
	CONSTRAINT PK_ServiceRepair PRIMARY KEY CLUSTERED (Id)
);



drop table ServiceProviding;
create table ServiceProviding(
	SeqNo	int IDENTITY(1,1) NOT NULL,
	Id	uniqueidentifier NOT NULL default NEWID(),
	Status	numeric(1, 0) default 1,
	CreatedAt	datetime2(7) NOT NULL default GetDate(),
	CreatedUser	uniqueidentifier,
	UpdatedAt	datetime2(7) NOT NULL default GetDate(),
	UpdatedUser	uniqueidentifier,
	ServiceNo	varchar(20),
	[Case]	 nvarchar(30),
	ProvidingType	numeric(1, 0),
	ClientId	uniqueidentifier,
	ClientName  nvarchar(30),
	CompanyId	uniqueidentifier,
	CompanyName nvarchar(30),
	Phone	varchar(50),
	Email	varchar(50),
	ServicePersion	uniqueidentifier,
	ServiceInfo	nvarchar(100),
	ProvidingDescription	nvarchar(200),
	ServiceDate	datetime2(7),
	ServiceStatus	numeric(1,0),
	CloseReasonDate	datetime2(7),
	Satisfaction	 numeric(10),
	Attachment	 varchar(50),
	CONSTRAINT PK_ServiceProviding PRIMARY KEY CLUSTERED (Id)
);




