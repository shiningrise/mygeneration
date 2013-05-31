using System;
using System.Text;
using System.IO;
using System.Collections;

namespace Zeus
{
	/// <summary>
	/// The ZeusOutput object is only available in the template code segment and is very much like a 
	/// StringBuilder. It contains methods that allow the developer to manipulate the current output buffer. 
	/// </summary>
	/// <remarks>
	/// The ZeusOutput is simply an output buffer. It's primary methods are <see cref="write">write()</see> and 
	/// <see cref="writeln">writeln()</see>, which naturally
	/// append string data to the buffer. Using the save and append methods, the buffer can be saved to disk.
	/// Code preservation is also supported using the ZeusOutput object. The two methods that enable code preservation 
	/// are <see cref="setPreserveSource">setPreserveSource()</see>, <see cref="preserve">preserve()</see>, <see cref="getPreservedData">getPreservedData()</see>, 
	/// and <see cref="getPreserveBlock">getPreserveBlock()</see>. The <see cref="preserve">preserve()</see> method
	/// writes directly to the output stream where the <see cref="getPreserveBlock">getPreserveBlock()</see> returns the entire preserve block as a string.
	/// You can then write that string to the output buffer manually. The <see cref="getPreservedData">getPreservedData()</see> method returns the data inside the
	/// preserve block without the preserve tags.
	/// </remarks>
	/// <example>
	/// Saving the buffer to disk, replacing an existing file: (JScript)
	/// <code>
	/// var filename = "c:\testfile.txt";
	/// output.write("Hello World!");
	/// output.save(filename, "o");
	/// </code>
	/// </example>
	/// <example>
	/// Backing up an existing file before overwriting: (JScript)
	/// <code>
	/// var filename = "c:\testfile.txt";
	/// output.write("Hello World!");
	/// output.save(filename, "b");
	/// </code>
	/// </example>
	/// <example>
	/// Append the buffer to an existing file before overwriting: (JScript)
	/// <code>
	/// var filename = "c:\testfile.txt";
	/// output.write("Hello World!");
	/// output.save(filename, "a");
	/// </code>
	/// </example>
	/// <example>
	/// Save the buffer to an new file with no overwrite (if the file exists, nothing will happen): (JScript)
	/// <code>
	/// var filename = "c:\testfile.txt";
	/// output.write("Hello World!");
	/// output.save(filename, "d");
	/// </code>
	/// </example>
	/// <example>
	/// Preserving a region the buffer to an existing file before overwriting: (JScript)
	/// <code>
	/// // The Template Code
	/// var filename = "c:\testfile.txt";
	/// output.setPreserveSource(filePath, "/*::", "::*/"); 
	/// output.write("Hello World!");
	/// output.preserve("myCustomProperties");
	/// output.write("Hello World Again!");
	/// output.save(filename, "o");
	/// </code>
	/// The existing file before template execution: "c:\testfile.txt"
	/// <code>
	/// Hello World!
	/// /*::PRESERVE_BEGIN myCustomProperties::*/ 
	/// preserved data here
	/// /*::PRESERVE_END myCustomProperties::*/ 
	/// Hello World Again!
	/// </code>
	/// Note that the preserved data is between the PRESERVE_BEGIN and PRESERVE_END tags. 
	/// When you set the preserve source, the start and end tags for the PRESERVE tags
	/// are defined; is this case, it's "/*::" and "::*/".
	/// </example>
	public class ZeusOutput : IZeusOutput
	{
		private const string PRESERVE_BEGIN = "PRESERVE_BEGIN";
		private const string PRESERVE_END = "PRESERVE_END";

		private string _preservePrefix = "//::";
		private string _preserveSuffix = ":://";
		private int _tablevel = 0;
		private Hashtable _preserveData = null;
		private StringBuilder _output = new StringBuilder();

		/// <summary>
		/// Creates a new ZeusOutput object.
		/// </summary>
		public ZeusOutput() {}

		/// <summary>
		/// Writes the inputed string, text, to the output buffer.
		/// </summary>
		/// <param name="text">A string to write to the output buffer</param>
		public void write(string text) 
		{
			_output.Append(text);
		}

		/// <summary>
		/// Writes the inputed string, text, to the output buffer followed by a newline.
		/// </summary>
		/// <param name="text">A string to write to the output buffer</param>
		public void writeln(string text) 
		{
			_output.Append(text);
			_output.Append("\r\n");
		}

