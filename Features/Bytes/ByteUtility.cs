﻿using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Framework
{
	public class ByteUtility
	{
		public const int DefaultBufferCapacity = 128;

		public class Writer
		{
			private byte[] _buffer;
			public byte[] Buffer { get { return _buffer; } }
			public int tracker;

			public Writer()
			{
				Refresh();
			}

			public Writer(int capacity)
			{
				Refresh(capacity);
			}

			public void Refresh()
			{
				Refresh(DefaultBufferCapacity);
			}

			public void Refresh(int capacity)
			{
				_buffer = new byte[capacity];
				tracker = 0;
			}

			public void SetTracker(int index)
			{
				tracker = index;
			}

			public unsafe byte[] CorrectBuffer()
			{
				byte[] otherBuffer = new byte[tracker];

				fixed (void* othPtr = &otherBuffer[0])
				{
					fixed (void* ptr = &_buffer[0])
					{
						UnsafeUtility.MemCpy(othPtr, ptr, tracker);
					}
				}

				return otherBuffer;
			}

			unsafe void EnsureSize(int add, int _index = -1)
			{
				int index;
				if (_index <= -1)
					index = tracker;
				else
					index = _index;

				if (index + add + 1 < _buffer.Length)
					return;

				var newSize = _buffer.Length + add;

				var tempArr = _buffer;
				_buffer = new byte[newSize];

				fixed (void* tmpPtr = &tempArr[0])
				{
					fixed (void* ptr = &_buffer[0])
					{
						UnsafeUtility.MemCpy(ptr, tmpPtr, tempArr.Length);
					}
				}
			}

			//Add

			public void Add(byte _byte, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(1, index);

				_buffer[index] = _byte;

				if (_index <= -1)
					tracker += 1;
			}

			public void Add(bool _bool, int _index = -1)
			{
				Add((byte)(_bool ? 1 : 0), _index);
			}

			public unsafe void Add(short _short, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(2, index);

				fixed (void* ptr = &_buffer[index])
					*((short*)ptr) = _short;

				if (_index <= -1)
					tracker += 2;
			}

			public unsafe void Add(ushort _ushort, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(2, index);

				fixed (void* ptr = &_buffer[index])
					*((ushort*)ptr) = _ushort;

				if (_index <= -1)
					tracker += 2;
			}

			public unsafe void Add(int _int, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(4, index);

				fixed (void* ptr = &_buffer[index])
					*((int*)ptr) = _int;

				if (_index <= -1)
					tracker += 4;
			}

			public unsafe void Add(uint _uint, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(4, index);

				fixed (void* ptr = &_buffer[index])
					*((uint*)ptr) = _uint;

				if (_index <= -1)
					tracker += 4;
			}

			public unsafe void Add(float _float, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(4, index);

				fixed (void* ptr = &_buffer[index])
					*((int*)ptr) = *(int*)&_float;

				if (_index <= -1)
					tracker += 4;
			}

			public unsafe void Add(string _string, int _index = -1)
			{
				var length = _string.Length;

				int index = _index <= -1 ? tracker : _index;

				EnsureSize(4 + length * 2, index);

				fixed (void* ptr = &_buffer[index])
					*((int*)ptr) = length;

				for (int i = 0; i < length; i++)
				{
					fixed (void* ptr = &_buffer[index + 4 + (i * 2)])
						*((short*)ptr) = (short)_string[i];
				}

				if (_index <= -1)
					tracker += 4 + length * 2;
			}

			public unsafe void Add(byte[] _arr, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(_arr.Length, index);

				fixed (void* tmpPtr = &_arr[0])
				{
					fixed (void* ptr = &_buffer[index])
					{
						UnsafeUtility.MemCpy(ptr, tmpPtr, _arr.Length);
					}
				}

				if (_index <= -1)
					tracker += _arr.Length;
			}

			public unsafe void Add<T>(T _item, int _index = -1) where T : IByteUtilizer
			{
				if (_item == null)
					return;

				var byteSize = _item.GetByteCount();

				int index = _index <= -1 ? tracker : _index;

				EnsureSize(4 + byteSize, index);

				fixed (void* ptr = &_buffer[index])
					*((int*)ptr) = byteSize;

				var _this = this;

				_item.Write(ref _this, index + 4);

				if (_index <= -1)
					tracker += 4 + byteSize;
			}

			public unsafe void Add<T>(T[] _array, int _index = -1) where T : IByteUtilizer
			{
				if (_array == null || _array.Length == 0)
					return;

				var byteSize = _array[0].GetByteCount();

				int index = _index <= -1 ? tracker : _index;

				EnsureSize(4 + 4 + (byteSize * _array.Length), index);

				fixed (void* ptr = &_buffer[tracker])
					*((int*)ptr) = _array.Length;

				fixed (void* ptr = &_buffer[tracker + 4])
					*((int*)ptr) = byteSize;

				var _this = this;

				for (int i = 0; i < _array.Length; i++)
				{
					_array[i].Write(ref _this, tracker + 8 + (byteSize * i));
				}

				if (_index <= -1)
					tracker += 4 + 4 + (byteSize * _array.Length);
			}

			public unsafe void AddUniqueArray<T>(T[] _array, int _index = -1) where T : IByteUtilizer
			{
				if (_array == null || _array.Length == 0)
					return;

				var arrayByteSize = _array.GetByteSize();

				int index = _index <= -1 ? tracker : _index;

				EnsureSize(arrayByteSize, index);

				fixed (void* ptr = &_buffer[index])
					*((int*)ptr) = _array.Length;

				fixed (void* ptr = &_buffer[index + 4])
					*((int*)ptr) = arrayByteSize + (_array.Length * 4);

				var _this = this;
				var prevIndex = 0;

				for (int i = 0; i < _array.Length; i++)
				{
					var item = _array[i];
					var size = item.GetByteCount();

					fixed (void* ptr = &_buffer[index + 8 + prevIndex])
						*((int*)ptr) = size;

					item.Write(ref _this, index + 8 + prevIndex + 4);

					prevIndex += size + 4;
				}

				if (_index <= -1)
					tracker += 4 + arrayByteSize + (_array.Length * 4);
			}

			public unsafe void Add(Vector2 _vector2, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(8, index);

				fixed (void* ptr = &_buffer[index])
					*((int*)ptr) = *(int*)&_vector2.x;

				fixed (void* ptr = &_buffer[index + 4])
					*((int*)ptr) = *(int*)&_vector2.y;

				if (_index <= -1)
					tracker += 8;
			}

			public unsafe void Add(Vector3 _vector3, int _index = -1)
			{
				int index = _index <= -1 ? tracker : _index;

				EnsureSize(12, index);

				fixed (void* ptr = &_buffer[index])
					*((int*)ptr) = *(int*)&_vector3.x;

				fixed (void* ptr = &_buffer[index + 4])
					*((int*)ptr) = *(int*)&_vector3.y;

				fixed (void* ptr = &_buffer[index + 8])
					*((int*)ptr) = *(int*)&_vector3.z;

				if (_index <= -1)
					tracker += 12;
			}
		}

		public class Reader
		{
			private byte[] _buffer;
			public byte[] Buffer { get { return _buffer; } }
			int tracker;

			public unsafe Reader(byte[] newBuffer)
			{
				Refresh(newBuffer);
			}

			public void Refresh(byte[] newBuffer)
			{
				_buffer = newBuffer;
				tracker = 0;
			}

			public void SetTracker(int index)
			{
				tracker = index;
			}

			unsafe bool TooMuch(int add, int index = -1)
			{
				if (index == -1)
					index = tracker;
				return index + add + 1 >= _buffer.Length;
			}

			public byte ReadByte(int index)
			{
				if (TooMuch(1))
					return default;

				return _buffer[index];
			}

			public byte ReadNextByte()
			{
				var value = ReadByte(tracker);
				tracker += 1;

				return value;
			}

			public bool ReadBool(int index)
			{
				var _byte = ReadByte(index);

				return _byte == 1;
			}

			public bool ReadNextBool()
			{
				var _byte = ReadNextByte();

				return _byte == 1;
			}

			public unsafe short ReadShort(int index)
			{
				if (TooMuch(2, index))
					return default;

				short value;

				fixed (byte* ptr = &_buffer[index])
				{
					value = *((short*)ptr);
				}

				return value;
			}

			public unsafe short ReadNextShort()
			{
				var value = ReadShort(tracker);

				tracker += 2;
				return value;
			}

			public unsafe ushort ReadUShort(int index)
			{
				if (TooMuch(2, index))
					return default;

				ushort value;

				fixed (byte* ptr = &_buffer[index])
				{
					value = *((ushort*)ptr);
				}

				return value;
			}

			public unsafe ushort ReadNextUShort()
			{
				var value = ReadUShort(tracker);

				tracker += 2;
				return value;
			}

			public unsafe int ReadInt(int index)
			{
				if (TooMuch(4, index))
					return default;

				int value;

				fixed (byte* ptr = &_buffer[index])
				{
					value = *((int*)ptr);
				}

				return value;
			}

			public unsafe int ReadNextInt()
			{
				var value = ReadInt(tracker);

				tracker += 4;
				return value;
			}

			public unsafe uint ReadUInt(int index)
			{
				if (TooMuch(4, index))
					return default;

				uint value;

				fixed (byte* ptr = &_buffer[index])
				{
					value = *((uint*)ptr);
				}

				return value;
			}

			public unsafe uint ReadNextUInt()
			{
				var value = ReadUInt(tracker);

				tracker += 4;
				return value;
			}

			public unsafe float ReadFloat(int index)
			{
				if (TooMuch(4, index))
					return default;

				float value;

				fixed (byte* ptr = &_buffer[index])
				{
					value = *(float*)&(*((int*)ptr));
				}

				return value;
			}

			public unsafe float ReadNextFloat()
			{
				var value = ReadFloat(tracker);

				tracker += 4;
				return value;
			}

			public unsafe string ReadString(int index)
			{
				if (TooMuch(4, index))
					return default;

				int length;

				fixed (byte* ptr = &_buffer[index])
					length = *((int*)ptr);

				if (TooMuch(4 + length * 2))
					return default;

				var sb = new StringBuilder(length);

				for (int i = 0; i < length; i++)
				{
					fixed (byte* ptr = &_buffer[index + 4 + (i * 2)])
					{
						var _short = *((short*)ptr);
						sb.Append((char)_short);
					}
				}

				return sb.ToString();
			}

			public unsafe string ReadNextString()
			{
				var value = ReadString(tracker);

				tracker += 4 + value.Length * 2;
				return value;
			}

			public unsafe T ReadItem<T>(int index, out int byteSize) where T : IByteUtilizer, new()
			{
				byteSize = 0;

				if (TooMuch(4))
					return default;

				fixed (byte* ptr = &_buffer[tracker])
					byteSize = *((int*)ptr);

				if (TooMuch(byteSize, tracker + 4))
					return default;

				var _this = this;
				T _item = new T();
				_item.Read(ref _this, tracker + 4);

				return _item;
			}

			public unsafe T ReadNextItem<T>() where T : IByteUtilizer, new()
			{
				T _item = ReadItem<T>(tracker, out var byteSize);

				tracker += 4 + byteSize;
				return _item;
			}

			public unsafe void ReadItemMB<T>(T item, int index, out int byteSize) where T : IByteUtilizerMB, new()
			{
				byteSize = 0;

				if (TooMuch(4))
					return;

				fixed (byte* ptr = &_buffer[tracker])
					byteSize = *((int*)ptr);

				if (TooMuch(byteSize, tracker + 4))
					return;

				var _this = this;
				item.Read(ref _this, tracker + 4);
			}

			public unsafe T[] ReadArray<T>(int index, out int byteSize) where T : IByteUtilizer, new()
			{
				byteSize = 0;

				if (TooMuch(8))
					return null;

				int length;

				fixed (byte* ptr = &_buffer[index])
					length = *((int*)ptr);

				fixed (byte* ptr = &_buffer[index + 4])
					byteSize = *((int*)ptr);

				if (TooMuch(length * byteSize, index + 8))
					return null;

				T[] _arr = new T[length];
				var _this = this;

				for (int i = 0; i < length; i++)
				{
					_arr[i] = new T();
					_arr[i].Read(ref _this, index + 8 + (i * byteSize));
				}

				return _arr;
			}

			public unsafe T[] ReadNextArray<T>() where T : IByteUtilizer, new()
			{
				T[] _arr = ReadArray<T>(tracker, out var byteSize);

				if (_arr != null)
					tracker += 8 + (_arr.Length * byteSize);

				return _arr;
			}

			public unsafe T[] ReadUniqueArray<T>(int index, out int byteSize) where T: IByteUtilizer, new()
			{
				byteSize = 0;
				
				if (TooMuch(8))
					return null;

				int length;

				fixed (byte* ptr = &_buffer[index])
					length = *((int*)ptr);

				fixed (byte* ptr = &_buffer[index + 4])
					byteSize = *((int*)ptr);

				if (TooMuch(byteSize, index + 8))
					return null;

				T[] _arr = new T[length];
				var _this = this;
				var prevIndex = 0;

				for (int i = 0; i < length; i++)
				{
					var size = 0;
					fixed (byte* ptr = &_buffer[index + 8 + prevIndex])
						size = *((int*)ptr);

					_arr[i] = new T();
					_arr[i].Read(ref _this, index + 8 + prevIndex + 4);

					prevIndex += size + 4;
				}

				return _arr;
			}

			public unsafe void ReadUniqueArrayMB<T>(T[] items, int index) where T : IByteUtilizerMB, new()
			{
				if (TooMuch(8))
					return;

				int length;
				int totalByteSize;

				fixed (byte* ptr = &_buffer[index])
					length = *((int*)ptr);

				fixed (byte* ptr = &_buffer[index + 4])
					totalByteSize = *((int*)ptr);

				if (TooMuch(totalByteSize, index + 8))
					return;

				var _this = this;
				var prevIndex = 0;

				for (int i = 0; i < length; i++)
				{
					var size = 0;
					fixed (byte* ptr = &_buffer[index + 8 + prevIndex])
						size = *((int*)ptr);

					items[i].Read(ref _this, index + 8 + prevIndex + 4);

					prevIndex += size + 4;
				}
			}

			public unsafe Vector2 ReadVector2(int index)
			{
				if (TooMuch(8, index))
					return default;

				Vector2 value = new Vector2();

				fixed (byte* ptr = &_buffer[index])
					value.x = *(float*)&(*((int*)ptr));

				fixed (byte* ptr = &_buffer[index + 4])
					value.y = *(float*)&(*((int*)ptr));

				return value;
			}

			public unsafe Vector2 ReadNextVector2()
			{
				Vector2 value = ReadVector2(tracker);

				tracker += 8;
				return value;
			}

			public unsafe Vector3 ReadVector3(int index)
			{
				if (TooMuch(12, index))
					return default;

				Vector3 value = new Vector3();

				fixed (byte* ptr = &_buffer[index])
					value.x = *(float*)&(*((int*)ptr));

				fixed (byte* ptr = &_buffer[index + 4])
					value.y = *(float*)&(*((int*)ptr));

				fixed (byte* ptr = &_buffer[index + 8])
					value.z = *(float*)&(*((int*)ptr));

				return value;
			}

			public unsafe Vector3 ReadNextVector3()
			{
				Vector3 value = ReadVector3(tracker);

				tracker += 12;
				return value;
			}
		}
	}

	public interface IByteUtilizer
	{
		int GetByteCount();
		int Write(ref ByteUtility.Writer writer, int index);
		int Read(ref ByteUtility.Reader reader, int index);
	}

	public interface IByteUtilizerMB : IByteUtilizer
	{
		
	}
}
