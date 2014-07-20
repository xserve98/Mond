﻿using System.Globalization;
using Mond.VirtualMachine;

namespace Mond
{
    public partial struct MondValue
    {
        public static implicit operator MondValue(bool value)
        {
            return new MondValue(value ? MondValueType.True : MondValueType.False);
        }

        public static implicit operator MondValue(double value)
        {
            return new MondValue(value);
        }

        public static implicit operator MondValue(string value)
        {
            return new MondValue(value);
        }

        public static implicit operator MondValue(MondFunction function)
        {
            return new MondValue(new Closure(function));
        }

        public static implicit operator MondValue(MondInstanceFunction function)
        {
            return new MondValue(new Closure(function));
        }

        public static implicit operator bool(MondValue value)
        {
            switch (value.Type)
            {
                case MondValueType.Undefined:
                case MondValueType.Null:
                case MondValueType.False:
                    return false;

                case MondValueType.Number:
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    return value.NumberValue != 0;

                default:
                    return true;
            }
        }

        public static implicit operator double(MondValue value)
        {
            if (value.Type != MondValueType.Number)
                throw new MondRuntimeException(RuntimeError.CantCastTo, value.Type, MondValueType.Number);

            return value.NumberValue;
        }

        public static implicit operator string(MondValue value)
        {
            if (value.Type != MondValueType.String)
                throw new MondRuntimeException(RuntimeError.CantCastTo, value.Type, MondValueType.String);

            return value.StringValue;
        }

        public static MondValue operator +(MondValue left, MondValue right)
        {
            var isString = left.Type == MondValueType.String || right.Type == MondValueType.String;

            if (isString)
                return new MondValue(left.ToString() + right.ToString());

            if (left.Type != MondValueType.Number || right.Type != MondValueType.Number)
                throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "addition", left.Type, right.Type);

            return new MondValue(left.NumberValue + right.NumberValue);
        }

        public static MondValue operator -(MondValue left, MondValue right)
        {
            if (left.Type != MondValueType.Number || right.Type != MondValueType.Number)
                throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "subtraction", left.Type, right.Type);

            return new MondValue(left.NumberValue - right.NumberValue);
        }

        public static MondValue operator *(MondValue left, MondValue right)
        {
            if (left.Type != MondValueType.Number || right.Type != MondValueType.Number)
                throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "multiplication", left.Type, right.Type);

            return new MondValue(left.NumberValue * right.NumberValue);
        }

        public static MondValue operator /(MondValue left, MondValue right)
        {
            if (left.Type != MondValueType.Number || right.Type != MondValueType.Number)
                throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "division", left.Type, right.Type);

            return new MondValue(left.NumberValue / right.NumberValue);
        }

        public static MondValue operator %(MondValue left, MondValue right)
        {
            if (left.Type != MondValueType.Number || right.Type != MondValueType.Number)
                throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "modulo", left.Type, right.Type);

            return new MondValue(left.NumberValue % right.NumberValue);
        }

        public static MondValue operator -(MondValue value)
        {
            if (value.Type != MondValueType.Number)
                throw new MondRuntimeException(RuntimeError.CantUseOperatorOnType, "negation", value.Type);

            return new MondValue(-value.NumberValue);
        }

        public static bool operator ==(MondValue left, MondValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MondValue left, MondValue right)
        {
            return !(left == right);
        }

        public static bool operator >(MondValue left, MondValue right)
        {
            switch (left.Type)
            {
                case MondValueType.Number:
                    if (right.Type != MondValueType.Number)
                        throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "relational", left.Type, right.Type);

                    return left.NumberValue > right.NumberValue;

                case MondValueType.String:
                    if (right.Type != MondValueType.String)
                        throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "relational", left.Type, right.Type);

                    return string.Compare(left.StringValue, right.StringValue, CultureInfo.InvariantCulture, CompareOptions.None) > 0;

                default:
                    throw new MondRuntimeException(RuntimeError.CantUseOperatorOnTypes, "relational", left.Type, right.Type);
            }
        }

        public static bool operator >=(MondValue left, MondValue right)
        {
            return left > right || left == right;
        }

        public static bool operator <(MondValue left, MondValue right)
        {
            return !(left >= right);
        }

        public static bool operator <=(MondValue left, MondValue right)
        {
            return !(left > right);
        }
    }
}