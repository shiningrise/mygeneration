<!--
    Document   : templates\xsd3b\DatabaseReport.xsd3b.htm.xsl
    Created on : 2007-05-03
    Author     : dl3bak

	Xsd3b is a relational mapping tool
	
	This template generate a html database report from xsd3b file
	
	Requirements
		Dl3bak.Xsd3b.dll
		Dl3bak.Xsd3b.xsd.dll
		Dl3bak.Xsd3b.xslt.dll
		templates\xsd3b\DatabaseReport.xsd3b.htm.xsl
		
		a host for Dl3bak.Xsd3b
			either xsd3bGui.exe
			or xsd3bcmd.exe
			or MyMeta.Plugins.Xsd3b.dll with MyGeneration
			
-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method= "html" />

	<xsl:template match="/" >
		<html>
			<head>
				<title>
					Database Model <xsl:value-of select="//SchemaDefinition/@SchemaID" />
				</title>
				<link href="xmi.css" rel="stylesheet" type="text/css"/>
			</head>
			<body>
				<xsl:call-template name="SchemaDefinition" />
				<xsl:call-template name="TableList" />
				<xsl:call-template name="RelationList" />
				
				<xsl:for-each select="//TableDefinition">
					<xsl:call-template name="TableDefinition" />
				</xsl:for-each>
				<hr />
				<h2>DataTypes</h2>
				<ul>
					<xsl:for-each select="/SchemaXsd3b/DataTypeDefinition">
						<xsl:call-template name="DataTypeDefinition" />
					</xsl:for-each>
				</ul>

			</body>
		</html>
	</xsl:template >

	<xsl:template name="SchemaDefinition">
		<h1>
			xsd3b Database Model <xsl:value-of select="//SchemaDefinition/@SchemaID" />
		</h1>
		<p>
			<pre>
				<xsl:value-of select="//SchemaDefinition/SchemaComment" />
			</pre>
		</p>
	</xsl:template>

	<xsl:template name="TableList">
		<h2>Tables</h2>
		<ul>
			<xsl:for-each select="//TableDefinition">

				<li>
					<xsl:call-template name="DokumentPK" />
				</li>
			</xsl:for-each>
		</ul>
	</xsl:template>

	<xsl:template name="TableDefinition">
		<hr />
		<h2>
			<a>
				<xsl:attribute name="name">
					<xsl:value-of select="@TableName"/>
				</xsl:attribute>
			</a>
			<xsl:call-template name="DokumentPK" />
		</h2>
		<p>
			<xsl:value-of select="@TableComment"/>
		</p>

		<xsl:call-template name="FieldDefinitionsByTable" />
		<xsl:call-template name="RelationsByTable" />

	</xsl:template>

	<xsl:template name="FieldDefinitionsByTable">
		<h4>Fields</h4>
		<ul>
			<xsl:for-each select="FieldDefinition">
				<xsl:call-template name="FieldDefinition" />
			</xsl:for-each>
		</ul>

	</xsl:template>


	<xsl:template name="FieldDefinition">
		<li>
		<xsl:if test="@DBFieldType">
			<xsl:text> {</xsl:text>
			<xsl:value-of select="@DBFieldType"/>
			<xsl:text>} </xsl:text>
		</xsl:if>
		<xsl:value-of select="@FieldName"/>
		<xsl:if test="@FieldAlias and @FieldAlias != @FieldName">
			<xsl:text>(</xsl:text>
			<xsl:value-of select="@FieldAlias"/>
			<xsl:text>)</xsl:text>
		</xsl:if>
		<xsl:text> : </xsl:text>
		<a>
			<xsl:attribute name="href">#<xsl:value-of select="@DataType"/></xsl:attribute>
			<xsl:value-of select="@DataType"/>
		</a>

		<xsl:if test="number(@StringSize) > 0">
			<xsl:text>(</xsl:text>
			<xsl:value-of select="@StringSize"/>
			<xsl:text>)</xsl:text>
		</xsl:if>
			<ul>
				<xsl:if test="@FieldComment">
					<li>
						<xsl:value-of select="@FieldComment"/>
					</li>				
				</xsl:if>
				<xsl:if test="@FieldExpression">
					<li>
						<xsl:text>FIELDEXPRESSION : </xsl:text>
						<xsl:value-of select="@FieldExpression"/>
					</li>
				</xsl:if>
				
				<li>					
					<xsl:if test="number(@PrimaryKeyNumber)>-1">
						<xsl:text>PK </xsl:text>
					</xsl:if>
					<xsl:if test="@Unique='true'">
						<xsl:text>UNIQUE </xsl:text>
					</xsl:if>
					<xsl:if test="@AllowDBNull='false'">
						<xsl:text>NOT_NULL </xsl:text>
					</xsl:if>
					<xsl:if test="@NullValue">
						<xsl:text>NULL(</xsl:text><xsl:value-of select="@NullValue"/><xsl:text>) </xsl:text>
					</xsl:if>
					<xsl:if test="@DefaultValue">
						<xsl:text>DEFAULT(</xsl:text>
						<xsl:value-of select="@DefaultValue"/>
						<xsl:text>) </xsl:text>
					</xsl:if>
					
					<xsl:if test="@ReadOnly='true'">
						<xsl:text>READONLY </xsl:text>
					</xsl:if>
					<xsl:if test="@AutoIncrement='true'">
						<xsl:text>AUTOINCREMENT(</xsl:text>						
						<xsl:value-of select="@AutoIncrementSeed"/>
						<xsl:text>,</xsl:text>
						<xsl:value-of select="@AutoIncrementStep"/>
						<xsl:text>) </xsl:text>
					</xsl:if>

					<xsl:if test="@XmlFieldType">
						<xsl:text>XML(</xsl:text>
						<xsl:value-of select="@XmlFieldType"/>
						<xsl:text>) </xsl:text>
					</xsl:if>
					
				</li>
			</ul>

		</li>
	</xsl:template>
	
	<xsl:template name="RelationsByTable">
		<xsl:variable name="tname">
			<xsl:value-of select="@TableName"/>
		</xsl:variable>

		<xsl:if test="//RelationDefinition[@ChildTableName=$tname or ../@TableName=$tname]">
			<h4>Relations</h4>
			<ul>
				<xsl:for-each select="RelationDefinition">

					<li>
						<xsl:call-template name="DokumentFK" />
						<xsl:if test="@RelationComment">
							<ul>
								<li>
									<xsl:value-of select="@RelationComment"/>
								</li>
								<li/>
							</ul>
						</xsl:if>
					</li>
				</xsl:for-each>
				<xsl:for-each select="//RelationDefinition[@ChildTableName=$tname]">
					<li>
						<xsl:call-template name="DokumentFK" />
						<xsl:if test="@RelationComment">
							<ul>
								<li>
									<xsl:value-of select="@RelationComment"/>
								</li>
								<li/>
							</ul>
						</xsl:if>
					</li>
				</xsl:for-each>

			</ul>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="RelationList">
		<h2>Relations</h2>
		<ul>
			<xsl:for-each select="//RelationDefinition">

				<li>
					<xsl:call-template name="DokumentFK" />

				</li>
			</xsl:for-each>
		</ul>
	</xsl:template>

	<xsl:template name="DokumentFK" >
		<!-- Dokumentation for sql-Foreing Key Relation
			Example
				FK_OrdersOrder Details : Order Details(OrderID) -> Orders (OrderID)

			. = //RelationDefinition
		
		-->
		<xsl:variable name="parentTName">
			<xsl:value-of select="../@TableName"/>
		</xsl:variable>
		<xsl:variable name="childTName">
			<xsl:value-of select="@ChildTableName"/>
		</xsl:variable>
		<xsl:variable name="relName">
			<xsl:value-of select="@RelationName"/>
		</xsl:variable>

		<xsl:if test="@DBRelationType">
			<xsl:text> {</xsl:text>
			<xsl:value-of select="@DBRelationType"/>
			<xsl:text>} </xsl:text>
		</xsl:if>

		<xsl:value-of select="$relName"/>
		<xsl:text> : </xsl:text>
		<a>
			<xsl:attribute name="href">
				#<xsl:value-of select="$parentTName"/>
			</xsl:attribute>
			<xsl:value-of select="$parentTName"/>
		</a>
		<xsl:text>(</xsl:text>
		<xsl:for-each select="FieldRelationDefinition">
			<xsl:variable name="fname">
				<xsl:value-of select="@ParentFieldName"/>
			</xsl:variable>
			<xsl:value-of select="$fname"/>
			<xsl:if test="position() != last()">
				<xsl:text>, </xsl:text>
			</xsl:if>
		</xsl:for-each>
		<xsl:text>) &lt;= </xsl:text>
		<a>
			<xsl:attribute name="href">
				#<xsl:value-of select="$childTName"/>
			</xsl:attribute>
			<xsl:value-of select="$childTName"/>
		</a>		
		<xsl:text>(</xsl:text>
		<xsl:for-each select="FieldRelationDefinition">
			<xsl:variable name="fname">
				<xsl:value-of select="@ChildFieldName"/>
			</xsl:variable>
			<xsl:value-of select="$fname"/>
			<xsl:if test="position() != last()">
				<xsl:text>, </xsl:text>
			</xsl:if>
		</xsl:for-each>
		<xsl:text>)
