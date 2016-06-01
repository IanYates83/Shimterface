﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shimterface.Tests
{
    [TestClass]
    public class MissingMemberTests
    {
        public interface IUnknownMethodTest
        {
            void UnknownMethod();
        }
        public interface IPropertyWithoutSetTest
        {
            string PropertyWithoutSet { get; set; }
        }

        public class TestClass
        {
            public string PropertyWithoutSet { get { return null; } }
        }

		[TestInitialize]
		public void init()
		{
			Shimterface.ResetState();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Cannot_shim_null()
		{
			Shimterface.Shim<IUnknownMethodTest>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidCastException))]
		public void All_interface_members_must_exist_in_object()
		{
			var obj = new TestClass();

			Shimterface.Shim<IUnknownMethodTest>(obj);
		}

		[TestMethod]
        public void Can_choose_to_ignore_missing_members_on_creation()
        {
            var obj = new TestClass();

            Shimterface.IgnoreMissingMembers<IUnknownMethodTest>();
            Shimterface.Shim<IUnknownMethodTest>(obj);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Ignored_missing_members_cannot_be_invoked()
        {
            var obj = new TestClass();

            Shimterface.IgnoreMissingMembers<IUnknownMethodTest>();
            var shim = Shimterface.Shim<IUnknownMethodTest>(obj);

            shim.UnknownMethod();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Properties_must_contain_all_required_parts()
        {
            var obj = new TestClass();

            Shimterface.Shim<IPropertyWithoutSetTest>(obj);
        }
    }
}
