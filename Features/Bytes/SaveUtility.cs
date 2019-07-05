using System.IO;
using UnityEngine;

namespace Framework
{
	public class SaveUtility
	{
		public static void Save(IByteUtilizer saveItem, string folderName, string fileName, bool overwrite = true)
		{
			string prefix = Application.persistentDataPath + "/";
			string folderPath = prefix + folderName;
			string filePath = folderPath + "/" + fileName;

			if (!overwrite && File.Exists(filePath))
			{
				Debug.LogError("[overwrite: false] File exists! Did not attempt to overwrite");
				return;
			}

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			var writer = new ByteUtility.Writer();
			writer.Add(saveItem);
			var bytes = writer.Buffer;

			var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			stream.Write(bytes, 0, bytes.Length);
			stream.Dispose();
			stream.Close();
		}

		public static bool Load<T>(ref T item, string folderName, string fileName) where T : IByteUtilizer, new()
		{
			string prefix = Application.persistentDataPath + "/";
			string folderPath = prefix + folderName;
			string filePath = folderPath + "/" + fileName;

			if (!File.Exists(filePath))
			{
				Debug.LogError($"File at [{filePath}] does not exist!");
				return false;
			}

			var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			var length = (int)stream.Length;
			byte[] bytes = new byte[length];
			stream.Read(bytes, 0, length);

			var reader = new ByteUtility.Reader(bytes);
			item = reader.ReadNextItem<T>();
			return true;
		}
	}
}