		/// <summary>
		/// Writes the inputed string, text, to the output buffer, prepended with the number of
		/// tabs specified by the tabLevel property.
		/// </summary>
		/// <param name="text">A string to write to the output buffer</param>
		public void autoTab(string text)
		{
			for (int i = 0; i < this._tablevel; i++) _output.Append("\t");
			_output.Append(text);
		}

		/// <summary>
		/// Writes the inputed string, text, to the output buffer followed by a newline, 
		/// prepended with the number of
		/// tabs specified by the tabLevel property.
		/// </summary>
		/// <param name="text"></param>
		public void autoTabLn(string text)
		{
			autoTab(text);
			_output.Append("\r\n");
		}

		/// <summary>
		/// Increments the tabLevel property.
		/// </summary>
		public void incTab() 
		{
			this._tablevel++;
		}

		/// <summary>
		/// Decrements the tabLevel property.
		/// </summary>
		public void decTab() 
		{
			this._tablevel--;
		}

		/// <summary>
		/// The tabLevel property is the number of tabs that are prepended to outputted text 
		/// when using the autoTab and autoTabLn methods.
		/// </summary>
		public int tabLevel
		{
			get { return _tablevel; }
			set { this._tablevel = value; }
		}

		/// <summary>
		/// Sets or gets the current output buffer.
		/// </summary>
		public string text
		{
			get 
			{
				return _output.ToString();
			}

			set 
			{
				this.clear();
				this._output.Append(value);
			}
		}

		/// <summary>
		/// Clears the output buffer.
		/// </summary>
		public void clear() 
		{
			_output.Remove(0, _output.Length);
		}

		/// <summary>
		/// Save the current buffer to a file at path. If the file exists, append to the file.
		/// </summary>
		/// <param name="path">The path of the file to append to.</param>
		public void append(string path) 
		{
			StreamWriter writer;

			if (File.Exists(path)) 
			{
				writer = File.AppendText(path);
			}
			else 
			{
				writer = File.CreateText(path);
			}

			writer.Write(_output.ToString());
			writer.Close();
		}

		public void save(string path, object action) 
		{
			saveEnc(path, action, Encoding.Default);
		}
		
		/// <summary>
		/// Save the current buffer to a file at path. If the file exists, 
		/// backup the existing file (by renaming it) and replace it. 
		///<code>
		///output.save(filename, "d", "ascii"); // Save only if file doesn't exists
		///output.save(filename, "o", "utf7"); // Overwrite
		///output.save(filename, "b", "unicode"); // Backup and overwrite
		///output.save(filename, "a", "utf8"); // Append
		///</code>
		/// </summary>
		/// <param name="path">The path of the file to write to.</param>
		/// <param name="action">
		/// "d" or "default" saves the file if it doesn't exist, 
		/// "o" or "overwrite" saves the file even if it has to overwrite, 
		/// "b" or "backup" backs up the existing file before overwriting it,
		/// "a" or "append" appends to the end of the current file if it exists,
		/// true or false for backup or overwrite (for legacy support)</param>
		/// <param name="encoding">
		/// The encoding object or a string value ("utf8", "utf7", "ascii", "unicode", "bigendianunicode").
		///</param>
		public void saveEnc(string path, object action, object encoding) 
		{
			Encoding enc = encoding as Encoding;

			if ((encoding != null) && (enc == null) && (enc != Encoding.Default))
			{
				string tmp = encoding.ToString().ToLower();

				if (tmp == "ascii")					enc = Encoding.ASCII;
				else if (tmp == "unicode")			enc = Encoding.Unicode;
				else if (tmp == "bigendianunicode")	enc = Encoding.BigEndianUnicode;
				else if (tmp == "utf7")				enc = Encoding.UTF7;
				else if (tmp == "utf8")				enc = Encoding.UTF8;
				else								enc = Encoding.Default;
			}

			StreamWriter writer = null;

			FileInfo finfo = new FileInfo(path);
			string a = "o";
			if (action is Boolean) 
			{
				a = ((bool)action) ? "b" : "o";
			}
			else if (action is String) 
			{
				if (action.ToString().Length > 0)
				{
					a = action.ToString().ToLower().Substring(0,1);
				}
			}

			if (finfo.Exists) 
			{
				if (a == "b")
				{
					int i = 0;
					string tmpfilename = string.Empty;
					do 
					{
						i++;
						tmpfilename = path + "." + i.ToString().PadLeft(3, '0') + ".bak";
						
					} while (File.Exists(tmpfilename));

					File.Move(path, tmpfilename);
					if (enc != Encoding.Default) writer = new StreamWriter(path, false, enc);
					else writer = new StreamWriter(path, false);
				}
				else if (a == "o")
				{
					File.Delete(path);
					if (enc != Encoding.Default) writer = new StreamWriter(path, false, enc);
					else writer = new StreamWriter(path, false);
				}
				else if (a == "a")
				{
					if (enc != Encoding.Default) writer = new StreamWriter(path, true, enc);
					else writer = new StreamWriter(path, true);
					//writer = File.AppendText(path);
				}
			}
			else 
			{
				// Create the directory if it doesn't exist!
				DirectoryInfo dinfo = new DirectoryInfo(finfo.DirectoryName);
				if (!dinfo.Exists)
				{
					dinfo.Create();
				}

				if (enc != Encoding.Default) writer = new StreamWriter(path, false, enc);
				else writer = new StreamWriter(path, false);
				//writer = File.CreateText(path);
			}

			if (writer != null) 
			{
				writer.Write(_output.ToString());
				writer.Close();
			}
		}

