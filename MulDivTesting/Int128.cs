using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;


namespace MulDivTesting
{
    // From https://github.com/ricksladkey/dirichlet-numerics
    public struct Int128 : IFormattable, IComparable, IComparable<Int128>, IEquatable<Int128>
    {
        private UInt128 v;

        private static readonly Int128 minValue = (Int128)((UInt128)1 << 127);
        private static readonly Int128 maxValue = (Int128)(((UInt128)1 << 127) - 1);
        private static readonly Int128 zero = (Int128)0;
        private static readonly Int128 one = (Int128)1;
        private static readonly Int128 minusOne = (Int128)(-1);

        public static Int128 MinValue { get { return minValue; } }
        public static Int128 MaxValue { get { return maxValue; } }
        public static Int128 Zero { get { return zero; } }
        public static Int128 One { get { return one; } }
        public static Int128 MinusOne { get { return minusOne; } }

        public static Int128 Parse(string value)
        {
            Int128 c;
            if (!TryParse(value, out c))
                throw new FormatException();
            return c;
        }

        public static bool TryParse(string value, out Int128 result)
        {
            return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string value, NumberStyles style, IFormatProvider format, out Int128 result)
        {
            BigInteger a;
            if (!BigInteger.TryParse(value, style, format, out a))
            {
                result = Int128.Zero;
                return false;
            }
            UInt128.Create(out result.v, a);
            return true;
        }

        public Int128(Int64 value)
        {
            UInt128.Create(out v, value);
        }

        public Int128(UInt64 value)
        {
            UInt128.Create(out v, value);
        }

        public Int128(double value)
        {
            UInt128.Create(out v, value);
        }

        public Int128(decimal value)
        {
            UInt128.Create(out v, value);
        }

        public Int128(BigInteger value)
        {
            UInt128.Create(out v, value);
        }

        public UInt64 S0 { get { return v.S0; } }
        public UInt64 S1 { get { return v.S1; } }

        public bool IsZero { get { return v.IsZero; } }
        public bool IsOne { get { return v.IsOne; } }
        public bool IsPowerOfTwo { get { return v.IsPowerOfTwo; } }
        public bool IsEven { get { return v.IsEven; } }
        public bool IsNegative { get { return v.S1 > Int64.MaxValue; } }
        public Int32 Sign { get { return IsNegative ? -1 : v.Sign; } }

        public override string ToString()
        {
            return ((BigInteger)this).ToString();
        }

        public string ToString(string format)
        {
            return ((BigInteger)this).ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return ((BigInteger)this).ToString(format, provider);
        }

