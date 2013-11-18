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
 *  Copyright (c) 2011-2013 Jiri Cincura (jiri@cincura.net)
 *  All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FirebirdSql.Data.Common
{
	static class Extensions
	{
		public static bool HasFlag(this Enum e, Enum flag)
		{
#if (!NET_40)
			return ((Convert.ToInt32(e) & Convert.ToInt32(flag)) != 0);
#else
			return e.HasFlag(flag);
#endif
		}

		public static Version ParseServerVersion(this string s)
		{
			Match m = Regex.Match(s, @"\w{2}-\w(\d+\.\d+\.\d+\.\d+) .*");
			if (!m.Success)
				return null;
			return new Version(m.Groups[1].Value);
		}

		public static bool CultureAwareEquals(this string string1, string string2)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(
				string1,
				string2,
				CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth |
				CompareOptions.IgnoreCase) == 0 ? true : false;
		}

		public static string StringJoin(this IEnumerable<string> data, string separator)
		{
#if (NET_35)
			return string.Join(separator, data.ToArray());
#else
			return string.Join(separator, data);
#endif
		}

		public static bool SetKeepAlive(this Socket socket, ulong time, ulong interval)
		{
			const int BytesPerLong = 4;
			const int BitsPerByte = 8;

			bool turnOn = time != 0 && interval != 0;
			ulong[] input = new[]
            	{
            		turnOn ? (ulong)1 : (ulong)0,
					time,
					interval
				};

			// tcp_keepalive struct
			byte[] inValue = new byte[3 * BytesPerLong];
			for (int i = 0; i < input.Length; i++)
			{
				inValue[i * BytesPerLong + 3] = (byte)(input[i] >> ((BytesPerLong - 1) * BitsPerByte) & 0xFF);
				inValue[i * BytesPerLong + 2] = (byte)(input[i] >> ((BytesPerLong - 2) * BitsPerByte) & 0xFF);
				inValue[i * BytesPerLong + 1] = (byte)(input[i] >> ((BytesPerLong - 3) * BitsPerByte) & 0xFF);
				inValue[i * BytesPerLong + 0] = (byte)(input[i] >> ((BytesPerLong - 4) * BitsPerByte) & 0xFF);
			}

			byte[] outValue = BitConverter.GetBytes(0);

			try
			{
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, turnOn ? 1 : 0);
				socket.IOControl(IOControlCode.KeepAliveValues, inValue, outValue);
			}
			catch (SocketException)
			{
				return false;
			}
			return true;
		}

		public static Task ConnectAsync(this Socket socket, IPEndPoint remoteEP)
		{
			return Task.Factory.FromAsync<IPEndPoint>(socket.BeginConnect, socket.EndConnect, remoteEP, null);
		}
	}
}
