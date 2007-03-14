/*
 *  Firebird ADO.NET Data provider for .NET and Mono 
 * 
 *     The contents of this file are subject to the Initial 
 *     Developer's Public License Version 1.0 (the "License"); 
 *     you may not use this file except in compliance with the 
 *     License. You may obtain a copy of the License at 
 *     http://www.firebirdsql.org/index.php?op=doc&id=idpl
 *
 *     Software distributed under the License is distributed on 
 *     an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either 
 *     express or implied.  See the License for the specific 
 *     language governing rights and limitations under the License.
 * 
 *  Copyright (c) 2002, 2006 Carlos Guzman Alvarez
 *  All Rights Reserved.
 */

using System;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;

using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using NUnit.Framework;

namespace FirebirdSql.Data.UnitTests
{
	[TestFixture]
	public class FbScriptTest : BaseTest
	{
		#region � Constructors �

		public FbScriptTest()
		{
		}

		#endregion

		#region � Unit Tests �

		[Test]
		public void IsqlScriptTest()
		{
			string fileName = ConfigurationManager.AppSettings["IsqlScript"];
			if (System.IO.File.Exists(fileName))
			{
				FbScript isql = new FbScript(fileName);
				foreach (string command in isql.Results)
				{
					Console.WriteLine(command);
				}
			}
		}

		#endregion
	}
}