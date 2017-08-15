﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

using Newtonsoft.Json;


namespace ifme
{
	static class Get
	{
		public static bool IsReady { get; set; } = false;

		public static Dictionary<string, string> LanguageCode
		{
			get
			{
				return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(AppRootDir, "language.json")));
			}
		}

		public static Dictionary<string, string> MimeList
		{
			get
			{
				return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(AppRootDir, "mime.json")));
			}
		}

		public static SortedSet<string> MimeTypeList
		{
			get
			{
				var temp = new SortedSet<string>();

				foreach (var item in MimeList)
				{
					try { temp.Add(item.Value); } catch { }
				}

				return temp;
			}
		}

		public static string AppPath
		{
			get
			{
				return Assembly.GetExecutingAssembly().Location;
			}
		}

		public static string AppRootDir
		{
			get
			{
				return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			}
		}

		public static Icon AppIcon
		{
			get
			{
				return Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
			}
		}

		public static string AppName
		{
			get
			{
				return Branding.Title();
			}
		}

		public static string AppNameLong
		{
			get
			{
				return $"{AppName} v{Application.ProductVersion} ('{Properties.Resources.AppCodeName}')";
			}
		}

		public static string AppNameLib
		{
			get
			{
				return $"{Branding.TitleShort()} v{Application.ProductVersion} {(OS.Is64bit ? "amd64" : "i686")} {(OS.IsWindows ? "windows" : "unix-like")}";
			}
		}

		public static string FolderTemp
		{
			get
			{
				if (string.IsNullOrEmpty(Properties.Settings.Default.TempDir))
				{
					Properties.Settings.Default.TempDir = Path.Combine(Path.GetTempPath(), "IFME");
					Properties.Settings.Default.Save();
				}
				
				return Properties.Settings.Default.TempDir;
			}
		}

		public static string FolderSave
		{
			get
			{
				var outdir = Properties.Settings.Default.OutputDir;

				// make sure path is full
				if (outdir.Length >= 2)
				{
					if (OS.IsLinux)
					{
						if (outdir[0] != '/')
							outdir = string.Empty;
					}
					else
					{
						if (outdir[1] != ':')
							outdir = string.Empty;
					}

				}

				if (string.IsNullOrEmpty(outdir))
				{
					var path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

					// windows xp
					if (path.IsDisable())
						path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

					Properties.Settings.Default.OutputDir = Path.Combine(path, "IFME");
					Properties.Settings.Default.Save();
				}

				return Properties.Settings.Default.OutputDir;
			}
			set
			{
				Properties.Settings.Default.OutputDir = value;
				Properties.Settings.Default.Save();
			}
		}

		public static string CodecFormat(string codecId)
		{
			var json = File.ReadAllText(Path.Combine(AppRootDir, "format.json"));
			var format = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			var formatId = string.Empty;

			if (format.TryGetValue(codecId, out formatId))
				return formatId;

			return "mkv";
		}

		public static string FileLang(string file)
		{
			file = Path.GetFileNameWithoutExtension(file);
			return file.Substring(file.Length - 3);
		}

		public static string LangCheck(string lang)
		{
			var temp = string.Empty;

			if (LanguageCode.TryGetValue(lang, out temp))
			{
				return lang; // if found
			}

			return "und";
		}

		public static string FileSizeIEC(long InBytes)
		{
			string[] IEC = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };

			if (InBytes == 0)
				return $"0{IEC[0]}";

			long bytes = Math.Abs(InBytes);
			int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
			double num = Math.Round(bytes / Math.Pow(1024, place), 1);
			return $"{(Math.Sign(InBytes) * num)}{IEC[place]}";
		}

		public static string FileSizeDEC(long InBytes)
		{
			string[] DEC = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

			if (InBytes == 0)
				return $"0{DEC[0]}";

			long bytes = Math.Abs(InBytes);
			int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1000)));
			double num = Math.Round(bytes / Math.Pow(1000, place), 1);
			return $"{(Math.Sign(InBytes) * num)}{DEC[place]}";
		}

		public static string Duration(DateTime past)
		{
			var span = DateTime.Now.Subtract(past);

			if (span.Days != 0)
				return $"{span.Days}d {span.Hours}h {span.Minutes}m {span.Seconds}s {span.Milliseconds}ms";
			else if (span.Hours != 0)
				return $"{span.Hours}h {span.Minutes}m {span.Seconds}s {span.Milliseconds}ms";
			else if (span.Minutes != 0)
				return $"{span.Minutes}m {span.Seconds}s {span.Milliseconds}ms";
			else
				return $"{span.Seconds}s {span.Milliseconds}ms";
		}

		internal static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, true);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}

		internal static string MimeType(string FileName)
		{
			var mime = new Dictionary<string, string>(MimeList, StringComparer.InvariantCultureIgnoreCase);
			var type = string.Empty;

			if (mime.TryGetValue(Path.GetExtension(FileName), out type))
			{
				return type;
			}

			return "application/octet-stream";
		}

		internal static bool IsValidPath(string FilePath)
		{
			try
			{
				Path.GetFileName(FilePath);

				if (!Path.IsPathRooted(FilePath))
					return false;
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		internal static string NewFilePath(string FilePath, string SaveDir)
		{
			// base
			var filename = Path.GetFileNameWithoutExtension(FilePath);
			var prefix = string.Empty;
			var postfix = string.Empty;

			// prefix
			if (Properties.Settings.Default.FileNamePrefixType == 1)
				prefix = $"[{DateTime.Now:yyyyMMdd_HHmmss}] ";
			else if (Properties.Settings.Default.FileNamePrefixType == 2)
				prefix = Properties.Settings.Default.FileNamePrefix;

			// postfix
			if (Properties.Settings.Default.FileNamePostfixType == 1)
				postfix = Properties.Settings.Default.FileNamePostfix;

			// use save folder
			filename = Path.Combine(SaveDir, $"{prefix}{filename}{postfix}");

			// check SaveDir is valid
			if (!IsValidPath(filename))
				filename = Path.Combine(Path.GetDirectoryName(FilePath), $"{prefix}{filename}{postfix}");
			
			// check if file already exist
			if (File.Exists(filename))
				filename = $"{filename} NEW";

			return filename;
		}
	}
}