        public static explicit operator Int128(double a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static implicit operator Int128(sbyte a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static implicit operator Int128(byte a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static implicit operator Int128(short a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static implicit operator Int128(ushort a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static implicit operator Int128(Int32 a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static implicit operator Int128(UInt32 a)
        {
            Int128 c;
            UInt128.Create(out c.v, (UInt64)a);
            return c;
        }

        public static implicit operator Int128(Int64 a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static implicit operator Int128(UInt64 a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static explicit operator Int128(decimal a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static explicit operator Int128(UInt128 a)
        {
            Int128 c;
            c.v = a;
            return c;
        }

        public static explicit operator UInt128(Int128 a)
        {
            return a.v;
        }

        public static explicit operator Int128(BigInteger a)
        {
            Int128 c;
            UInt128.Create(out c.v, a);
            return c;
        }

        public static explicit operator sbyte(Int128 a)
        {
            return (sbyte)a.v.S0;
        }

        public static explicit operator byte(Int128 a)
        {
            return (byte)a.v.S0;
        }

        public static explicit operator short(Int128 a)
        {
            return (short)a.v.S0;
        }

        public static explicit operator ushort(Int128 a)
        {
            return (ushort)a.v.S0;
        }

        public static explicit operator Int32(Int128 a)
        {
            return (Int32)a.v.S0;
        }

        public static explicit operator UInt32(Int128 a)
        {
            return (UInt32)a.v.S0;
        }

        public static explicit operator Int64(Int128 a)
        {
            return (Int64)a.v.S0;
        }

        public static explicit operator UInt64(Int128 a)
        {
            return a.v.S0;
        }

        public static explicit operator decimal(Int128 a)
        {
            if (a.IsNegative)
            {
                UInt128 c;
                UInt128.Negate(out c, ref a.v);
                return -(decimal)c;
            }
            return (decimal)a.v;
        }

        public static implicit operator BigInteger(Int128 a)
        {
            if (a.IsNegative)
            {
                UInt128 c;
                UInt128.Negate(out c, ref a.v);
                return -(BigInteger)c;
            }
            return (BigInteger)a.v;
        }

        public static explicit operator float(Int128 a)
        {
            if (a.IsNegative)
            {
                UInt128 c;
                UInt128.Negate(out c, ref a.v);
                return -UInt128.ConvertToFloat(ref c);
            }
            return UInt128.ConvertToFloat(ref a.v);
        }

        public static explicit operator double(Int128 a)
        {
            if (a.IsNegative)
            {
                UInt128 c;
                UInt128.Negate(out c, ref a.v);
                return -UInt128.ConvertToDouble(ref c);
            }
            return UInt128.ConvertToDouble(ref a.v);
        }

        public static Int128 operator <<(Int128 a, Int32 b)
        {
            Int128 c;
            UInt128.LeftShift(out c.v, ref a.v, b);
            return c;
        }

        public static Int128 operator >>(Int128 a, Int32 b)
        {
            Int128 c;
            UInt128.ArithmeticRightShift(out c.v, ref a.v, b);
            return c;
        }

        public static Int128 operator &(Int128 a, Int128 b)
        {
            Int128 c;
            UInt128.And(out c.v, ref a.v, ref b.v);
            return c;
        }

        public static Int32 operator &(Int128 a, Int32 b)
        {
            return (Int32)(a.v & (UInt32)b);
        }

        public static Int32 operator &(Int32 a, Int128 b)
        {
            return (Int32)(b.v & (UInt32)a);
        }

        public static Int64 operator &(Int128 a, Int64 b)
        {
            return (Int64)(a.v & (UInt64)b);
        }

        public static Int64 operator &(Int64 a, Int128 b)
        {
            return (Int64)(b.v & (UInt64)a);
        }

        public static Int128 operator |(Int128 a, Int128 b)
        {
            Int128 c;
            UInt128.Or(out c.v, ref a.v, ref b.v);
            return c;
        }

        public static Int128 operator ^(Int128 a, Int128 b)
        {
            Int128 c;
            UInt128.ExclusiveOr(out c.v, ref a.v, ref b.v);
            return c;
        }

        public static Int128 operator ~(Int128 a)
        {
            Int128 c;
            UInt128.Not(out c.v, ref a.v);
            return c;
        }

        public static Int128 operator +(Int128 a, Int64 b)
        {
            Int128 c;
            if (b < 0)
                UInt128.Subtract(out c.v, ref a.v, (UInt64)(-b));
            else
                UInt128.Add(out c.v, ref a.v, (UInt64)b);
            return c;
        }

        public static Int128 operator +(Int64 a, Int128 b)
        {
            Int128 c;
            if (a < 0)
                UInt128.Subtract(out c.v, ref b.v, (UInt64)(-a));
            else
                UInt128.Add(out c.v, ref b.v, (UInt64)a);
            return c;
        }

        public static Int128 operator +(Int128 a, Int128 b)
        {
            Int128 c;
            UInt128.Add(out c.v, ref a.v, ref b.v);
            return c;
        }

        public static Int128 operator ++(Int128 a)
        {
            Int128 c;
            UInt128.Add(out c.v, ref a.v, 1);
            return c;
        }

        public static Int128 operator -(Int128 a, Int64 b)
        {
            Int128 c;
            if (b < 0)
                UInt128.Add(out c.v, ref a.v, (UInt64)(-b));
            else
                UInt128.Subtract(out c.v, ref a.v, (UInt64)b);
            return c;
        }

        public static Int128 operator -(Int128 a, Int128 b)
        {
            Int128 c;
            UInt128.Subtract(out c.v, ref a.v, ref b.v);
            return c;
        }

        public static Int128 operator +(Int128 a)
        {
            return a;
        }

        public static Int128 operator -(Int128 a)
        {
            Int128 c;
            UInt128.Negate(out c.v, ref a.v);
            return c;
        }

        public static Int128 operator --(Int128 a)
        {
            Int128 c;
            UInt128.Subtract(out c.v, ref a.v, 1);
            return c;
        }

        public static Int128 operator *(Int128 a, Int32 b)
        {
            Int128 c;
            Multiply(out c, ref a, b);
            return c;
        }

        public static Int128 operator *(Int32 a, Int128 b)
        {
            Int128 c;
            Multiply(out c, ref b, a);
            return c;
        }

        public static Int128 operator *(Int128 a, UInt32 b)
        {
            Int128 c;
            Multiply(out c, ref a, b);
            return c;
        }

        public static Int128 operator *(UInt32 a, Int128 b)
        {
            Int128 c;
            Multiply(out c, ref b, a);
            return c;
        }

        public static Int128 operator *(Int128 a, Int64 b)
        {
            Int128 c;
            Multiply(out c, ref a, b);
            return c;
        }

        public static Int128 operator *(Int64 a, Int128 b)
        {
            Int128 c;
            Multiply(out c, ref b, a);
            return c;
        }

        public static Int128 operator *(Int128 a, UInt64 b)
        {
            Int128 c;
            Multiply(out c, ref a, b);
            return c;
        }

        public static Int128 operator *(UInt64 a, Int128 b)
        {
            Int128 c;
            Multiply(out c, ref b, a);
            return c;
        }

        public static Int128 operator *(Int128 a, Int128 b)
        {
            Int128 c;
            Multiply(out c, ref a, ref b);
            return c;
        }

        public static Int128 operator /(Int128 a, Int32 b)
        {
            Int128 c;
            Divide(out c, ref a, b);
            return c;
        }

        public static Int128 operator /(Int128 a, UInt32 b)
        {
            Int128 c;
            Divide(out c, ref a, b);
            return c;
        }

        public static Int128 operator /(Int128 a, Int64 b)
        {
            Int128 c;
            Divide(out c, ref a, b);
            return c;
        }

        public static Int128 operator /(Int128 a, UInt64 b)
        {
            Int128 c;
            Divide(out c, ref a, b);
            return c;
        }

        public static Int128 operator /(Int128 a, Int128 b)
        {
            Int128 c;
            Divide(out c, ref a, ref b);
            return c;
        }

        public static Int32 operator %(Int128 a, Int32 b)
        {
            return Remainder(ref a, b);
        }

        public static Int32 operator %(Int128 a, UInt32 b)
        {
            return Remainder(ref a, b);
        }

        public static Int64 operator %(Int128 a, Int64 b)
        {
            return Remainder(ref a, b);
        }

        public static Int64 operator %(Int128 a, UInt64 b)
        {
            return Remainder(ref a, b);
        }

        public static Int128 operator %(Int128 a, Int128 b)
        {
            Int128 c;
            Remainder(out c, ref a, ref b);
            return c;
        }

        public static bool operator <(Int128 a, UInt128 b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator <(UInt128 a, Int128 b)
        {
            return b.CompareTo(a) > 0;
        }

        public static bool operator <(Int128 a, Int128 b)
        {
            return LessThan(ref a.v, ref b.v);
        }

        public static bool operator <(Int128 a, Int32 b)
        {
            return LessThan(ref a.v, b);
        }

        public static bool operator <(Int32 a, Int128 b)
        {
            return LessThan(a, ref b.v);
        }

        public static bool operator <(Int128 a, UInt32 b)
        {
            return LessThan(ref a.v, b);
        }

        public static bool operator <(UInt32 a, Int128 b)
        {
            return LessThan(a, ref b.v);
        }

        public static bool operator <(Int128 a, Int64 b)
        {
            return LessThan(ref a.v, b);
        }

        public static bool operator <(Int64 a, Int128 b)
        {
            return LessThan(a, ref b.v);
        }

        public static bool operator <(Int128 a, UInt64 b)
        {
            return LessThan(ref a.v, b);
        }

        public static bool operator <(UInt64 a, Int128 b)
        {
            return LessThan(a, ref b.v);
        }

        public static bool operator <=(Int128 a, UInt128 b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator <=(UInt128 a, Int128 b)
        {
            return b.CompareTo(a) >= 0;
        }

        public static bool operator <=(Int128 a, Int128 b)
        {
            return !LessThan(ref b.v, ref a.v);
        }

        public static bool operator <=(Int128 a, Int32 b)
        {
            return !LessThan(b, ref a.v);
        }

        public static bool operator <=(Int32 a, Int128 b)
        {
            return !LessThan(ref b.v, a);
        }

        public static bool operator <=(Int128 a, UInt32 b)
        {
            return !LessThan(b, ref a.v);
        }

        public static bool operator <=(UInt32 a, Int128 b)
        {
            return !LessThan(ref b.v, a);
        }

        public static bool operator <=(Int128 a, Int64 b)
        {
            return !LessThan(b, ref a.v);
        }

        public static bool operator <=(Int64 a, Int128 b)
        {
            return !LessThan(ref b.v, a);
        }

        public static bool operator <=(Int128 a, UInt64 b)
        {
            return !LessThan(b, ref a.v);
        }

        public static bool operator <=(UInt64 a, Int128 b)
        {
            return !LessThan(ref b.v, a);
        }

        public static bool operator >(Int128 a, UInt128 b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator >(UInt128 a, Int128 b)
        {
            return b.CompareTo(a) < 0;
        }

        public static bool operator >(Int128 a, Int128 b)
        {
            return LessThan(ref b.v, ref a.v);
        }

        public static bool operator >(Int128 a, Int32 b)
        {
            return LessThan(b, ref a.v);
        }

        public static bool operator >(Int32 a, Int128 b)
        {
            return LessThan(ref b.v, a);
        }

        public static bool operator >(Int128 a, UInt32 b)
        {
            return LessThan(b, ref a.v);
        }

        public static bool operator >(UInt32 a, Int128 b)
        {
            return LessThan(ref b.v, a);
        }

        public static bool operator >(Int128 a, Int64 b)
        {
            return LessThan(b, ref a.v);
        }

        public static bool operator >(Int64 a, Int128 b)
        {
            return LessThan(ref b.v, a);
        }

        public static bool operator >(Int128 a, UInt64 b)
        {
            return LessThan(b, ref a.v);
        }

        public static bool operator >(UInt64 a, Int128 b)
        {
            return LessThan(ref b.v, a);
        }

        public static bool operator >=(Int128 a, UInt128 b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator >=(UInt128 a, Int128 b)
        {
            return b.CompareTo(a) <= 0;
        }

        public static bool operator >=(Int128 a, Int128 b)
        {
            return !LessThan(ref a.v, ref b.v);
        }

        public static bool operator >=(Int128 a, Int32 b)
        {
            return !LessThan(ref a.v, b);
        }

        public static bool operator >=(Int32 a, Int128 b)
        {
            return !LessThan(a, ref b.v);
        }

        public static bool operator >=(Int128 a, UInt32 b)
        {
            return !LessThan(ref a.v, b);
        }

        public static bool operator >=(UInt32 a, Int128 b)
        {
            return !LessThan(a, ref b.v);
        }

        public static bool operator >=(Int128 a, Int64 b)
        {
            return !LessThan(ref a.v, b);
        }

        public static bool operator >=(Int64 a, Int128 b)
        {
            return !LessThan(a, ref b.v);
        }

        public static bool operator >=(Int128 a, UInt64 b)
        {
            return !LessThan(ref a.v, b);
        }

        public static bool operator >=(UInt64 a, Int128 b)
        {
            return !LessThan(a, ref b.v);
        }

        public static bool operator ==(UInt128 a, Int128 b)
        {
            return b.Equals(a);
        }

        public static bool operator ==(Int128 a, UInt128 b)
        {
            return a.Equals(b);
        }

        public static bool operator ==(Int128 a, Int128 b)
        {
            return a.v.Equals(b.v);
        }

        public static bool operator ==(Int128 a, Int32 b)
        {
            return a.Equals(b);
        }

        public static bool operator ==(Int32 a, Int128 b)
        {
            return b.Equals(a);
        }

        public static bool operator ==(Int128 a, UInt32 b)
        {
            return a.Equals(b);
        }

        public static bool operator ==(UInt32 a, Int128 b)
        {
            return b.Equals(a);
        }

        public static bool operator ==(Int128 a, Int64 b)
        {
            return a.Equals(b);
        }

        public static bool operator ==(Int64 a, Int128 b)
        {
            return b.Equals(a);
        }

        public static bool operator ==(Int128 a, UInt64 b)
        {
            return a.Equals(b);
        }

        public static bool operator ==(UInt64 a, Int128 b)
        {
            return b.Equals(a);
        }

        public static bool operator !=(UInt128 a, Int128 b)
        {
            return !b.Equals(a);
        }

        public static bool operator !=(Int128 a, UInt128 b)
        {
            return !a.Equals(b);
        }

        public static bool operator !=(Int128 a, Int128 b)
        {
            return !a.v.Equals(b.v);
        }

        public static bool operator !=(Int128 a, Int32 b)
        {
            return !a.Equals(b);
        }

        public static bool operator !=(Int32 a, Int128 b)
        {
            return !b.Equals(a);
        }

        public static bool operator !=(Int128 a, UInt32 b)
        {
            return !a.Equals(b);
        }

        public static bool operator !=(UInt32 a, Int128 b)
        {
            return !b.Equals(a);
        }

        public static bool operator !=(Int128 a, Int64 b)
        {
            return !a.Equals(b);
        }

        public static bool operator !=(Int64 a, Int128 b)
        {
            return !b.Equals(a);
        }

        public static bool operator !=(Int128 a, UInt64 b)
        {
            return !a.Equals(b);
        }

        public static bool operator !=(UInt64 a, Int128 b)
        {
            return !b.Equals(a);
        }

        public Int32 CompareTo(UInt128 other)
        {
            if (IsNegative)
                return -1;
            return v.CompareTo(other);
        }

        public Int32 CompareTo(Int128 other)
        {
            return SignedCompare(ref v, other.S0, other.S1);
        }

        public Int32 CompareTo(Int32 other)
        {
            return SignedCompare(ref v, (UInt64)other, (UInt64)(other >> 31));
        }

        public Int32 CompareTo(UInt32 other)
        {
            return SignedCompare(ref v, (UInt64)other, 0);
        }

        public Int32 CompareTo(Int64 other)
        {
            return SignedCompare(ref v, (UInt64)other, (UInt64)(other >> 63));
        }

        public Int32 CompareTo(UInt64 other)
        {
            return SignedCompare(ref v, other, 0);
        }

        public Int32 CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is Int128))
                throw new ArgumentException();
            return CompareTo((Int128)obj);
        }

        private static bool LessThan(ref UInt128 a, ref UInt128 b)
        {
            var as1 = (Int64)a.S1;
            var bs1 = (Int64)b.S1;
            if (as1 != bs1)
                return as1 < bs1;
            return a.S0 < b.S0;
        }

        private static bool LessThan(ref UInt128 a, Int64 b)
        {
            var as1 = (Int64)a.S1;
            var bs1 = b >> 63;
            if (as1 != bs1)
                return as1 < bs1;
            return a.S0 < (UInt64)b;
        }

        private static bool LessThan(Int64 a, ref UInt128 b)
        {
            var as1 = a >> 63;
            var bs1 = (Int64)b.S1;
            if (as1 != bs1)
                return as1 < bs1;
            return (UInt64)a < b.S0;
        }

        private static bool LessThan(ref UInt128 a, UInt64 b)
        {
            var as1 = (Int64)a.S1;
            if (as1 != 0)
                return as1 < 0;
            return a.S0 < b;
        }

        private static bool LessThan(UInt64 a, ref UInt128 b)
        {
            var bs1 = (Int64)b.S1;
            if (0 != bs1)
                return 0 < bs1;
            return a < b.S0;
        }

        private static Int32 SignedCompare(ref UInt128 a, UInt64 bs0, UInt64 bs1)
        {
            var as1 = a.S1;
            if (as1 != bs1)
                return ((Int64)as1).CompareTo((Int64)bs1);
            return a.S0.CompareTo(bs0);
        }

        public bool Equals(UInt128 other)
        {
            return !IsNegative && v.Equals(other);
        }

        public bool Equals(Int128 other)
        {
            return v.Equals(other.v);
        }

        public bool Equals(Int32 other)
        {
            if (other < 0)
                return v.S1 == UInt64.MaxValue && v.S0 == (UInt32)other;
            return v.S1 == 0 && v.S0 == (UInt32)other;
        }

        public bool Equals(UInt32 other)
        {
            return v.S1 == 0 && v.S0 == other;
        }

        public bool Equals(Int64 other)
        {
            if (other < 0)
                return v.S1 == UInt64.MaxValue && v.S0 == (UInt64)other;
            return v.S1 == 0 && v.S0 == (UInt64)other;
        }

        public bool Equals(UInt64 other)
        {
            return v.S1 == 0 && v.S0 == other;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Int128))
                return false;
            return Equals((Int128)obj);
        }

        public override Int32 GetHashCode()
        {
            return v.GetHashCode();
        }

        public static void Multiply(out Int128 c, ref Int128 a, Int32 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b < 0)
                    UInt128.Multiply(out c.v, ref aneg, (UInt32)(-b));
                else
                {
                    UInt128.Multiply(out c.v, ref aneg, (UInt32)b);
                    UInt128.Negate(ref c.v);
                }
            }
            else
            {
                if (b < 0)
                {
                    UInt128.Multiply(out c.v, ref a.v, (UInt32)(-b));
                    UInt128.Negate(ref c.v);
                }
                else
                    UInt128.Multiply(out c.v, ref a.v, (UInt32)b);
            }
            Debug.Assert((BigInteger)c == (BigInteger)a * (BigInteger)b);
        }

        public static void Multiply(out Int128 c, ref Int128 a, UInt32 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                UInt128.Multiply(out c.v, ref aneg, b);
                UInt128.Negate(ref c.v);
            }
            else
                UInt128.Multiply(out c.v, ref a.v, b);
            Debug.Assert((BigInteger)c == (BigInteger)a * (BigInteger)b);
        }

        public static void Multiply(out Int128 c, ref Int128 a, Int64 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b < 0)
                    UInt128.Multiply(out c.v, ref aneg, (UInt64)(-b));
                else
                {
                    UInt128.Multiply(out c.v, ref aneg, (UInt64)b);
                    UInt128.Negate(ref c.v);
                }
            }
            else
            {
                if (b < 0)
                {
                    UInt128.Multiply(out c.v, ref a.v, (UInt64)(-b));
                    UInt128.Negate(ref c.v);
                }
                else
                    UInt128.Multiply(out c.v, ref a.v, (UInt64)b);
            }
            Debug.Assert((BigInteger)c == (BigInteger)a * (BigInteger)b);
        }

        public static void Multiply(out Int128 c, ref Int128 a, UInt64 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                UInt128.Multiply(out c.v, ref aneg, b);
                UInt128.Negate(ref c.v);
            }
            else
                UInt128.Multiply(out c.v, ref a.v, b);
            Debug.Assert((BigInteger)c == (BigInteger)a * (BigInteger)b);
        }

        public static void Multiply(out Int128 c, ref Int128 a, ref Int128 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.Multiply(out c.v, ref aneg, ref bneg);
                }
                else
                {
                    UInt128.Multiply(out c.v, ref aneg, ref b.v);
                    UInt128.Negate(ref c.v);
                }
            }
            else
            {
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.Multiply(out c.v, ref a.v, ref bneg);
                    UInt128.Negate(ref c.v);
                }
                else
                    UInt128.Multiply(out c.v, ref a.v, ref b.v);
            }
            Debug.Assert((BigInteger)c == (BigInteger)a * (BigInteger)b);
        }

