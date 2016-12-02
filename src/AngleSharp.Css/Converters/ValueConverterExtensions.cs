﻿namespace AngleSharp.Css.Converters
{
    using AngleSharp.Css.Dom;
    using AngleSharp.Css.Parser;
    using AngleSharp.Css.Values;
    using AngleSharp.Text;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Essential extensions for using the value converters.
    /// </summary>
    static class ValueConverterExtensions
    {
        public static ICssValue Convert(this IValueConverter converter, String value)
        {
            var source = new StringSource(value);
            source.SkipSpacesAndComments();
            var result = converter.Convert(source);
            source.SkipSpacesAndComments();
            return source.IsDone ? result : null;
        }

        public static ICssValue ConvertFromProperty(this IValueConverter converter, String value)
        {
            var source = new StringSource(value);
            source.SkipSpacesAndComments();
            var result = converter.ConvertValueOrInherit(source);
            source.SkipSpacesAndComments();
            return source.IsDone ? result : null;
        }

        private static ICssValue ConvertValueOrInherit(this IValueConverter converter, StringSource source)
        {
            if (!source.IsIdentifier(CssKeywords.Inherit))
            {
                 return converter.Convert(source);
            }

            return Inherit.Instance;
        }

        public static IValueConverter Many(this IValueConverter converter, Int32 min = 1, Int32 max = UInt16.MaxValue)
        {
            return new OneOrMoreValueConverter(converter, min, max);
        }

        public static IValueConverter FromList(this IValueConverter converter)
        {
            return new ListValueConverter(converter);
        }

        public static IValueConverter ToConverter<T>(this Dictionary<String, T> values)
        {
            return new DictionaryValueConverter<T>(values);
        }

        public static IValueConverter Periodic(this IValueConverter converter)
        {
            return new PeriodicValueConverter(converter);
        }

        public static IValueConverter Option(this IValueConverter converter)
        {
            return new OptionValueConverter<Object>(converter, null);
        }

        public static IValueConverter Option<T>(this IValueConverter converter, T defaultValue)
        {
            return new OptionValueConverter<T>(converter, defaultValue);
        }

        public static IValueConverter For(this IValueConverter converter, params String[] labels)
        {
            return converter;
        }
        public static String Join(this ICssValue[] values, String separator)
        {
            var buffer = StringBuilderPool.Obtain();
            var previous = false;

            for (var i = 0; i < values.Length; i++)
            {
                var str = values[i]?.CssText;

                if (!String.IsNullOrEmpty(str))
                {
                    if (previous)
                    {
                        buffer.Append(separator);
                    }

                    buffer.Append(str);
                    previous = true;
                }
            }

            return buffer.ToPool();
        }

        public static String Join<T>(this T[] values, String separator)
        {
            var buffer = StringBuilderPool.Obtain();
            var previous = false;

            for (var i = 0; i < values.Length; i++)
            {
                var str = values[i].ToString();

                if (!String.IsNullOrEmpty(str))
                {
                    if (previous)
                    {
                        buffer.Append(separator);
                    }

                    buffer.Append(str);
                    previous = true;
                }
            }

            return buffer.ToPool();
        }
    }
}
