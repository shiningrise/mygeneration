CREATE TABLE [DataTypeTest] (
	[Field_IntIdentity] [int] IDENTITY (1, 1) NOT NULL ,
	[Field_VarChar] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Field_NVarChar] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Field_Char] [char] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Field_NChar] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Field_Text] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Field_NText] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Field_BigInt] [bigint] NULL ,
	[Field_DateTime] [datetime] NULL ,
	[Field_Decimal] [decimal](18, 0) NULL ,
	[Field_Float] [float] NULL ,
	[Field_Image] [image] NULL ,
	[Field_Int] [int] NULL ,
	[Field_Money] [money] NULL ,
	[Field_Numeric] [numeric](18, 0) NULL ,
	[Field_Real] [real] NULL ,
	[Field_SmallDateTime] [smalldatetime] NULL ,
	[Field_SmallInt] [smallint] NULL ,
	[Field_SmallMoney] [smallmoney] NULL ,
	[Field_SqlVariant] [sql_variant] NULL ,
	[Field_TimeStamp] [timestamp] NULL ,
	[Field_TinyInt] [tinyint] NULL ,
	[Field_UniqueIdentifier] [uniqueidentifier] NULL ,
	[Field_VarBinary] [varbinary] (50) NULL ,
	[Field_Binary] [binary] (50) NULL ,
	[Field_Bit] [bit] NULL ,
	CONSTRAINT [PK_DataTypeTest] PRIMARY KEY  CLUSTERED 
	(
		[Field_IntIdentity]
	)  ON [PRIMARY] 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


