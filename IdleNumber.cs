using System;
using System.Collections.Generic;

namespace BigNumbers
{
	public enum Suffix
	{
		    None, Thousand, Million, Billion, Trillion,
        Quadrillion, Quintillion, Sextillion, Septillion,
        Octillion, Nonillion, Decillion, Undecillion,
        Duodecillion, Tredecillion
	}


	public class IdleNumber
	{
		#region Properties

		public string SmallText => GetSmallNumberText();
		public string FullText => GetFullNumberText();
    public bool Negative { get => _negativeValue; set => _negativeValue = value; }
		public static IdleNumber MaxValue => GetMaxValue();
    public static IdleNumber MinValue => GetMinValue();
		public static string Divider { set => _divider = value; }
		public static bool ShowSuffix { set => _showSuffix = value; }

    #endregion


    #region Private Values

    private static readonly int SuffixEnumSize = Enum.GetNames(typeof(Suffix)).Length;
		private Suffix _suffix;
		private int[] _bigNumber;
    private bool _negativeValue = false;
		private static string _divider = "";
		private static bool _showSuffix = true;

    #endregion


    #region Constructors

    public IdleNumber()
		{
			_suffix = 0;
      _bigNumber = new int[SuffixEnumSize];
		}

		public IdleNumber(long initValue)
		{
      if (initValue < 0)
      {
          _negativeValue = true;
          initValue *= -1;
      }
      _bigNumber = ConvertFromLongToArray(initValue, SuffixEnumSize);
			_suffix = (Suffix)CountActualSize(_bigNumber) - 1;
		}

    public IdleNumber(string initString)
    {
        if (initString[0].Equals('-'))
        {
            _negativeValue = true;
            initString = initString.Remove(0, 1);
        }
        _bigNumber = ConvertFromStringToArray(initString, SuffixEnumSize);
        _suffix = (Suffix)CountActualSize(_bigNumber) - 1;
    }

