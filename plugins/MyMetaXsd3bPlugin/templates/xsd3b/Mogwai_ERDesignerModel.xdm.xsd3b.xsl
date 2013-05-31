<!--
    Document   : templates\xsd3b\Mogwai_ERDesignerModel.xdm.xsd3b.xsl
    Created on : 2007-04-29
    Author     : dl3bak
 
   <a href="http://mogwai.sourceforge.net/erdesigner/erdesigner.html">Mogwai ERDesigner</a>
       is a tool for vizualizing the Database structure which uses an
       XML-based file format.

	Xsd3b is a relational mapping tool
	
	This template translates *.xdm-files to xsd3b-files
	
	Requirements
		Dl3bak.Xsd3b.dll
		Dl3bak.Xsd3b.xsd.dll
		Dl3bak.Xsd3b.xslt.dll
		templates\xsd3b\Mogwai_ERDesignerModel.xdm.xsd3b.xsl
		
		a host for Dl3bak.Xsd3b
			either xsd3bGui.exe
			or xsd3bcmd.exe
			or MyMeta.Plugins.Xsd3b.dll with MyGeneration
			
-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output indent="yes" omit-xml-declaration="yes" />

	<xsl:template match="/" >
		 <xsl:call-template name="SchemaXsd3b" />
	</xsl:template >

	<xsl:template name="SchemaXsd3b" >
	    <SchemaXsd3b>
	    
		  <!-- exactly one  SchemaDefinition required -->
		  <SchemaDefinition> 
			<xsl:attribute name="SchemaID">ERDesignerModelSchemaNameGoesHere</xsl:attribute> 
			<!--
			<xsl:attribute name="XSD3bVersion">2000106</xsl:attribute> 
			<xsl:attribute name="XmlNamespace">XML-NamespaceGoesHere</xsl:attribute> 
			<xsl:attribute name="LocaleCulture">en-US</xsl:attribute> 
			<xsl:attribute name="DatasetNamespace">Dataset-NamespaceGoesHere</xsl:attribute> 
			<xsl:attribute name="UseNamedSimpleTypes">true</xsl:attribute> 
			<xsl:attribute name="UseNamedComplexTypes">true</xsl:attribute> 
			<xsl:attribute name="ExampleDataSource">ExampleDataGoesHere</xsl:attribute> 
			<xsl:attribute name="UseNestedSimpleTypes">false</xsl:attribute> 
			<xsl:attribute name="SchemaAlias">SchemaAliasGoesHere</xsl:attribute> 
			<xsl:attribute name="ExportDotNetCF">true</xsl:attribute> 
			<xsl:attribute name="ExportExcludeEvents">true</xsl:attribute>
			-->
		    <SchemaComment>SchemaCommentGoesHere
created from Mogwai ERDesigner *.xdm file
using Mogwai_ERDesignerModel.xdm.xsd3b.xsl

