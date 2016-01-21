﻿using System;
using System.Text;

using IniParser;
using IniParser.Model;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ifme
{
	public class Get
	{
		static StringComparison IC { get { return StringComparison.InvariantCultureIgnoreCase; } }

		public static string MediaContainer(string codec)
		{
			IniData getFmt = new FileIniDataParser().ReadFile("format.ini", Encoding.UTF8);
			string format = string.Empty;

			try
			{
				format = getFmt["format"][codec];
			}
			catch (Exception)
			{
				Console.WriteLine("Requested file container not found, using default");
			}
			finally
			{
				if (string.IsNullOrEmpty(format))
					format = codec;
            }

			return format;
		}

		public static string FileName(string file)
		{
			return Path.GetFileName(file);
		}

		public static string FileNameOnly(string file)
		{
			return Path.GetFileNameWithoutExtension(file);
		}

		public static string FileExtension(string filePath)
		{
			return Path.GetExtension(filePath).Substring(1); // jump dot
		}

		public static string FileFolder(string filePath)
		{
			return Path.GetDirectoryName(filePath);
		}

		public static string FileLang(string file)
		{
			file = Path.GetFileNameWithoutExtension(file);
			return file.Substring(file.Length - 3);
		}

		public static string FileSize(string file)
		{
			FileInfo f = new FileInfo(file);
			long byteCount = f.Length;

			string[] IEC = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };
			if (byteCount == 0)
				return "0" + IEC[0];

			long bytes = Math.Abs(byteCount);
			int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
			double num = Math.Round(bytes / Math.Pow(1024, place), 1);
			return (Math.Sign(byteCount) * num).ToString() + " " + IEC[place];
		}

		public static bool IsPathNetwork(string path)
		{
			if (path[0] == '\\')
				if (path[1] == '\\')
					return true;

			if (path[0] == 'f')
				if (path[1] == 'i')
					return true;

			return false;
		}

		public static string LanguageFullName(string code)
		{
			foreach (var item in File.ReadAllLines("iso.code"))
				if (string.Equals(code, item.Substring(0, 3), IC))
					return item;

			return string.Empty;
		}

		public static string LanguageCodeName(string fullname)
		{
			return fullname.Substring(0, 3); // capture only 3 letter
		}

		public static bool SubtitleValid(string file)
		{
			string exts = Path.GetExtension(file);
			if (string.Equals(exts, ".ass", IC))
				return true;
			else if (string.Equals(exts, ".ssa", IC))
				return true;
			else if (string.Equals(exts, ".srt", IC))
				return true;
			else
				return false;
		}

		public static string AttachmentValid(string file)
		{
			FileInfo f = new FileInfo(file);

			if (f.Length >= 1073741824)
				return "application/octet-stream";

			byte[] data = File.ReadAllBytes(file);
			byte[] MagicTTF = { 0x00, 0x01, 0x00, 0x00, 0x00 };
			byte[] MagicOTF = { 0x4F, 0x54, 0x54, 0x4F, 0x00 };
			byte[] MagicWOFF = { 0x77, 0x4F, 0x46, 0x46, 0x00 };
			byte[] check = new byte[5];

			Buffer.BlockCopy(data, 0, check, 0, 5);

			if (MagicTTF.SequenceEqual(check))
				return "application/x-truetype-font";

			if (MagicOTF.SequenceEqual(check))
				return "application/vnd.ms-opentype";

			if (MagicWOFF.SequenceEqual(check))
				return "application/font-woff";

			return "application/octet-stream";
		}

		public static string Duration(DateTime past)
		{
			TimeSpan span = DateTime.Now.Subtract(past);

			if (span.Days != 0)
				return $"{span.Days}d {span.Hours}h {span.Minutes}m {span.Seconds}s {span.Milliseconds}ms";
			else if (span.Hours != 0)
				return $"{span.Hours}h {span.Minutes}m {span.Seconds}s {span.Milliseconds}ms";
			else if (span.Minutes != 0)
				return $"{span.Minutes}m {span.Seconds}s {span.Milliseconds}ms";
			else
				return $"{span.Seconds}s {span.Milliseconds}ms";
		}

		public static string AviSynthGetFile(string file)
		{
			if (string.Equals(Path.GetExtension(file), ".avs", IC))
			{
				foreach (var item in File.ReadAllLines(file))
				{
					foreach (var code in File.ReadAllLines("avisynthsource.code"))
					{
						if (item.Contains(code))
						{
							var match = Regex.Match(item, $"{code}\\(\"(.*)\"");
							if (match.Success)
							{
								file = match.Groups[1].Value;
							}
						}
					}
				}
			}

			if (file[0] == '/' || file[1] == ':') // check path is full or just file
			{
				return file; // return if file is full path
			}
			else
			{
				return Path.Combine(Path.GetDirectoryName(file), file); // merge current folder & path
			}
		}
	}
}