		public IdleNumber(int[] number)
    {
        _bigNumber = new int[SuffixEnumSize];
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i] < 0 || number[i] > 999)
                throw new Exception("IdleNumber: Init array has inappropriate values");
            _bigNumber[i] = number[i];
        }
			  _suffix = (Suffix)CountActualSize(number) - 1;
    }

    #endregion


        #region Operators

        #region +

        public static IdleNumber operator +(IdleNumber first, IdleNumber second)
        {
            if (!first.Negative && !second.Negative)
                return SumIdleNumbers(first, second);
            if (first.Negative && !second.Negative)
            {
                return SubIdleNumbers(second, IdleNumber.Abs(first));
            }
            if (!first.Negative && second.Negative)
            {
                return SubIdleNumbers(first, IdleNumber.Abs(second));
            }
            if(first.Negative && second.Negative)
            {
                var res = SumIdleNumbers(IdleNumber.Abs(first), IdleNumber.Abs(second));
                res.Negative = true;
                return res;
            }
            return new IdleNumber();
        }

        public static IdleNumber operator +(IdleNumber idleNumber, long val)
        {
            if (idleNumber is null)
                throw new Exception("IdleNumber: Null value in + operator");
            return new IdleNumber(val) + idleNumber;
        }

        public static IdleNumber operator +(long val, IdleNumber idleNumber)
        {
            if (idleNumber is null)
                throw new Exception("IdleNumber: Null value in + operator");
            return new IdleNumber(val) + idleNumber;
        }

        #endregion

        #region -

        public static IdleNumber operator -(IdleNumber first, IdleNumber second)
        {
            if (!first.Negative && !second.Negative)
                return SubIdleNumbers(first, second);
            if (first.Negative && !second.Negative)
            {
                var res = SumIdleNumbers(IdleNumber.Abs(first), second);
                res.Negative = true;
                return res;
            }
            if (!first.Negative && second.Negative)
            {
                return SumIdleNumbers(first, IdleNumber.Abs(second));
            }
            if (first.Negative && second.Negative)
            {
                return SubIdleNumbers(IdleNumber.Abs(second), IdleNumber.Abs(first));
            }
            return new IdleNumber();
        }

        public static IdleNumber operator -(IdleNumber first, long second)
        {
            return first - IdleNumber.ConvertFromLong(second);
        }

        public static IdleNumber operator -(long first, IdleNumber second)
        {
            return IdleNumber.ConvertFromLong(first) - second;
        }

        #endregion

        #region *

        public static IdleNumber operator *(IdleNumber first, int increaser)
        {
            if (first is null) throw new Exception("IdleNumber: Null value in * operator");
            if (increaser == 0) return new IdleNumber();
            bool shouldBeNegative = false;
            bool alreadyNegative = false;
            if (first.Negative) alreadyNegative = true;
            
            if (increaser < 0)
            {
                shouldBeNegative = true;
                increaser *= -1;
            }
            var a = first;
            for (int i = 1; i < increaser; i++) first += a;
            if(shouldBeNegative && !alreadyNegative || !shouldBeNegative && alreadyNegative) first.Negative = true;
            if (!shouldBeNegative && !alreadyNegative || shouldBeNegative && alreadyNegative) first.Negative = false;
            return first;
        }

        public static IdleNumber operator *(int increaser, IdleNumber first)
        {
            if (first is null) throw new Exception("IdleNumber: Null value in * operator");
            return first * increaser;
        }

        #endregion

        #region == != < > <= >=

        public static bool operator ==(IdleNumber first, IdleNumber second)
        {
            if (first is null || second is null)
                throw new Exception("IdleNumber: Null value in == operator");
            return first.IsEqual(second);
        }

        public static bool operator !=(IdleNumber first, IdleNumber second)
        {
            if (first is null || second is null)
                throw new Exception("IdleNumber: Null value in != operator");
            return !(first == second);
        }

        public static bool operator >(IdleNumber first, IdleNumber second)
        {
            if (first is null || second is null)
                throw new Exception("IdleNumber: Null value in > operator");
            return first.IsGreaterThen(second);
        }

        public static bool operator <(IdleNumber first, IdleNumber second)
        {
            if (first is null || second is null)
                throw new Exception("IdleNumber: Null value in < operator");
            return !first.IsGreaterThen(second);
        }

        public static bool operator >=(IdleNumber first, IdleNumber second)
        {
            if (first is null || second is null)
                throw new Exception("IdleNumber: Null value in >= operator");
            return first.IsGreaterThen(second) || first.IsEqual(second);
        }

        public static bool operator <=(IdleNumber first, IdleNumber second)
        {
            if (first is null || second is null)
                throw new Exception("IdleNumber: Null value in <= operator");
            return !first.IsGreaterThen(second) || first.IsEqual(second);
        }

        #endregion

        #endregion


        #region Public Static Methods

        public static IdleNumber ConvertFromLong(long val)
        {
            bool shouldBeNegative = false;
            if (val < 0)
            {
                shouldBeNegative = true;
                val *= -1;
            }
            var result = new IdleNumber(ConvertFromLongToArray(val, SuffixEnumSize));
            if (shouldBeNegative) result.Negative = true;
            return result;
        }

        public static IdleNumber Abs(IdleNumber val)
        {
            var res = new IdleNumber(val.GetNumArray());
            res.Negative = false;
            return res;
        }

        #endregion


        #region Private Static Methods

        private static IdleNumber SumIdleNumbers(IdleNumber first, IdleNumber second)
        {
            if (first is null || second is null)
                throw new Exception("IdleNumber: Null value in + operator");

            var firstArray = first.GetNumArray();
            var secondArray = second.GetNumArray();
            int[] sumArray = new int[firstArray.Length];
            int nextCellIncreaser = 0;
            for (int i = 0; i < sumArray.Length; i++)
            {
                int cellValue = firstArray[i] + secondArray[i] + nextCellIncreaser;
                if (cellValue >= 1000)
                {
                    nextCellIncreaser = 1;
                    cellValue %= 1000;
                }
                else nextCellIncreaser = 0;
                sumArray[i] = cellValue;
            }
            if (nextCellIncreaser != 0 && sumArray[sumArray.Length - 1] != 0)
                throw new Exception("IdleNumber: Numbers couldn't be summed, cause Sum is bigger then IdleNumber.MaxValue, please expand Enum");

            return new IdleNumber(sumArray);
        }

        private static IdleNumber SubIdleNumbers(IdleNumber first, IdleNumber second)
        {
            int[] firstArray;
            int[] secondArray;
            bool shouldBeNegative = false;

            if (first.IsGreaterThen(second) || first.IsEqual(second))
            {
                firstArray = first.GetNumArray();
                secondArray = second.GetNumArray();
            }
            else
            {
                secondArray = first.GetNumArray();
                firstArray = second.GetNumArray();
                shouldBeNegative = true;
            }
            var sumArray = new int[firstArray.Length];

            for (int i = 0; i < sumArray.Length; i++)
            {
                if (firstArray[i] < secondArray[i])
                {
                    firstArray[i] += 1000;
                    firstArray[i + 1]--;
                }
                sumArray[i] = firstArray[i] - secondArray[i];
            }

            var result = new IdleNumber(sumArray);
            if (shouldBeNegative) result.Negative = true;
            return result;
        }

        private static int[] ConvertFromLongToArray(long val, int arraySize)
        {
			int[] array = new int[arraySize];
			var currentIndex = 0;
			while (val > 0)
			{
				if (currentIndex > array.Length)
					throw new Exception("IdleNumber: Init value is bigger then Suffix Enum size, please expand Enum");
				array[currentIndex] = (int)(val % 1000);
				currentIndex++;
				val /= 1000;
			}
			
			return array;
		}

		private static int CountActualSize(int[] array)
		{
			int size = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 0) size = i + 1;
			}
			return size;
		}

		private static IdleNumber GetMaxValue()
        {
			var array = new int[Enum.GetNames(typeof(Suffix)).Length];
            for (int i = 0; i < array.Length; i++) array[i] = 999;
			return new IdleNumber(array);
        }

        private static IdleNumber GetMinValue()
        {
            var val = GetMaxValue();
            val.Negative = true;
            return val;
        }

        private static int[] ConvertFromStringToArray(string initValue, int arraySize)
        {
            int[] array = new int[arraySize];
            var index = 0;

            var stringLength = initValue.Count();
            var t = stringLength % 3;
            if (t == 1) initValue = initValue.Insert(0, "00");
            if (t == 2) initValue = initValue.Insert(0, "0");

            while (initValue.Count() >= 3)
            {
                array[index] = Convert.ToInt32(initValue.Substring(initValue.Count() - 3, 3));
                initValue = initValue.Remove(initValue.Count() - 3, 3);
                index++;
            }
            return array;
        }

        private static bool IsFirstGreaterThenSecondAbs(IdleNumber first, IdleNumber second)
        {
            var firstArray = first.GetNumArray();
            var secondArray = second.GetNumArray(); ;
            var firstSize = CountActualSize(firstArray);
            var secondSize = CountActualSize(secondArray);

            if (firstSize > secondSize) return true;
            else if (firstSize < secondSize) return false;
            else
            {
                for (int i = firstSize - 1; i >= 0; i--)
                {
                    if (firstArray[i] > secondArray[i]) return true;
                    if (firstArray[i] < secondArray[i]) return false;
                }
            }
            return false;
        }

        #endregion


        #region Public Methods

        public int[] GetNumArray()
        {
			return _bigNumber;
        }

		public void Assign(IdleNumber other)
        {
			_bigNumber = other.GetNumArray();
            _negativeValue = other.Negative;
        }

		public bool IsEqual(IdleNumber other)
        {
            if (_negativeValue != other.Negative) return false;
			var otherArray = other.GetNumArray();
            for (int i = 0; i < _bigNumber.Length; i++)
            {
				if (_bigNumber[i] != otherArray[i]) return false;
            }
			return true;
        }

        public bool IsGreaterThen(IdleNumber other)
        {
            if(!_negativeValue && !other.Negative)
            {
                return IsFirstGreaterThenSecondAbs(this, other);
            }
            if (_negativeValue && !other.Negative) return false;
            if (!_negativeValue && other.Negative) return true;
            if (_negativeValue && other.Negative)
            {
                return !IsFirstGreaterThenSecondAbs(this, other);
            }
            return false;
        }

        public long ConvertToLong()
        {
            long result = _bigNumber[0];
            int increaser = 1000;
            int size = CountActualSize(_bigNumber);
            if (size > 7)
                throw new Exception("IdleNumber: Can not convert to long, not enough memory");
            for (int i = 1; i < size; i++)
            {
                result += _bigNumber[i] * increaser;
                increaser *= 1000;
            }
            if (_negativeValue) result *= -1;
            return result;
        }

		#endregion


		#region Private Methods

		private string GetFullNumberText()
		{
			string result = "";
            if (_negativeValue) result += "-";
			int size = CountActualSize(_bigNumber);
            if (size == 0) return "0";
			for (int i = size - 1; i >= 0; i--)
			{
				string num = "";
				if (_bigNumber[i] < 100 && _bigNumber[i] > 10 && i != size - 1) num += "0" + _bigNumber[i].ToString();
				else if (_bigNumber[i] < 10 && i != size - 1) num += "00" + _bigNumber[i].ToString();
				else num += _bigNumber[i].ToString();
				result += num;
				if (i != 0) result += _divider;
			}
			if (_showSuffix) return result + " " + _suffix.ToString();
			else return result;

		}

		private string GetSmallNumberText()
        {
			string result = "";
            if (_negativeValue) result += "-";
            int index = CountActualSize(_bigNumber) - 1;
			string num = "";
			if (index != 0)
			{
                if (_bigNumber[index - 1] < 100) num += "0" + _bigNumber[index - 1].ToString();
                else if (_bigNumber[index - 1] < 10) num += "00" + _bigNumber[index - 1].ToString();
                else num += _bigNumber[index - 1].ToString();
                result += _bigNumber[index].ToString() + _divider + num;
			}
			else result += _bigNumber[index].ToString();
            if (_showSuffix) return result + " " + _suffix.ToString();
            else return result;
        }

        #endregion


        #region Override Methods

        public override string ToString()
        {
			return FullText;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            var code = 0;
            for (int i = 0; i < CountActualSize(_bigNumber); i++)
            {
                code += _bigNumber[i].GetHashCode();
            }
            return code;
        }

        #endregion
    }
}