see http://mogwai.sourceforge.net/erdesigner/erdesigner.html
		    </SchemaComment>
		  </SchemaDefinition>
		  
		  <!-- not required DataTypeDefinitions. 
			<DataTypeDefinition DataTypeName="DataType" ParentDataTypeName="DataType" /> 
				is the root where all types are inherited from 
				
			required attributes:
				DataTypeName and ParentDataTypeName
			Example
				<DataTypeDefinition DataTypeName="System.String" ParentDataTypeName="DataType" /> 
			
		  -->

			<DataTypeDefinition DataTypeName="DataType" ParentDataTypeName="DataType" />
			<xsl:call-template name="DataTypeDefinition-System" />
		 <xsl:call-template name="DataTypeDefinition-sql" />
		 <xsl:call-template name="DataTypeDefinition-app" />
		 
		<xsl:for-each select="//Entities/Entity">
		  
		  <!-- required attributes: TableName -->
		  <TableDefinition>
			<xsl:attribute name="TableName"><xsl:value-of select="@name"/></xsl:attribute> 
			<xsl:attribute name="TableComment"><xsl:value-of select="@comment"/></xsl:attribute> 
			<!-- 
			<xsl:attribute name="AllowAnyAttribute">true</xsl:attribute> 
			<xsl:attribute name="ExportDataRowAlias">TableRenameRowAliasGoesHere</xsl:attribute> 
			<xsl:attribute name="ExportDataTableAlias">TableRenameTableGoesHere</xsl:attribute> 
			<xsl:attribute name="TableAlias">TableAliasGoesHere</xsl:attribute> 
			<xsl:attribute name="DBTableType">TableSteroetypeGoesHere</xsl:attribute>
			-->
			
			<xsl:for-each select="Attribute">
			<!-- required attributes: FieldName,  DataType
				ForeignKeys 	DataType(reference to //DataTypeDefinition/@DataTypeName) 
				PrimaryKeyNumber if exist and not "-1" this field is part of the primaryKey
				XmlFieldType	Attribute, Element, Hidden
				-->
				
			<FieldDefinition>
				<xsl:attribute name="FieldName"><xsl:value-of select="@name"/></xsl:attribute> 
				<xsl:attribute name="DataType">app.<xsl:value-of select="@domain"/></xsl:attribute> 
				<xsl:attribute name="DefaultValue"><xsl:value-of select="@defaultvalue"/></xsl:attribute> 
				<xsl:attribute name="FieldComment"><xsl:value-of select="@comment"/></xsl:attribute> 
				 <xsl:if test="'true' = @isprimarykey">
					<xsl:attribute name="PrimaryKeyNumber">1</xsl:attribute>
				</xsl:if>
				 <xsl:if test="'true' = @isrequired">
					<xsl:attribute name="AllowDBNull">false</xsl:attribute> 
				</xsl:if>
				<!--
				<xsl:attribute name="FieldAlias">FieldAliasGoesHere</xsl:attribute> 
				<xsl:attribute name="FieldExpression">FieldExpressionGoesHere</xsl:attribute> 
				<xsl:attribute name="DBFieldType">FieldStereotypeGoesHere</xsl:attribute>
				<xsl:attribute name="XmlFieldType">Attribute</xsl:attribute>
				<xsl:attribute name="NullValue">FieldNullValueGoesHere</xsl:attribute> 
				<xsl:attribute name="StringSize">99</xsl:attribute> 
				<xsl:attribute name="Unique">true</xsl:attribute> 
				<xsl:attribute name="AllowDBNull">false</xsl:attribute> 
				<xsl:attribute name="AutoIncrement">true</xsl:attribute> 
				<xsl:attribute name="AutoIncrementSeed">100</xsl:attribute> 
				<xsl:attribute name="AutoIncrementStep">10</xsl:attribute> 
				-->
				
			</FieldDefinition>
			
			</xsl:for-each>
			<xsl:variable name="tabname"><xsl:value-of select="@name"/></xsl:variable>

			<xsl:for-each select="//Relations/Relation[@primary=$tabname]">
			<!-- required attributes: RelationName,  ChildTableName
				ForeignKeys 	ChildTableName(reference to //TableDefinition/@TableName) 
				xxxxRule		None, Cascade, SetDefault, SetNull
				[@primary='@name']
				-->
			<RelationDefinition>
				<xsl:attribute name="RelationName"><xsl:value-of select="@name"/></xsl:attribute> 
				<xsl:attribute name="ChildTableName"><xsl:value-of select="@secondary"/></xsl:attribute> 
				<!-- 
				<xsl:attribute name="IsNested">true</xsl:attribute> 
				<xsl:attribute name="RelationComment">RelationCommentGoesHere</xsl:attribute> 
				<xsl:attribute name="AcceptRejectRule">Cascade</xsl:attribute> 
				<xsl:attribute name="UpdateRule">None</xsl:attribute> 
				<xsl:attribute name="DeleteRule">None</xsl:attribute> 
				<xsl:attribute name="ExportParentAlias">RelationRenameParentGoesHere</xsl:attribute> 
				<xsl:attribute name="ExportGetChildrenAlias">RelationRenameChildrenGoesHere</xsl:attribute> 
				<xsl:attribute name="RelationAlias">RelationAliasGoesHere</xsl:attribute> 
				<xsl:attribute name="DBParentFieldsUnique">true</xsl:attribute> 
				<xsl:attribute name="DBRelationType">RelationStereotypeGoesHere</xsl:attribute>
				-->
				
				<xsl:for-each select="Mapping">
				<!-- required attributes: ParentFieldName,  ChildFieldName
					ForeignKeys 	xxxxFieldName(reference to //TableDefinition/FieldDefinition/@FieldName) 
					-->
				<FieldRelationDefinition>
					<xsl:attribute name="ParentFieldName"><xsl:value-of select="@primary"/></xsl:attribute> 
					<xsl:attribute name="ChildFieldName"><xsl:value-of select="@secondary"/></xsl:attribute>
				</FieldRelationDefinition>
				</xsl:for-each>
				
			</RelationDefinition>
			</xsl:for-each>
		  </TableDefinition>
		</xsl:for-each>
		</SchemaXsd3b>
	</xsl:template>

	<xsl:template name="DataTypeDefinition-System" >
		<xsl:comment>//Domains/Domain/@java-class is System.xxx</xsl:comment>
		<xsl:for-each select="//Domains/Domain/@java-class">
		<xsl:sort />
		
		<!-- every value only once (unique) -->
		<xsl:if test="generate-id(current()) = generate-id(//Domains/Domain[@java-class=current()]/@java-class)">

		  <DataTypeDefinition>
			<xsl:attribute name="DataTypeName">System.<xsl:value-of select="."/></xsl:attribute> 
			<xsl:attribute name="ParentDataTypeName">DataType</xsl:attribute> 
		  </DataTypeDefinition>
		</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="DataTypeDefinition-sql" >
		<xsl:comment>//Domains/Domain/@definition ist sql.xxx</xsl:comment>
		<xsl:for-each select="//Domains/Domain/@definition">
		<xsl:sort />
		
		<!-- every value only once (unique) -->
		<xsl:if test="generate-id(current()) = generate-id(//Domains/Domain[@definition=current()]/@definition)">
		  <DataTypeDefinition>
			<xsl:attribute name="DataTypeName">sql.<xsl:value-of select="."/></xsl:attribute> 
			<xsl:attribute name="ParentDataTypeName">System.<xsl:value-of select="../@java-class"/></xsl:attribute> 
		  </DataTypeDefinition>
		</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="DataTypeDefinition-app" >
	
		<xsl:comment>//Domains/Domain/@name ist app.xxx</xsl:comment>
		<xsl:for-each select="//Domains/Domain/@name">
		<xsl:sort />
		
		  <DataTypeDefinition>
			<xsl:attribute name="DataTypeName">app.<xsl:value-of select="."/></xsl:attribute> 
			<xsl:attribute name="ParentDataTypeName">sql.<xsl:value-of select="../@definition"/></xsl:attribute> 
		  </DataTypeDefinition>
		
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>