</xsl:text>

	</xsl:template>

	<xsl:template name="DokumentPK" >
		<!-- Dokumentation for sql-Table with PKs
			Example
				Order Details(OrderID)

			. = //TableDefinition
		
		-->
		<xsl:variable name="tname">
			<xsl:value-of select="@TableName"/>
		</xsl:variable>

		<xsl:if test="@DBTableType">
		<xsl:text> {</xsl:text>
		<xsl:value-of select="@DBTableType"/>
		<xsl:text>} </xsl:text>
		</xsl:if>
		<a>
			<xsl:attribute name="href">#<xsl:value-of select="$tname"/></xsl:attribute>
			<xsl:value-of select="$tname"/>
		</a>
		<xsl:text>(</xsl:text>
		<xsl:for-each select="FieldDefinition[number(@PrimaryKeyNumber) >= 0]">
			<xsl:variable name="fname">
				<xsl:value-of select="@FieldName"/>
			</xsl:variable>
			<xsl:value-of select="$fname"/>
			<xsl:if test="position() != last()">
				<xsl:text>, </xsl:text>
			</xsl:if>
		</xsl:for-each>
		<xsl:text>)
</xsl:text>

	</xsl:template>

	<xsl:template name="DataTypeDefinition">
		<xsl:variable name="dtname">
			<xsl:value-of select="@DataTypeName"/>
			<xsl:if test="number(@StringSize)">
				<xsl:text>(</xsl:text>
				<xsl:value-of select="@StringSize"/>
				<xsl:text>)</xsl:text>
			</xsl:if>
		</xsl:variable>

		<li>
			<xsl:if test="@DBDomainType">
				<xsl:text> {</xsl:text>
				<xsl:value-of select="@DBDomainType"/>
				<xsl:text>} </xsl:text>
			</xsl:if>
			<a>
				<xsl:attribute name="name">
					<xsl:value-of select="$dtname"/>
				</xsl:attribute>
				<xsl:value-of select="$dtname"/>
			</a>
			<xsl:if test="@DataTypeComment">
				<xsl:text> : </xsl:text>
				<xsl:value-of select="@DataTypeComment"/>
			</xsl:if>
			<ul>
				<xsl:for-each select="DataTypeDefinition">
					<xsl:call-template name="DataTypeDefinition" />
				</xsl:for-each>
			</ul>
		</li>
	</xsl:template>
	
</xsl:stylesheet>