		/// <summary>
		/// The file at the path parameter will be opened up and all PRESERVE blocks will be loaded
		/// and kept for use in this ZeusOutput object. The preserve(key) method can be used to get
		/// preserved code segments by it's key parameter.
		/// </summary>
		/// <param name="path">A path to the file that contains code segments that are to be preserved.</param>
		/// <param name="prefix">Prefix to the PRESERVE_BEGIN tag.</param>
		/// <param name="suffix">Suffix to the PRESERVE_END tag.</param>
		public void setPreserveSource(string path, string prefix, string suffix) 
		{
			this._preservePrefix = prefix;
			this._preserveSuffix = suffix;
			this._preserveData = new Hashtable();

			if (File.Exists(path))
			{
				StreamReader reader = File.OpenText(path);
				bool inBlock = false;
				string beginPreserve = prefix + PRESERVE_BEGIN;
				string endPreserve = prefix + PRESERVE_END;
				string key = string.Empty;
				StringBuilder data = new StringBuilder();
				
				string line = reader.ReadLine();
				while (line != null) 
				{
					if (inBlock) 
					{
						int i = line.IndexOf(endPreserve);
						if (i >= 0) 
						{
							data.Append(line.Substring(0, i));
							this._preserveData[key] = data.ToString();
							data = new StringBuilder();
							
							line = line.Substring(i);

							inBlock = false;
							continue;
						}
						else 
						{
							data.Append(line);
							data.Append(Environment.NewLine);
						}
					}
					else 
					{
						int i = line.IndexOf(beginPreserve);
						if (i >= 0) 
						{
							i = i + beginPreserve.Length;
							int j = line.IndexOf(suffix, i);
							if (j >= 0) 
							{
								key = line.Substring(i, (j - i)).Trim();
								j = j + suffix.Length;
								
								line = line.Substring(j);
								
								inBlock = true;
								continue;
							}
						}
					}
					
					line = reader.ReadLine();
				}
				reader.Close();
			}
		}

		/// <summary>
		/// If the setPreserveSource(targetFile, prefex, suffix) function was called, all of the custom code from
		/// the targetFile is stored in the ZeusOutput object. When the preserve method is called, the code
		/// segment that correlates to the key parameter is written to the output buffer surrounted by the appropriate preserve tags.
		/// </summary>
		/// <param name="key">The key that identifies the desired code segment</param>
		public void preserve(string key)
		{
			_output.Append( getPreserveBlock(key) );
		}

		/// <summary>
		/// This function returns the preserved data (without the preserve tags) that corresponds to the key parameter.
		/// </summary>
		/// <param name="key">The key that identifies the desired code segment</param>
		/// <returns>The preserved data that corresponds to the key parameter</returns>
		public string getPreservedData(string key)
		{
			return this._preserveData[key].ToString();
		}

		/// <summary>
		/// This function returns the preserved data with the preserve tags that corresponds to the key parameter.
		/// </summary>
		/// <param name="key">The key that identifies the desired code segment</param>
		/// <returns>The preserved data with the preserve tags.</returns>
		public string getPreserveBlock(string key) 
		{
			if (this._preserveData != null) 
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(this._preservePrefix);
				sb.Append(PRESERVE_BEGIN);
				sb.Append(" ");
				sb.Append(key);
				sb.Append(this._preserveSuffix);

				if (this._preserveData.Contains(key))
				{
					sb.Append( this.getPreservedData(key) );
				}

				sb.Append(this._preservePrefix);
				sb.Append(PRESERVE_END);
				sb.Append(" ");
				sb.Append(key);
				sb.Append(this._preserveSuffix);

				return sb.ToString();
			}

			return string.Empty;
		}
	}
}