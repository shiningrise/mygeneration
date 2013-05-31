;-----------------------------------------
; dOOdads Installation Script
;-----------------------------------------

; Set the compressions to lzma, which is always the best compression!
SetCompressor lzma 

; The name of the installer
Name "dOOdads"

; The file to write
OutFile "dOOdads_installer.exe"

; Icon doesn't work for some reason
Icon ".\modern-install.ico"

XPStyle on

ShowInstDetails show

;LicenseText "Liscence Agreement"
;LicenseData "BSDLicense.rtf"

; The default installation directory
InstallDir $PROGRAMFILES\MyGeneration

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM SOFTWARE\MyGeneration "Install_Dir"

; The text to prompt the user to enter a directory
ComponentText "This will install MyGeneration's dOOdads .NET Architecture on your computer."

; The text to prompt the user to enter a directory
DirText "Point to your MyGeneration Installation."

  ; The stuff to install
  Section "Install dOOdads"

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ;Create Settings Directory 
  ;ExecShell mkdir $INSTDIR\Settings

  File /oname=dOOdads.chm ..\doodads\dOOdads.chm

  ; Create Folders
  CreateDirectory "$INSTDIR\Architectures"
  
  ; Create Architecture Sub-Folders
  CreateDirectory "$INSTDIR\Architectures\dOOdads"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\dOOdad_Demo"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\dOOdad_Demo\Generated"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\MyGeneration.dOOdads"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\"  
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\dOOdad_Demo"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\dOOdad_Demo\Generated"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\MyGeneration.dOOdads"
  CreateDirectory "$INSTDIR\Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\"

  ; dOOdads
  ; VB.NET Demo
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\README.TXT ..\doodads\VB.NET\dOOdad_Demo\README.TXT
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\About.resx ..\doodads\VB.NET\dOOdad_Demo\About.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\About.vb ..\doodads\VB.NET\dOOdad_Demo\About.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\App.config ..\doodads\VB.NET\dOOdad_Demo\App.config
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\AssemblyInfo.vb ..\doodads\VB.NET\dOOdad_Demo\AssemblyInfo.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo.sln ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo.sln
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.sln ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.sln  
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo.vbproj ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo.vbproj
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.vbproj ..\doodads\VB.NET\dOOdad_Demo\dOOdad_Demo_2005.vbproj  
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Employees.vb ..\doodads\VB.NET\dOOdad_Demo\Employees.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\EmployeesEdit.resx ..\doodads\VB.NET\dOOdad_Demo\EmployeesEdit.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\EmployeesEdit.vb ..\doodads\VB.NET\dOOdad_Demo\EmployeesEdit.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\FillComboBox.resx ..\doodads\VB.NET\dOOdad_Demo\FillComboBox.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\FillComboBox.vb ..\doodads\VB.NET\dOOdad_Demo\FillComboBox.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Form1.resx ..\doodads\VB.NET\dOOdad_Demo\Form1.resx
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Form1.vb ..\doodads\VB.NET\dOOdad_Demo\Form1.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Invoices.vb ..\doodads\VB.NET\dOOdad_Demo\Invoices.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Products.vb ..\doodads\VB.NET\dOOdad_Demo\Products.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\TheMasterSample.vb ..\doodads\VB.NET\dOOdad_Demo\TheMasterSample.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Generated\_Employees.vb ..\doodads\VB.NET\dOOdad_Demo\Generated\_Employees.vb
  File /oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\Generated\_Products.vb ..\doodads\VB.NET\dOOdad_Demo\Generated\_Products.vb
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Application.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Application.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Application.myapp" "..\doodads\VB.NET\dOOdad_Demo\My Project\Application.myapp"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Application1.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Application1.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Resources.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Resources.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Resources.resx" "..\doodads\VB.NET\dOOdad_Demo\My Project\Resources.resx"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Settings.Designer.vb" "..\doodads\VB.NET\dOOdad_Demo\My Project\Settings.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\dOOdad_Demo\My Project\Settings.settings" "..\doodads\VB.NET\dOOdad_Demo\My Project\Settings.settings"

  ; VB.NET dOOdads
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\README.TXT ..\doodads\VB.NET\MyGeneration.dOOdads\README.TXT
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\AssemblyInfo.vb ..\doodads\VB.NET\MyGeneration.dOOdads\AssemblyInfo.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\BusinessEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\BusinessEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.vbproj ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.vbproj
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.vbproj ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.vbproj  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\TransactionMgr.vb ..\doodads\VB.NET\MyGeneration.dOOdads\TransactionMgr.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\WhereParameter.vb ..\doodads\VB.NET\MyGeneration.dOOdads\WhereParameter.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\AggregateParameter.vb ..\doodads\VB.NET\MyGeneration.dOOdads\AggregateParameter.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.sln ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.sln
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx ..\doodads\VB.NET\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx  
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4Entity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\MySql4Entity.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.vb
  File /oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.vb ..\doodads\VB.NET\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.vb
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Application.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Application.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Application.myapp" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Application.myapp"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Application1.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Application1.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Resources.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Resources.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Resources.resx" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Resources.resx"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Settings.Designer.vb" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Settings.Designer.vb"
  File "/oname=Architectures\dOOdads\VB.NET\MyGeneration.dOOdads\My Project\Settings.settings" "..\doodads\VB.NET\MyGeneration.dOOdads\My Project\Settings.settings"
									
  ; CSharp Demo
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\README.TXT ..\doodads\CSharp\dOOdad_Demo\README.TXT
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\App.ico ..\doodads\CSharp\dOOdad_Demo\App.ico
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\About.resx ..\doodads\CSharp\dOOdad_Demo\About.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\About.cs ..\doodads\CSharp\dOOdad_Demo\About.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\App.config ..\doodads\CSharp\dOOdad_Demo\App.config
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\AssemblyInfo.cs ..\doodads\CSharp\dOOdad_Demo\AssemblyInfo.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo.sln ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo.sln
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.sln ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.sln  
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo.csproj ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo.csproj
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.csproj ..\doodads\CSharp\dOOdad_Demo\dOOdad_Demo_2005.csproj  
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Employees.cs ..\doodads\CSharp\dOOdad_Demo\Employees.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\EmployeesEdit.resx ..\doodads\CSharp\dOOdad_Demo\EmployeesEdit.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\EmployeesEdit.cs ..\doodads\CSharp\dOOdad_Demo\EmployeesEdit.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\FillComboBox.resx ..\doodads\CSharp\dOOdad_Demo\FillComboBox.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\FillComboBox.cs ..\doodads\CSharp\dOOdad_Demo\FillComboBox.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Form1.resx ..\doodads\CSharp\dOOdad_Demo\Form1.resx
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Form1.cs ..\doodads\CSharp\dOOdad_Demo\Form1.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Invoices.cs ..\doodads\CSharp\dOOdad_Demo\Invoices.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Products.cs ..\doodads\CSharp\dOOdad_Demo\Products.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\TheMasterSample.cs ..\doodads\CSharp\dOOdad_Demo\TheMasterSample.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Generated\_Employees.cs ..\doodads\CSharp\dOOdad_Demo\Generated\_Employees.cs
  File /oname=Architectures\dOOdads\CSharp\dOOdad_Demo\Generated\_Products.cs ..\doodads\CSharp\dOOdad_Demo\Generated\_Products.cs

  ; CSharp dOOdads
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\README.TXT ..\doodads\CSharp\MyGeneration.dOOdads\README.TXT
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\AssemblyInfo.cs ..\doodads\CSharp\MyGeneration.dOOdads\AssemblyInfo.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\BusinessEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\BusinessEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.csproj ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.csproj
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.csproj ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.csproj  
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.prjx   
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\TransactionMgr.cs ..\doodads\CSharp\MyGeneration.dOOdads\TransactionMgr.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\WhereParameter.cs ..\doodads\CSharp\MyGeneration.dOOdads\WhereParameter.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\AggregateParameter.cs ..\doodads\CSharp\MyGeneration.dOOdads\AggregateParameter.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.sln ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.sln
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads_2005.sln  
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx ..\doodads\CSharp\MyGeneration.dOOdads\MyGeneration.dOOdads.cmbx   
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SqlClientDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OleDbDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\OracleClientDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\FirebirdSqlDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\VistaDBEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\PostgreSqlEntity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4DynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4Entity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\MySql4Entity.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteDynamicQuery.cs
  File /oname=Architectures\dOOdads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.cs ..\doodads\CSharp\MyGeneration.dOOdads\DbAdapters\SQLiteEntity.cs

SectionEnd ; end the section
; eof