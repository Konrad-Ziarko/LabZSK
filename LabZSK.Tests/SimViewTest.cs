// <copyright file="SimViewTest.cs" company="Ziarko Konrad Tomasz - Wojskowa Akademia Techniczna">Copyright ©  2016</copyright>
using System;
using LabZSK.Simulation;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LabZSK.Simulation.Tests
{
    /// <summary>This class contains parameterized unit tests for SimView</summary>
    [PexClass(typeof(SimView))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class SimViewTest
    {
    }
}
