
// Function to escape data based on it's type
function escapeDataItem(dbtype, datatype, data)
{

	// Escapes SQL Server data
	if (dbtype == "SQL") 
	{
		if (data == null) 
		{
			data = "NULL";
		}
		else if ((datatype == "nvarchar") ||
			(datatype == "varchar") ||
			(datatype == "ntext") ||
			(datatype == "text") ||
			(datatype == "nchar") ||
			(datatype == "char"))
		{
			data = data.split("'").join("''");
			data = "'" + data + "'";
		}
		else if (datatype == "uniqueidentifier") 
		{
			if (data.charAt(0) == '{')
			{
				data = data.substring(1, data.length - 1);
			}

			data = "'" + data + "'";
		}
		else if ((datatype == "datetime") || (datatype == "smalldatetime"))
		{
			// Format like this: 7/16/2003 10:24:00 AM
			data = "'" + formatDateTime("" + data) + "'";
		}
		else if (datatype == "bit")
		{
			if (data == "true")
				data = "1";
			else
				data = "0";
		}
		else if (datatype == "image")
		{
			throw Error("The 'image' data type of SQL server is not supported in this script.");
		}
		else if (datatype == "sql_variant")
		{
			throw Error("The 'sql_variant' data type of SQL server is not supported in this script.");
		}
	}

	return data;
}



function formatDateTime(sDate) 
{
	var my_date = new Date(sDate)

	var month = "" + (my_date.getMonth() + 1); 
	var day = "" + my_date.getDate(); 
	var year = "" + my_date.getFullYear();
	var hours = "" + my_date.getHours(); 
	var minutes = "" + my_date.getMinutes(); 
	var seconds = "" + my_date.getSeconds();
	
	var returnval = (month.length == 1 ? '0' + month : month) + "/" + 
		(day.length == 1 ? '0' + day : day) + "/" + 
		year + " " + 
		(hours.length == 1 ? '0' + hours : hours) + ":" + 
		(minutes.length == 1 ? ("0" + minutes) : minutes) + ":" + 
		(seconds.length == 1 ? ("0" + seconds) : seconds); 
		
	return returnval;
} 


function getTableDep(db, boolOrder)
{
	var objs = new coll();
	var numadded = -1;
	
	while (numadded != 0)
	{
		numadded = 0;
		
		for (var i = 0; i < db.Tables.Count; i++)
		{
			var tableMeta = db.Tables.Item(i);
			var tablename = tableMeta.Name;
			var fkeys = tableMeta.ForeignKeys;
		
			 if (fkeys.Count == 0) 
			{
				// If there are no foreign keys, add it to the collection.
				if (objs.add(tablename))
					numadded++;
			}
			else 
			{
				// If there are foreign keys, loop through them and see if
				// all of the referencing tables are already in the collection.
				// If they are all in there, add the table.
				var allExist = true;
				for (var x = 0; x < fkeys.Count; x++)
				{
					fkey = fkeys.item(x);
					var primaryTable = fkey.PrimaryTable;
					
					// only look at direct foriegn keys
					if (primaryTable == tablename)
					{
						if ( (objs.indexOf(fkey.ForeignTable) == -1) && (fkey.ForeignTable != tablename) )
						{
							allExist = false;
							break;
						}
					}
				}
	
				if (allExist) 
					if (objs.add(tablename))
						numadded++;
			}
		}
	}
	
	// Any tables left over get added to the end of the list
	for (var i = 0; i < db.Tables.Count; i++)
	{
		var tablename = db.Tables.Item(i).Name;
		if (objs.indexOf(tablename) == -1) 
		{
			if (objs.add(tablename))
				numadded++;
		}
	}

	return objs.getItems(boolOrder);
}


function getViewDep(db, boolOrder)
{
	var objs = new coll();
	var numadded = -1;
	
	while (numadded != 0)
	{
		numadded = 0;
		
		for (var i = 0; i < db.Views.Count; i++)
		{
			var viewMeta = db.Views.Item(i);
			var viewname = viewMeta.Name;
			var subviews = viewMeta.SubViews;
		
			/*for (var x = 0; x < viewMeta.SubViews.Count; x++)
			{
				subview = viewMeta.SubViews.item(x);
				output.write(viewname + " -> V::" + subview.Name);
			}
			for (var x = 0; x < viewMeta.SubTables.Count; x++)
			{
				subtable = viewMeta.SubTables.item(x);
				output.write(viewname + " -> T::" + subtable.Name);
			}*/
			if (subviews.Count == 0) 
			{
				// If there are no foreign keys, add it to the collection.
				if (objs.add(viewname))
					numadded++;
			}
			else 
			{
				// If there are foreign keys, loop through them and see if
				// all of the referencing tables are already in the collection.
				// If they are all in there, add the table.
				var allExist = true;
				for (var x = 0; x < subviews.Count; x++)
				{
					subview = subviews.item(x);
					output.writeln(viewname + " -> " + subview.Name);
					
					if ( (objs.indexOf(subview.Name) == -1) && (subview.Name != viewname) )
					{
						allExist = false;
						break;
					}
				}
	
				if (allExist) 
					if (objs.add(viewname))
						numadded++;
			}
		}
	}
	
	// Any tables left over get added to the end of the list
	for (var i = 0; i < db.Views.Count; i++)
	{
		var viewname = db.Views.Item(i).Name;
		if (objs.indexOf(viewname) == -1) 
		{
			if (objs.add(viewname))
				numadded++;
		}
	}

	return objs.getItems(boolOrder);
}

function coll()
{
	this._outarray = null;
	this.items = new Array();
	this.add = coll__add;
	this.remove = coll__remove;
	this.indexOf = coll__indexOf;
	this.getItems = coll__getItems;
}

function coll__add(item) 
{
	if (this.indexOf(item) == -1) 
	{
		this._outarray = null;
		this.items.push(item);
		return true;
	}
	return false;
}

function coll__remove(item) 
{
	var k = this.indexOf(item);

	if (item == this.items[k])
	{
		this._outarray = null;
		this.items[k] = "<_NULL_>";
		return true;
	}
	return false;
}

function coll__getItems(order) 
{
	if (this._outarray == null)
	{
	
		this._outarray = new Array();
		if (order) 
		{
			for (var k=0; k < this.items.length; k++) 
			{
				if (this.items[k] != "<_NULL_>")
					this._outarray.push(this.items[k]);
			}
		}
		else
		{
			for (var k=this.items.length - 1; k >= 0 ; k--) 
			{
				if (this.items[k] != "<_NULL_>")
					this._outarray.push(this.items[k]);
			}
		}
	}
	return this._outarray;
}

function coll__indexOf(item) 
{
	for (var k=0; k < this.items.length; k++) 
	{
		if (item == this.items[k])
		{
			return k;
		}
	}
	return -1;
}