        public static void Divide(out Int128 c, ref Int128 a, Int32 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b < 0)
                    UInt128.Multiply(out c.v, ref aneg, (UInt32)(-b));
                else
                {
                    UInt128.Multiply(out c.v, ref aneg, (UInt32)b);
                    UInt128.Negate(ref c.v);
                }
            }
            else
            {
                if (b < 0)
                {
                    UInt128.Multiply(out c.v, ref a.v, (UInt32)(-b));
                    UInt128.Negate(ref c.v);
                }
                else
                    UInt128.Multiply(out c.v, ref a.v, (UInt32)b);
            }
            Debug.Assert((BigInteger)c == (BigInteger)a / (BigInteger)b);
        }

        public static void Divide(out Int128 c, ref Int128 a, UInt32 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                UInt128.Divide(out c.v, ref aneg, b);
                UInt128.Negate(ref c.v);
            }
            else
                UInt128.Divide(out c.v, ref a.v, b);
            Debug.Assert((BigInteger)c == (BigInteger)a / (BigInteger)b);
        }

        public static void Divide(out Int128 c, ref Int128 a, Int64 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b < 0)
                    UInt128.Divide(out c.v, ref aneg, (UInt64)(-b));
                else
                {
                    UInt128.Divide(out c.v, ref aneg, (UInt64)b);
                    UInt128.Negate(ref c.v);
                }
            }
            else
            {
                if (b < 0)
                {
                    UInt128.Divide(out c.v, ref a.v, (UInt64)(-b));
                    UInt128.Negate(ref c.v);
                }
                else
                    UInt128.Divide(out c.v, ref a.v, (UInt64)b);
            }
            Debug.Assert((BigInteger)c == (BigInteger)a / (BigInteger)b);
        }

        public static void Divide(out Int128 c, ref Int128 a, UInt64 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                UInt128.Divide(out c.v, ref aneg, b);
                UInt128.Negate(ref c.v);
            }
            else
                UInt128.Divide(out c.v, ref a.v, b);
            Debug.Assert((BigInteger)c == (BigInteger)a / (BigInteger)b);
        }

        public static void Divide(out Int128 c, ref Int128 a, ref Int128 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.Divide(out c.v, ref aneg, ref bneg);
                }
                else
                {
                    UInt128.Divide(out c.v, ref aneg, ref b.v);
                    UInt128.Negate(ref c.v);
                }
            }
            else
            {
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.Divide(out c.v, ref a.v, ref bneg);
                    UInt128.Negate(ref c.v);
                }
                else
                    UInt128.Divide(out c.v, ref a.v, ref b.v);
            }
            Debug.Assert((BigInteger)c == (BigInteger)a / (BigInteger)b);
        }

        public static Int32 Remainder(ref Int128 a, Int32 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b < 0)
                    return (Int32)UInt128.Remainder(ref aneg, (UInt32)(-b));
                else
                    return -(Int32)UInt128.Remainder(ref aneg, (UInt32)b);
            }
            else
            {
                if (b < 0)
                    return -(Int32)UInt128.Remainder(ref a.v, (UInt32)(-b));
                else
                    return (Int32)UInt128.Remainder(ref a.v, (UInt32)b);
            }
        }

        public static Int32 Remainder(ref Int128 a, UInt32 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                return -(Int32)UInt128.Remainder(ref aneg, b);
            }
            else
                return (Int32)UInt128.Remainder(ref a.v, b);
        }

        public static Int64 Remainder(ref Int128 a, Int64 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b < 0)
                    return (Int64)UInt128.Remainder(ref aneg, (UInt64)(-b));
                else
                    return -(Int64)UInt128.Remainder(ref aneg, (UInt64)b);
            }
            else
            {
                if (b < 0)
                    return -(Int64)UInt128.Remainder(ref a.v, (UInt64)(-b));
                else
                    return (Int64)UInt128.Remainder(ref a.v, (UInt64)b);
            }
        }

        public static Int64 Remainder(ref Int128 a, UInt64 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                return -(Int64)UInt128.Remainder(ref aneg, b);
            }
            else
                return (Int64)UInt128.Remainder(ref a.v, b);
        }

        public static void Remainder(out Int128 c, ref Int128 a, ref Int128 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.Remainder(out c.v, ref aneg, ref bneg);
                }
                else
                {
                    UInt128.Remainder(out c.v, ref aneg, ref b.v);
                    UInt128.Negate(ref c.v);
                }
            }
            else
            {
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.Remainder(out c.v, ref a.v, ref bneg);
                    UInt128.Negate(ref c.v);
                }
                else
                    UInt128.Remainder(out c.v, ref a.v, ref b.v);
            }
            Debug.Assert((BigInteger)c == (BigInteger)a % (BigInteger)b);
        }

        public static Int128 Abs(Int128 a)
        {
            if (!a.IsNegative)
                return a;
            Int128 c;
            UInt128.Negate(out c.v, ref a.v);
            return c;
        }

        public static Int128 Square(Int64 a)
        {
            if (a < 0)
                a = -a;
            Int128 c;
            UInt128.Square(out c.v, (UInt64)a);
            return c;
        }

        public static Int128 Square(Int128 a)
        {
            Int128 c;
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                UInt128.Square(out c.v, ref aneg);
            }
            else
                UInt128.Square(out c.v, ref a.v);
            return c;
        }

        public static Int128 Cube(Int64 a)
        {
            Int128 c;
            if (a < 0)
            {
                UInt128.Cube(out c.v, (UInt64)(-a));
                UInt128.Negate(ref c.v);
            }
            else
                UInt128.Cube(out c.v, (UInt64)a);
            return c;
        }

        public static Int128 Cube(Int128 a)
        {
            Int128 c;
            if (a < 0)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                UInt128.Cube(out c.v, ref aneg);
                UInt128.Negate(ref c.v);
            }
            else
                UInt128.Cube(out c.v, ref a.v);
            return c;
        }

        public static void Add(ref Int128 a, Int64 b)
        {
            if (b < 0)
                UInt128.Subtract(ref a.v, (UInt64)(-b));
            else
                UInt128.Add(ref a.v, (UInt64)b);
        }

        public static void Add(ref Int128 a, ref Int128 b)
        {
            UInt128.Add(ref a.v, ref b.v);
        }

        public static void Subtract(ref Int128 a, Int64 b)
        {
            if (b < 0)
                UInt128.Add(ref a.v, (UInt64)(-b));
            else
                UInt128.Subtract(ref a.v, (UInt64)b);
        }

        public static void Subtract(ref Int128 a, ref Int128 b)
        {
            UInt128.Subtract(ref a.v, ref b.v);
        }

        public static void Add(ref Int128 a, Int128 b)
        {
            UInt128.Add(ref a.v, ref b.v);
        }

        public static void Subtract(ref Int128 a, Int128 b)
        {
            UInt128.Subtract(ref a.v, ref b.v);
        }

        public static void AddProduct(ref Int128 a, ref UInt128 b, Int32 c)
        {
            UInt128 product;
            if (c < 0)
            {
                UInt128.Multiply(out product, ref b, (UInt32)(-c));
                UInt128.Subtract(ref a.v, ref product);
            }
            else
            {
                UInt128.Multiply(out product, ref b, (UInt32)c);
                UInt128.Add(ref a.v, ref product);
            }
        }

        public static void AddProduct(ref Int128 a, ref UInt128 b, Int64 c)
        {
            UInt128 product;
            if (c < 0)
            {
                UInt128.Multiply(out product, ref b, (UInt64)(-c));
                UInt128.Subtract(ref a.v, ref product);
            }
            else
            {
                UInt128.Multiply(out product, ref b, (UInt64)c);
                UInt128.Add(ref a.v, ref product);
            }
        }

        public static void SubtractProduct(ref Int128 a, ref UInt128 b, Int32 c)
        {
            UInt128 d;
            if (c < 0)
            {
                UInt128.Multiply(out d, ref b, (UInt32)(-c));
                UInt128.Add(ref a.v, ref d);
            }
            else
            {
                UInt128.Multiply(out d, ref b, (UInt32)c);
                UInt128.Subtract(ref a.v, ref d);
            }
        }

        public static void SubtractProduct(ref Int128 a, ref UInt128 b, Int64 c)
        {
            UInt128 d;
            if (c < 0)
            {
                UInt128.Multiply(out d, ref b, (UInt64)(-c));
                UInt128.Add(ref a.v, ref d);
            }
            else
            {
                UInt128.Multiply(out d, ref b, (UInt64)c);
                UInt128.Subtract(ref a.v, ref d);
            }
        }

        public static void AddProduct(ref Int128 a, UInt128 b, Int32 c)
        {
            AddProduct(ref a, ref b, c);
        }

        public static void AddProduct(ref Int128 a, UInt128 b, Int64 c)
        {
            AddProduct(ref a, ref b, c);
        }

        public static void SubtractProduct(ref Int128 a, UInt128 b, Int32 c)
        {
            SubtractProduct(ref a, ref b, c);
        }

        public static void SubtractProduct(ref Int128 a, UInt128 b, Int64 c)
        {
            SubtractProduct(ref a, ref b, c);
        }

        public static void Pow(out Int128 result, ref Int128 value, Int32 exponent)
        {
            if (exponent < 0)
                throw new ArgumentException("exponent must not be negative");
            if (value.IsNegative)
            {
                UInt128 valueneg;
                UInt128.Negate(out valueneg, ref value.v);
                if ((exponent & 1) == 0)
                    UInt128.Pow(out result.v, ref valueneg, (UInt32)exponent);
                else
                {
                    UInt128.Pow(out result.v, ref valueneg, (UInt32)exponent);
                    UInt128.Negate(ref result.v);
                }
            }
            else
                UInt128.Pow(out result.v, ref value.v, (UInt32)exponent);
        }

        public static Int128 Pow(Int128 value, Int32 exponent)
        {
            Int128 result;
            Pow(out result, ref value, exponent);
            return result;
        }

        public static UInt64 FloorSqrt(Int128 a)
        {
            if (a.IsNegative)
                throw new ArgumentException("argument must not be negative");
            return UInt128.FloorSqrt(a.v);
        }

        public static UInt64 CeilingSqrt(Int128 a)
        {
            if (a.IsNegative)
                throw new ArgumentException("argument must not be negative");
            return UInt128.CeilingSqrt(a.v);
        }

        public static Int64 FloorCbrt(Int128 a)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                return -(Int64)UInt128.FloorCbrt(aneg);
            }
            return (Int64)UInt128.FloorCbrt(a.v);
        }

        public static Int64 CeilingCbrt(Int128 a)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                return -(Int64)UInt128.CeilingCbrt(aneg);
            }
            return (Int64)UInt128.CeilingCbrt(a.v);
        }

        public static Int128 Min(Int128 a, Int128 b)
        {
            if (LessThan(ref a.v, ref b.v))
                return a;
            return b;
        }

        public static Int128 Max(Int128 a, Int128 b)
        {
            if (LessThan(ref b.v, ref a.v))
                return a;
            return b;
        }

        public static double Log(Int128 a)
        {
            return Log(a, Math.E);
        }

        public static double Log10(Int128 a)
        {
            return Log(a, 10);
        }

        public static double Log(Int128 a, double b)
        {
            if (a.IsNegative || a.IsZero)
                throw new ArgumentException("argument must be positive");
            return Math.Log(UInt128.ConvertToDouble(ref a.v), b);
        }

        public static Int128 Add(Int128 a, Int128 b)
        {
            Int128 c;
            UInt128.Add(out c.v, ref a.v, ref b.v);
            return c;
        }

        public static Int128 Subtract(Int128 a, Int128 b)
        {
            Int128 c;
            UInt128.Subtract(out c.v, ref a.v, ref b.v);
            return c;
        }

        public static Int128 Multiply(Int128 a, Int128 b)
        {
            Int128 c;
            Multiply(out c, ref a, ref b);
            return c;
        }

        public static Int128 Divide(Int128 a, Int128 b)
        {
            Int128 c;
            Divide(out c, ref a, ref b);
            return c;
        }

        public static Int128 Remainder(Int128 a, Int128 b)
        {
            Int128 c;
            Remainder(out c, ref a, ref b);
            return c;
        }

        public static Int128 DivRem(Int128 a, Int128 b, out Int128 remainder)
        {
            Int128 c;
            Divide(out c, ref a, ref b);
            Remainder(out remainder, ref a, ref b);
            return c;
        }

        public static Int128 Negate(Int128 a)
        {
            Int128 c;
            UInt128.Negate(out c.v, ref a.v);
            return c;
        }

        public static Int128 GreatestCommonDivisor(Int128 a, Int128 b)
        {
            Int128 c;
            GreatestCommonDivisor(out c, ref a, ref b);
            return c;
        }

        public static void GreatestCommonDivisor(out Int128 c, ref Int128 a, ref Int128 b)
        {
            if (a.IsNegative)
            {
                UInt128 aneg;
                UInt128.Negate(out aneg, ref a.v);
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.GreatestCommonDivisor(out c.v, ref aneg, ref bneg);
                }
                else
                    UInt128.GreatestCommonDivisor(out c.v, ref aneg, ref b.v);
            }
            else
            {
                if (b.IsNegative)
                {
                    UInt128 bneg;
                    UInt128.Negate(out bneg, ref b.v);
                    UInt128.GreatestCommonDivisor(out c.v, ref a.v, ref bneg);
                }
                else
                    UInt128.GreatestCommonDivisor(out c.v, ref a.v, ref b.v);
            }
        }

        public static void LeftShift(ref Int128 c, Int32 d)
        {
            UInt128.LeftShift(ref c.v, d);
        }

        public static void LeftShift(ref Int128 c)
        {
            UInt128.LeftShift(ref c.v);
        }

        public static void RightShift(ref Int128 c, Int32 d)
        {
            UInt128.ArithmeticRightShift(ref c.v, d);
        }

        public static void RightShift(ref Int128 c)
        {
            UInt128.ArithmeticRightShift(ref c.v);
        }

        public static void Swap(ref Int128 a, ref Int128 b)
        {
            UInt128.Swap(ref a.v, ref b.v);
        }

        public static Int32 Compare(Int128 a, Int128 b)
        {
            return a.CompareTo(b);
        }

        public static void Shift(out Int128 c, ref Int128 a, Int32 d)
        {
            UInt128.ArithmeticShift(out c.v, ref a.v, d);
        }

        public static void Shift(ref Int128 c, Int32 d)
        {
            UInt128.ArithmeticShift(ref c.v, d);
        }

        public static Int128 ModAdd(Int128 a, Int128 b, Int128 modulus)
        {
            Int128 c;
            UInt128.ModAdd(out c.v, ref a.v, ref b.v, ref modulus.v);
            return c;
        }

        public static Int128 ModSub(Int128 a, Int128 b, Int128 modulus)
        {
            Int128 c;
            UInt128.ModSub(out c.v, ref a.v, ref b.v, ref modulus.v);
            return c;
        }

        public static Int128 ModMul(Int128 a, Int128 b, Int128 modulus)
        {
            Int128 c;
            UInt128.ModMul(out c.v, ref a.v, ref b.v, ref modulus.v);
            return c;
        }

        public static Int128 ModPow(Int128 value, Int128 exponent, Int128 modulus)
        {
            Int128 result;
            UInt128.ModPow(out result.v, ref value.v, ref exponent.v, ref modulus.v);
            return result;
        }
    }
}
