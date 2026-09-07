using System;
using System.Text;
using S7.Net;

namespace PlcSimClient
{
    public enum PlcDataType
    {
        Bool, Byte, Word, DWord, Int, DInt, Real, String
    }

    public class PlcManager
    {
        private readonly PlcConfig _config;
        private Plc _plc;

        public bool IsConnected
        {
            get { return _plc != null && _plc.IsConnected; }
        }

        public PlcManager(PlcConfig config)
        {
            _config = config;
        }

        public bool Connect()
        {
            try
            {
                Logger.Info("Попытка подключения: " + _config);

                _plc = new Plc(_config.CpuType, _config.IpAddress, _config.Rack, _config.Slot);
                _plc.Open();

                if (_plc.IsConnected)
                {
                    Logger.Info("Подключение успешно установлено.");
                    return true;
                }

                Logger.Error("Подключение не установлено.");
                return false;
            }
            catch (PlcException plcEx)
            {
                Logger.Error("Ошибка протокола S7: " + plcEx.Message);
                _plc = null;
                return false;
            }
            catch (System.Net.Sockets.SocketException sockEx)
            {
                Logger.Error("Сетевая ошибка: " + sockEx.Message);
                _plc = null;
                return false;
            }
            catch (TimeoutException timeoutEx)
            {
                Logger.Error("Таймаут подключения: " + timeoutEx.Message);
                _plc = null;
                return false;
            }
            catch (Exception ex)
            {
                // Общий catch (Exception) должен идти ПОСЛЕДНИМ
                Logger.Error("Непредвиденная ошибка: " + ex.Message);
                _plc = null;
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_plc != null)
                {
                    _plc.Close();
                    Logger.Info("Соединение с PLC закрыто.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при закрытии: " + ex.Message);
            }
            finally
            {
                _plc = null;
            }
        }

        private void EnsureConnected()
        {
            if (_plc == null || !_plc.IsConnected)
                throw new InvalidOperationException("Соединение с PLC не установлено. Сначала выполните подключение.");
        }

        // Читает значение из DB по номеру, смещению и типу данных
        public object ReadValue(int dbNumber, int byteOffset, PlcDataType dataType, int bitNumber = 0, int stringMaxLength = 254)
        {
            EnsureConnected();

            try
            {
                object result;

                switch (dataType)
                {
                    case PlcDataType.Bool:
                        {
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 1);
                            result = (bytes[0] & (1 << bitNumber)) != 0;
                            break;
                        }
                    case PlcDataType.Byte:
                        {
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 1);
                            result = bytes[0];
                            break;
                        }
                    case PlcDataType.Word:
                        {
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 2);
                            result = (ushort)((bytes[0] << 8) | bytes[1]);
                            break;
                        }
                    case PlcDataType.DWord:
                        {
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 4);
                            result = ((uint)bytes[0] << 24) | ((uint)bytes[1] << 16) | ((uint)bytes[2] << 8) | bytes[3];
                            break;
                        }
                    case PlcDataType.Int:
                        {
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 2);
                            result = (short)((bytes[0] << 8) | bytes[1]);
                            break;
                        }
                    case PlcDataType.DInt:
                        {
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 4);
                            result = (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
                            break;
                        }
                    case PlcDataType.Real:
                        {
                            // S7 хранит числа в Big-Endian, поэтому байты разворачиваем
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 4);
                            byte[] reversed = new byte[] { bytes[3], bytes[2], bytes[1], bytes[0] };
                            result = BitConverter.ToSingle(reversed, 0);
                            break;
                        }
                    case PlcDataType.String:
                        {
                            // Строка S7: [0]=макс.длина, [1]=факт.длина, далее символы
                            byte[] bytes = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, stringMaxLength + 2);
                            byte actualLength = bytes[1];
                            int lengthToRead = Math.Min(actualLength, bytes.Length - 2);
                            result = Encoding.ASCII.GetString(bytes, 2, Math.Max(0, lengthToRead));
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException("dataType");
                }

                Logger.Info("Чтение DB" + dbNumber + ", смещение " + byteOffset + ", тип " + dataType + " -> " + result);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка чтения DB" + dbNumber + ", смещение " + byteOffset + ": " + ex.Message);
                throw;
            }
        }

        // Записывает значение в DB по номеру, смещению и типу данных
        public void WriteValue(int dbNumber, int byteOffset, PlcDataType dataType, string rawValue, int bitNumber = 0, int stringMaxLength = 254)
        {
            EnsureConnected();

            try
            {
                switch (dataType)
                {
                    case PlcDataType.Bool:
                        {
                            bool value = ParseBool(rawValue);
                            byte[] current = _plc.ReadBytes(DataType.DataBlock, dbNumber, byteOffset, 1);
                            byte newByte = value
                                ? (byte)(current[0] | (1 << bitNumber))
                                : (byte)(current[0] & ~(1 << bitNumber));
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, new byte[] { newByte });
                            break;
                        }
                    case PlcDataType.Byte:
                        {
                            byte value = byte.Parse(rawValue);
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, new byte[] { value });
                            break;
                        }
                    case PlcDataType.Word:
                        {
                            ushort value = ushort.Parse(rawValue);
                            byte[] bytes = new byte[] { (byte)(value >> 8), (byte)(value & 0xFF) };
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, bytes);
                            break;
                        }
                    case PlcDataType.DWord:
                        {
                            uint value = uint.Parse(rawValue);
                            byte[] bytes = new byte[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)(value & 0xFF) };
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, bytes);
                            break;
                        }
                    case PlcDataType.Int:
                        {
                            short value = short.Parse(rawValue);
                            byte[] bytes = new byte[] { (byte)(value >> 8), (byte)(value & 0xFF) };
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, bytes);
                            break;
                        }
                    case PlcDataType.DInt:
                        {
                            int value = int.Parse(rawValue);
                            byte[] bytes = new byte[] { (byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)(value & 0xFF) };
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, bytes);
                            break;
                        }
                    case PlcDataType.Real:
                        {
                            float value = float.Parse(rawValue, System.Globalization.CultureInfo.InvariantCulture);
                            byte[] bytes = BitConverter.GetBytes(value);
                            byte[] reversed = new byte[] { bytes[3], bytes[2], bytes[1], bytes[0] };
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, reversed);
                            break;
                        }
                    case PlcDataType.String:
                        {
                            if (rawValue.Length > stringMaxLength)
                                throw new ArgumentException("Длина строки превышает " + stringMaxLength);

                            byte[] textBytes = Encoding.ASCII.GetBytes(rawValue);
                            byte[] fullBytes = new byte[stringMaxLength + 2];
                            fullBytes[0] = (byte)stringMaxLength;
                            fullBytes[1] = (byte)textBytes.Length;
                            Array.Copy(textBytes, 0, fullBytes, 2, textBytes.Length);
                            _plc.WriteBytes(DataType.DataBlock, dbNumber, byteOffset, fullBytes);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException("dataType");
                }

                Logger.Info("Запись DB" + dbNumber + ", смещение " + byteOffset + ", тип " + dataType + " <- " + rawValue);
            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка записи DB" + dbNumber + ", смещение " + byteOffset + ": " + ex.Message);
                throw;
            }
        }

        private static bool ParseBool(string raw)
        {
            string normalized = raw.Trim().ToLowerInvariant();
            return normalized == "1" || normalized == "true" || normalized == "да" || normalized == "yes";
        }
    }
}