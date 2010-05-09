﻿using System;
﻿using Norm.Commands.Qualifiers;
using System.Text.RegularExpressions;

namespace Norm
{
    /// <summary>
    /// Qualifier operations.
    /// Provides a way to specify some of the "special" qualifiers that can be used for querying.
    /// </summary>
    /// <remarks>
    /// This should remain in the Norm namespace so that it's available 
    /// automatically when someone is using a MongoCollection.
    /// </remarks>
    public class Q
    {
        /// <summary>
        /// Construct an "equals" qualifier testing for Null.
        /// </summary>
        /// <remarks>
        /// This is just sugar, it returns an object reference to null.
        /// </remarks>
        /// <returns></returns>
        public static object IsNull()
        {
            return (object)null;
        }

        /// <summary>
        /// Construct a {$ne : null} qualifier
        /// </summary>
        /// <returns></returns>
        public static NotEqualQualifier IsNotNull()
        {
            return Q.NotEqual(new bool?());
        }

        /// <summary>
        /// construct a $where qualifier
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static WhereQualifier Where(string expression)
        {
            return new WhereQualifier(expression);
        }

        /// <summary>
        /// Builds a $lt qualifier for the search.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static LessThanQualifier LessThan(double value)
        {
            return new LessThanQualifier(value);
        }

        /// <summary>
        /// Builds a $lte qualifier for the search.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static LessOrEqualQualifier LessOrEqual(double value)
        {
            return new LessOrEqualQualifier(value);
        }

        /// <summary>
        /// Builds a $gte qualifier for the search.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static GreaterOrEqualQualifier GreaterOrEqual(double value)
        {
            return new GreaterOrEqualQualifier(value);
        }

        /// <summary>
        /// Builds a $gt qualifier for the search.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static GreaterThanQualifier GreaterThan(double value)
        {
            return new GreaterThanQualifier(value);
        }

        /// <summary>
        /// Builds a $gt qualifier for the search.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static GreaterThanQualifier GreaterThan(DateTime value)
        {
            return new GreaterThanQualifier(value);
        }

        /// <summary>
        /// Builds an $all statement
        /// </summary>
        /// <typeparam name="T">Type to qualify</typeparam>
        /// <param name="all">All.</param>
        /// <returns></returns>
        public static AllQualifier<T> All<T>(params T[] all)
        {
            return new AllQualifier<T>(all);
        }

        /// <summary>
        /// Builds an $in qualifier statement.
        /// </summary>
        /// <typeparam name="T">Type to qualify</typeparam>
        /// <param name="inSet">The in set.</param>
        /// <returns></returns>
        public static InQualifier<T> In<T>(params T[] inSet)
        {
            return new InQualifier<T>(inSet);
        }

        /// <summary>
        /// Builds a $ne qualifier against the value.
        /// </summary>
        /// <typeparam name="T">Type to compare for equality</typeparam>
        /// <param name="test">The test.</param>
        /// <returns></returns>
        public static NotEqualQualifier NotEqual<T>(T test)
        {
            return new NotEqualQualifier(test);
        }

        /// <summary>
        /// Passes the value straight back to you, new { Property = "value"} will
        /// work just fine as a qualifier. Here for the sake of consistency.
        /// </summary>
        /// <typeparam name="T">Type to compare for equality</typeparam>
        /// <param name="test">The test.</param>
        /// <returns></returns>
        public static T Equals<T>(T test)
        {
            return test;
        }

        /// <summary>
        /// Builds a $size qualifier.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static SizeQualifier Size(double size)
        {
            return new SizeQualifier(size);
        }

        /// <summary>
        /// Builds an $nin qualifier statement.
        /// </summary>
        /// <typeparam name="T">Type to qualify</typeparam>
        /// <param name="inSet">The in set.</param>
        /// <returns></returns>
        public static NotInQualifier<T> NotIn<T>(params T[] inSet)
        {
            return new NotInQualifier<T>(inSet);
        }

        /// <summary>
        /// constructs a $elemMatch qualifier statement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchDoc"></param>
        /// <returns></returns>
        public static ElementMatch<T> ElementMatch<T>(T matchDoc)
        {
            return new ElementMatch<T>(matchDoc);
        }

        /// <summary>
        /// returns a constructed regex to be used to match the specified property name in the DB.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Regex Matches(String pattern)
        {
            return new Regex(pattern);
        }

        /// <summary>
        /// Builds an $exists qualifier for the search.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static ExistsQuallifier Exists(bool value)
        {
            return new ExistsQuallifier(value);
        }
    }
}